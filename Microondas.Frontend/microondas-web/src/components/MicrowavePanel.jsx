import { useEffect, useMemo, useState } from "react";
import NumericKeyboard from "./NumericKeyboard";
import ProgramList from "./ProgramList";
import CustomProgramForm from "./CustomProgramForm";
import { formatTime } from "../utils/formatTime";
import {
  getCurrentSession,
  pauseOrCancelHeating,
  quickStartHeating,
  resumeHeating,
  startManualHeating,
  startProgramHeating,
  tickHeating,
} from "../api/microwaveApi";
import { createCustomProgram, getPrograms } from "../api/programApi";

function MicrowavePanel({ onLogout }) {
  const [timeInSeconds, setTimeInSeconds] = useState("");
  const [power, setPower] = useState("");
  const [selectedField, setSelectedField] = useState("time");
  const [session, setSession] = useState(null);
  const [programs, setPrograms] = useState([]);
  const [programsLoading, setProgramsLoading] = useState(false);
  const [actionLoading, setActionLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  async function loadPrograms() {
    try {
      setProgramsLoading(true);
      const data = await getPrograms();
      setPrograms(data || []);
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setProgramsLoading(false);
    }
  }

  async function loadSession() {
    try {
      const data = await getCurrentSession();
      setSession(data);
    } catch {
      // se não existir endpoint ou vier vazio, não trava a tela
    }
  }

  useEffect(() => {
    loadPrograms();
    loadSession();
  }, []);

  useEffect(() => {
    if (!session?.isRunning) return;

    const timer = setInterval(async () => {
      try {
        const data = await tickHeating();
        setSession(data);
      } catch (err) {
        setError(getApiError(err));
        clearInterval(timer);
      }
    }, 1000);

    return () => clearInterval(timer);
  }, [session?.isRunning]);

  function clearMessages() {
    setMessage("");
    setError("");
  }

  function getApiError(err) {
    return (
      err?.response?.data?.message ||
      err?.response?.data?.title ||
      err?.message ||
      "Ocorreu um erro."
    );
  }

  function handleKeyboardNumber(number) {
    if (session?.isRunning || session?.isPaused) return;

    if (selectedField === "time") {
      setTimeInSeconds((old) => `${old}${number}`);
      return;
    }

    setPower((old) => `${old}${number}`);
  }

  function handleKeyboardClear() {
    if (session?.isRunning || session?.isPaused) return;

    if (selectedField === "time") {
      setTimeInSeconds("");
      return;
    }

    setPower("");
  }

  async function handleStartManual() {
    clearMessages();

    try {
      setActionLoading(true);

      const payload = {
        timeInSeconds: timeInSeconds === "" ? null : Number(timeInSeconds),
        power: power === "" ? null : Number(power),
      };

      const data = await startManualHeating(payload);
      setSession(data);
      setMessage("Aquecimento iniciado.");
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setActionLoading(false);
    }
  }

  async function handleQuickStart() {
    clearMessages();

    try {
      setActionLoading(true);
      const data = await quickStartHeating();
      setSession(data);
      setMessage("Início rápido acionado.");
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setActionLoading(false);
    }
  }

  async function handlePauseOrCancel() {
    clearMessages();

    try {
      setActionLoading(true);
      const data = await pauseOrCancelHeating();
      setSession(data);

      if (data?.isCancelled) {
        setTimeInSeconds("");
        setPower("");
        setMessage("Aquecimento cancelado.");
      } else if (data?.isPaused) {
        setMessage("Aquecimento pausado.");
      } else {
        setMessage("Campos limpos.");
        setTimeInSeconds("");
        setPower("");
      }
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setActionLoading(false);
    }
  }

  async function handleResume() {
    clearMessages();

    try {
      setActionLoading(true);
      const data = await resumeHeating();
      setSession(data);
      setMessage("Aquecimento retomado.");
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setActionLoading(false);
    }
  }

  async function handleSelectProgram(program) {
    clearMessages();

    try {
      setActionLoading(true);

      const data = await startProgramHeating(program.name);

      setSession(data);
      setTimeInSeconds(String(program.timeInSeconds));
      setPower(String(program.power));
      setMessage(`Programa ${program.name} iniciado.`);
    } catch (err) {
      setError(getApiError(err));
    } finally {
      setActionLoading(false);
    }
  }

  async function handleSaveCustomProgram(payload) {
    clearMessages();

    try {
      setActionLoading(true);
      await createCustomProgram(payload);
      setMessage("Programa customizado salvo com sucesso.");
      await loadPrograms();
      return true;
    } catch (err) {
      setError(getApiError(err));
      return false;
    } finally {
      setActionLoading(false);
    }
  }

  const statusText = useMemo(() => {
    if (!session) return "Parado";
    if (session.isFinished) return "Concluído";
    if (session.isCancelled) return "Cancelado";
    if (session.isPaused) return "Pausado";
    if (session.isRunning) return "Aquecendo";
    return "Parado";
  }, [session]);

  return (
    <div className="app-shell">
      <div className="top-bar">
        <h1>Micro-ondas digital</h1>
        <button type="button" onClick={onLogout}>
          Sair
        </button>
      </div>

      <div className="main-grid">
        <div className="left-column">
          <div className="microwave-card">
            <h2>Painel</h2>

            <div className="status-box">
              <div><strong>Status:</strong> {statusText}</div>
              <div>
                <strong>Tempo restante:</strong>{" "}
                {formatTime(session?.remainingTimeInSeconds ?? 0)}
              </div>
              <div>
                <strong>Potência:</strong> {session?.power ?? "-"}
              </div>
              <div>
                <strong>Programa:</strong> {session?.programName || "Manual"}
              </div>
            </div>

            <div className="display-box">
              <div className="display-line">
                <label>Tempo (segundos)</label>
                <input
                  value={timeInSeconds}
                  onFocus={() => setSelectedField("time")}
                  onChange={(e) => setTimeInSeconds(e.target.value)}
                  disabled={session?.isRunning || session?.isPaused}
                />
              </div>

              <div className="display-line">
                <label>Potência</label>
                <input
                  value={power}
                  onFocus={() => setSelectedField("power")}
                  onChange={(e) => setPower(e.target.value)}
                  disabled={session?.isRunning || session?.isPaused}
                />
              </div>
            </div>

            <NumericKeyboard
              onNumberClick={handleKeyboardNumber}
              onClear={handleKeyboardClear}
            />

            <div className="action-row">
              <button onClick={handleStartManual} disabled={actionLoading}>
                Iniciar
              </button>

              <button onClick={handleQuickStart} disabled={actionLoading}>
                Início rápido
              </button>

              <button onClick={handlePauseOrCancel} disabled={actionLoading}>
                Pausa / Cancelar
              </button>

              <button
                onClick={handleResume}
                disabled={actionLoading || !session?.isPaused}
              >
                Retomar
              </button>
            </div>

            {message && <div className="success-box">{message}</div>}
            {error && <div className="error-box">{error}</div>}

            <div className="process-box">
              <h3>Processamento</h3>
              <div className="process-text">
                {session?.processText || "Nada em execução."}
              </div>
            </div>
          </div>
        </div>

        <div className="right-column">
          <ProgramList
            programs={programs}
            onSelectProgram={handleSelectProgram}
            loading={programsLoading}
          />

          <CustomProgramForm
            onSave={handleSaveCustomProgram}
            loading={actionLoading}
          />
        </div>
      </div>
    </div>
  );
}

export default MicrowavePanel;