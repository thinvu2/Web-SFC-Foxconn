import axios from "axios";
import store from '../store/index'

const baseDomain = store.state.apiAddress;


export default {
    getRepo(url, payload) {
      return axios.post(`${baseDomain}/${url}`, payload,{
          headers: {
            Authorization: "Bearer caca",
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*", 
          },
        });
    },
    getApiServer(url, params) {
      return axios.get(`${baseDomain}/${url}`, {
        params: params,
        headers: {
          Authorization: "Bearer caca",
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "*"
        },
      });
    }
}