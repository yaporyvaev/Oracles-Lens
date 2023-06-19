import axiosOrig from "axios";
import { BASE_URL } from "../constants";

const axios = axiosOrig.create({
  withCredentials: true,
  maxRedirects: 0,
});

axios.defaults.baseURL = BASE_URL;

export { axios };
