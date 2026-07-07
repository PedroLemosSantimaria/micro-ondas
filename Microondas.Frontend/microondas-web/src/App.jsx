import { useState } from "react";
import LoginForm from "./components/LoginForm";
import MicrowavePanel from "./components/MicrowavePanel";
import { loginRequest } from "./api/authApi";
import { useAuth } from "./hooks/useAuth";
import "./styles/app.css";
import "./styles/form.css";
import "./styles/microwave.css";
import "./styles/program.css";

function App() {
  const { isAuthenticated, login, logout } = useAuth();
  const [loginLoading, setLoginLoading] = useState(false);
  const [loginError, setLoginError] = useState("");

  async function handleLogin(data) {
    try {
      setLoginError("");
      setLoginLoading(true);

      const result = await loginRequest(data);

      const token =
        result?.token ||
        result?.accessToken ||
        result?.jwtToken;

      if (!token) {
        setLoginError("A API não retornou token.");
        return;
      }

      login(token);
    } catch (err) {
      setLoginError(
        err?.response?.data?.message ||
          err?.response?.data?.title ||
          "Falha ao autenticar."
      );
    } finally {
      setLoginLoading(false);
    }
  }

  if (!isAuthenticated) {
    return (
      <div className="login-page">
        <LoginForm
          onLogin={handleLogin}
          loading={loginLoading}
          error={loginError}
        />
      </div>
    );
  }

  return <MicrowavePanel onLogout={logout} />;
}

export default App;