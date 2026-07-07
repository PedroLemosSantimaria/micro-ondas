function ProgramList({ programs, onSelectProgram, loading }) {
  const safePrograms = Array.isArray(programs) ? programs : [];

  return (
    <div className="program-list-card">
      <h3>Programas</h3>

      {loading && <p>Carregando programas...</p>}

      {!loading && safePrograms.length === 0 && (
        <p>Nenhum programa encontrado.</p>
      )}

      <div className="program-list">
        {safePrograms.map((program) => (
          <button
            key={`${program.name}-${program.heatingChar}`}
            className="program-item"
            onClick={() => onSelectProgram(program)}
            type="button"
          >
            <div className="program-item-header">
              <strong
                style={{ fontStyle: program.isDefault ? "normal" : "italic" }}
              >
                {program.name}
              </strong>
            </div>

            <div className="program-item-body">
              <div>Alimento: {program.food}</div>
              <div>Tempo: {program.timeInSeconds}s</div>
              <div>Potência: {program.power}</div>
              <div>Caractere: {program.heatingChar}</div>
            </div>
          </button>
        ))}
      </div>
    </div>
  );
}

export default ProgramList;