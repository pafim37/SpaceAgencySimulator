import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useDeleteBodyRequest = () => {
  const { showSnackbar } = useSnackbar();
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const deleteBodyRequest = async (bodyname: string) => {
    return await axiosBody
      .delete(bodyname)
      .then((response) => {
        showSnackbar(`The ${bodyname} was removed sucessfully`, "success");
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return undefined;
      });
  };
  return deleteBodyRequest;
};
