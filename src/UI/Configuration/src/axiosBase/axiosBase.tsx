import axios from "axios";

const useAxiosBase = (endpoint: string, connectionId?: string) =>
  axios.create({
    baseURL: "http://localhost:5000/" + endpoint,
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      "X-SAS-SignalRClientId": connectionId,
    },
  });

export default useAxiosBase;
