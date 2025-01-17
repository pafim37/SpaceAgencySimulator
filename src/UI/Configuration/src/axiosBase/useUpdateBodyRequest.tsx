import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useUpdateBodyRequest = () => {
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);
  const { showSnackbar } = useSnackbar();
  const updateBodyRequest = async (body: BodyType) => {
    return await axiosBody
      .patch("/", body)
      .then((response) => {
        showSnackbar(`The ${body.name} was updated successfully`, "success");
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return undefined;
      });
  };
  return updateBodyRequest;
};
