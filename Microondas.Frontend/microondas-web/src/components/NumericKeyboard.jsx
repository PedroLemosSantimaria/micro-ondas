function NumericKeyboard({ onNumberClick, onClear }) {
  const numbers = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];

  return (
    <div className="keyboard-box">
      <div className="keyboard-grid">
        {numbers.map((number) => (
          <button
            key={number}
            type="button"
            className="key-btn"
            onClick={() => onNumberClick(number)}
          >
            {number}
          </button>
        ))}
      </div>

      <button type="button" className="clear-btn" onClick={onClear}>
        Limpar
      </button>
    </div>
  );
}

export default NumericKeyboard;