import axiosClient from "./axiosClient";

export async function getPrograms() {
  const response = await axiosClient.get("/api/programs");
  return response.data.data ?? [];
}

export async function createCustomProgram(programData) {
  const response = await axiosClient.post("/api/programs", programData);
  return response.data;
}