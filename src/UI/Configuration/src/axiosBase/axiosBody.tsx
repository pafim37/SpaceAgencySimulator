import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";

const axiosBody = axiosBase("body/");

export const useGetBodiesRequest = () => {
  const { showSnackbar } = useSnackbar();

  const getBodiesRequest = async () => {
    let data: Array<BodyType> = undefined;
    await axiosBody
      .get("/")
      .then((response) => {
        data = response.data;
      })
      .catch((error) => {
        showSnackbar(
          `Error occured while fetch bodies: ${error.message}`,
          "error"
        );
      });
    return data;
  };

  return getBodiesRequest;
};

export const useGetBodyRequest = () => {
  const { showSnackbar } = useSnackbar();

  const getBodyRequest = async (name: string) => {
    let data: BodyType = undefined;
    await axiosBody
      .get(name)
      .then((response) => {
        data = response.data;
      })
      .catch((error) => {
        showSnackbar(
          `Error occured while fetch ${name}: ${error.message}`,
          "error"
        );
      });
    return data;
  };

  return getBodyRequest;
};

export const useCreateBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const ceateBodyRequest = async (body: BodyType) => {
    try {
      await axiosBody.post("/", body);
      showSnackbar(`The ${body.name} was created successfully`, "success");
      return true;
    } catch (error) {
      const statusCode = error.response?.status;
      if (statusCode === 409) {
        showSnackbar(`Error occured: ${error.response.data.message}`, "error");
      } else {
        showSnackbar(`Error occured: ${error.message}`, "error");
      }
      return false;
    }
  };
  return ceateBodyRequest;
};

export const useUpdateBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const updateBodyRequest = async (body: BodyType) => {
    try {
      axiosBody.patch("/", body);
      showSnackbar(`The ${body.name} was updated successfully`, "success");
      return true;
    } catch (error) {
      showSnackbar(`Error occured: ${error.message}`, "error");
      return false;
    }
  };
  return updateBodyRequest;
};

export const useDeleteBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const deleteBodyRequest = async (bodyname: string) => {
    try {
      axiosBody.delete(bodyname);
      showSnackbar(`The ${bodyname} was removed sucessfully`, "success");
      return true;
    } catch (error) {
      showSnackbar(`Error occured: ${error.message}`, "error");
      return false;
    }
  };
  return deleteBodyRequest;
};

export const useChangeStateBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const changeStateBodyRequest = async (
    bodyname: string,
    newState: boolean
  ) => {
    try {
      axiosBody.post(bodyname, newState);
      return true;
    } catch (error) {
      showSnackbar(`Error occured: ${error.message}`, "error");
      return false;
    }
  };
  return changeStateBodyRequest;
};

export default axiosBody;
