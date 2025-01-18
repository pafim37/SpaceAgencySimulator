import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useGetBodiesRequest = () => {
  const { showSnackbar } = useSnackbar();
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const getBodiesRequest = async () => {
    return await axiosBody
      .get("/")
      .then((response) => {
        return response.data;
      })
      .catch((error) => {
        showSnackbar(
          `Error occured while fetch bodies: ${error.message}`,
          "error"
        );
        return undefined;
      });
  };
  return getBodiesRequest;
};
