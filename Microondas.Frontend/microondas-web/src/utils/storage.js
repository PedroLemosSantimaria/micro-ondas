const tokenKey = "microondas_token";

export function saveToken(token) {
  localStorage.setItem(tokenKey, token);
}

export function getToken() {
  return localStorage.getItem(tokenKey);
}

export function removeToken() {
  localStorage.removeItem(tokenKey);
}