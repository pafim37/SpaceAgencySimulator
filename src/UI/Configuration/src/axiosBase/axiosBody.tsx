import axiosBase from "./axiosBase";

const axiosBody = axiosBase("body/");

export const axiosGetBodies = async () => {
  let data: Array<BodyType>;
  await axiosBody
    .get("/")
    .then((response) => {
      data = response.data;
    })
    .catch((error) => console.log(error));
  return data;
};

export const axiosGetBody = async (name: string) => {
  let data: BodyType;
  await axiosBody
    .get(name)
    .then((response) => {
      data = response.data;
    })
    .catch((error) => console.log(error));
  return data;
};

export const axiosCreateBody = async (body: BodyType) => {
  await axiosBody.post("/", body).catch((error) => console.log(error));
};

export const axiosUpdateBody = async (body: BodyType) => {
  axiosBody.patch("/", body).catch((error) => console.log(error));
};

export const axiosDeleteBody = async (bodyname: string) => {
  axiosBody.delete(bodyname).catch((error) => console.log(error));
};

export default axiosBody;
