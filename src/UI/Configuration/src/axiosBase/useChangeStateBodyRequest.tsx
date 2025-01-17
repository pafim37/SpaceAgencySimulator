import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useChangeStateBodyRequest = () => {
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const { showSnackbar } = useSnackbar();

  const changeStateBodyRequest = async (
    bodyname: string,
    newState: boolean
  ) => {
    return await axiosBody
      .post(bodyname, newState)
      .then((response) => {
        showSnackbar(`The ${bodyname} was updated sucessfully`, "success");
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return undefined;
      });
  };
  return changeStateBodyRequest;
};
