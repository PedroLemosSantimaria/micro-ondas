import axiosClient from "./axiosClient";

export async function loginRequest(data) {
  const response = await axiosClient.post("/api/auth/login", data);
  return response.data;
}