import axiosBase from "./axiosBase";
import { useSnackbar } from "../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useCloneBodyRequest = () => {
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/clone", connectionId);
  const { showSnackbar } = useSnackbar();

  const cloneBodyRequest = async (
    bodyname: string,
  ) => {
    console.log(`Cloning body with name: ${bodyname}`);
    return await axiosBody
      .post(bodyname)
      .then((response) => {
        showSnackbar(`The ${bodyname} was cloned successfully`, "success");
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return undefined;
      });
  };
  return cloneBodyRequest;
};
