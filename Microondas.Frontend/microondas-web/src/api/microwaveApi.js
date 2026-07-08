import axiosClient from "./axiosClient";

export async function startManualHeating(data) {
  const response = await axiosClient.post("/api/microwave/start", data);
  return response.data.data;
}

export async function quickStartHeating() {
  const response = await axiosClient.post("/api/microwave/quick-start");
  return response.data.data;
}

export async function startProgramHeating(programName) {
  const response = await axiosClient.post("/api/microwave/start-program/" + programName, {
    programName,
  });

  return response.data.data;
}

export async function pauseOrCancelHeating() {
  const response = await axiosClient.post("/api/microwave/pause-cancel");
  return response.data.data;
}

export async function resumeHeating() {
  const response = await axiosClient.post("/api/microwave/resume");
  return response.data.data;
}

export async function tickHeating() {
  const response = await axiosClient.post("/api/microwave/tick");
  return response.data.data;
}

export async function getCurrentSession() {
  const response = await axiosClient.get("/api/microwave/current");
  return response.data.data;
}