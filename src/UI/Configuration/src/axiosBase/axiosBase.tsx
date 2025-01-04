import axios from 'axios';

const axiosBase = (endpoint: string) => axios.create({
    baseURL: 'http://localhost:5000/' + endpoint,
    headers: {
        Accept: "application/json",
        "Content-Type": "application/json"
    }
});

export default axiosBase;