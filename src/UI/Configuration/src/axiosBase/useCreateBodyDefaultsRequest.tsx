import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";
import { useConnectionId } from "../providers/ConnectionIdContext";

export const useCreateBodyDefaultsRequest = () => {
  const { showSnackbar } = useSnackbar();
  const { connectionId } = useConnectionId();
  const axiosBody = axiosBase("body/", connectionId);

  const ceateBodyDefaultsRequest = async (names: string[]) => {
    return await axiosBody
      .post("/defaults", names)
      .then((response) => {
        const data: string[] = response.data.map((b: BodyType) => b.name);
        showSnackbar(
          `The ${names.map((name) => name)} was created successfully`,
          "success"
        );
        return data;
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
  return ceateBodyDefaultsRequest;
};
