import { useState } from "react";

const initialForm = {
  name: "",
  food: "",
  timeInSeconds: "",
  power: "",
  heatingChar: "",
  instructions: "",
};

function CustomProgramForm({ onSave, loading }) {
  const [form, setForm] = useState(initialForm);

  function updateField(field, value) {
    setForm((old) => ({
      ...old,
      [field]: value,
    }));
  }

  async function handleSubmit(e) {
    e.preventDefault();

    const payload = {
      name: form.name,
      food: form.food,
      timeInSeconds: Number(form.timeInSeconds),
      power: Number(form.power),
      heatingChar: form.heatingChar,
      instructions: form.instructions,
    };

    const ok = await onSave(payload);

    if (ok) {
      setForm(initialForm);
    }
  }

  return (
    <div className="custom-program-card">
      <h3>Cadastrar programa customizado</h3>

      <form onSubmit={handleSubmit} className="default-form">
        <div className="form-group">
          <label>Nome do programa</label>
          <input
            value={form.name}
            onChange={(e) => updateField("name", e.target.value)}
          />
        </div>

        <div className="form-group">
          <label>Alimento</label>
          <input
            value={form.food}
            onChange={(e) => updateField("food", e.target.value)}
          />
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Tempo (segundos)</label>
            <input
              type="number"
              value={form.timeInSeconds}
              onChange={(e) => updateField("timeInSeconds", e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>Potência</label>
            <input
              type="number"
              value={form.power}
              onChange={(e) => updateField("power", e.target.value)}
            />
          </div>

          <div className="form-group">
            <label>Caractere</label>
            <input
              value={form.heatingChar}
              onChange={(e) => updateField("heatingChar", e.target.value)}
              maxLength={1}
            />
          </div>
        </div>

        <div className="form-group">
          <label>Instruções</label>
          <textarea
            rows="4"
            value={form.instructions}
            onChange={(e) => updateField("instructions", e.target.value)}
          />
        </div>

        <button type="submit" disabled={loading}>
          {loading ? "Salvando..." : "Salvar programa"}
        </button>
      </form>
    </div>
  );
}

export default CustomProgramForm;