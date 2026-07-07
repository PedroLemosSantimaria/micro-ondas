import { useEffect, useState } from "react";
import { getToken, removeToken, saveToken } from "../utils/storage";

export function useAuth() {
  const [token, setToken] = useState(getToken());

  useEffect(() => {
    setToken(getToken());
  }, []);

  function login(newToken) {
    saveToken(newToken);
    setToken(newToken);
  }

  function logout() {
    removeToken();
    setToken(null);
  }

  return {
    token,
    isAuthenticated: !!token,
    login,
    logout,
  };
}