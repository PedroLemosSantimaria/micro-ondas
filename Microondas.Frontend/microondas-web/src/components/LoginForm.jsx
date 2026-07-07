import { useState } from "react";

function LoginForm({ onLogin, loading, error }) {
  const [userName, setUserName] = useState("admin");
  const [password, setPassword] = useState("123456");

  function handleSubmit(e) {
    e.preventDefault();
    onLogin({ userName, password });
  }

  return (
    <div className="login-card">
      <h2>Entrar</h2>
      <p className="login-subtitle">Autentique para usar o micro-ondas</p>

      <form onSubmit={handleSubmit} className="default-form">
        <div className="form-group">
          <label>Usuário</label>
          <input
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            placeholder="Digite o usuário"
          />
        </div>

        <div className="form-group">
          <label>Senha</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Digite a senha"
          />
        </div>

        {error && <div className="error-box">{error}</div>}

        <button type="submit" disabled={loading}>
          {loading ? "Entrando..." : "Entrar"}
        </button>
      </form>
    </div>
  );
}

export default LoginForm;