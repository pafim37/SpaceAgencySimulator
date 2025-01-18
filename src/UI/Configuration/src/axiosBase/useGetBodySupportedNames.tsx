import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useGetBodySupportedNames = () => {
  const { showSnackbar } = useSnackbar();
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const getBodySupportedNames = async () => {
    return await axiosBody
      .get("supported-names")
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return undefined;
      });
  };
  return getBodySupportedNames;
};
