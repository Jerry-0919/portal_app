import axios from "axios";
// import router from '@/router'

import cookies from "@/cookies.js";

const apiClient = axios.create({
  baseURL: "/api/",
  withCredentials: true,
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
  },
});
apiClient.defaults.headers.common["Authorization"] =
  "Bearer " + cookies.getCookie("bearer");
export default apiClient;
