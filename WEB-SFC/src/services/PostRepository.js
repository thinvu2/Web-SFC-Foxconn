import Repository from "./Repository";

export default{
    send(url,payload){
        return Repository.post(url,payload);
    }
};