import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useCreateBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const createBodyRequest = async (body: BodyType) => {
    return await axiosBody
      .post("/", body)
      .then((response) => {
        showSnackbar(`The ${body.name} was created successfully`, "success");
        return response.data as BodyType;
      })
      .catch((error) => {
        const statusCode = error.response?.status;
        if (statusCode === 409) {
          showSnackbar(
            `Error occured: ${error.response.data.message}`,
            "error"
          );
        } else {
          showSnackbar(`Error occured: ${error.message}`, "error");
        }
        return undefined;
      });
  };
  return createBodyRequest;
};
