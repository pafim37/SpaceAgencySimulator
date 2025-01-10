import axiosBase from "./axiosBase";
import { useSnackbar } from "./../providers/SnackbarContext";

const axiosBody = axiosBase("body/");

export const useGetBodiesRequest = () => {
  const { showSnackbar } = useSnackbar();

  const getBodiesRequest = async () => {
    let data: Array<BodyType> = undefined;
    return await axiosBody
      .get("/")
      .then((response) => {
        data = response.data;
        return data;
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

export const useGetBodyRequest = () => {
  const { showSnackbar } = useSnackbar();

  const getBodyRequest = async (name: string) => {
    let data: BodyType = undefined;
    return await axiosBody
      .get(name)
      .then((response) => {
        data = response.data;
        return data;
      })
      .catch((error) => {
        showSnackbar(
          `Error occured while fetch ${name}: ${error.message}`,
          "error"
        );
        return data;
      });
  };

  return getBodyRequest;
};

export const useCreateBodyRequest = () => {
  let data: BodyType = undefined;
  const { showSnackbar } = useSnackbar();
  const ceateBodyRequest = async (body: BodyType) => {
    return await axiosBody
      .post("/", body)
      .then((response) => {
        data = response.data;
        showSnackbar(`The ${body.name} was created successfully`, "success");
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
        return data;
      });
  };
  return ceateBodyRequest;
};

export const useUpdateBodyRequest = () => {
  let data: BodyType = undefined;
  const { showSnackbar } = useSnackbar();
  const updateBodyRequest = async (body: BodyType) => {
    return await axiosBody
      .patch("/", body)
      .then((response) => {
        data = response.data;
        showSnackbar(`The ${body.name} was updated successfully`, "success");
        return data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return data;
      });
  };
  return updateBodyRequest;
};

export const useDeleteBodyRequest = () => {
  let data: BodyType = undefined;
  const { showSnackbar } = useSnackbar();
  const deleteBodyRequest = async (bodyname: string) => {
    return await axiosBody
      .delete(bodyname)
      .then((response) => {
        showSnackbar(`The ${bodyname} was removed sucessfully`, "success");
        return response.data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return data;
      });
  };
  return deleteBodyRequest;
};

export const useChangeStateBodyRequest = () => {
  let data: BodyType = undefined;
  const { showSnackbar } = useSnackbar();
  const changeStateBodyRequest = async (
    bodyname: string,
    newState: boolean
  ) => {
    return await axiosBody
      .post(bodyname, newState)
      .then((response) => {
        data = response.data;
        showSnackbar(`The ${bodyname} was updated sucessfully`, "success");
        return data;
      })
      .catch((error) => {
        showSnackbar(`Error occured: ${error.message}`, "error");
        return data;
      });
  };
  return changeStateBodyRequest;
};

export const useCreateBodyDefaultsRequest = () => {
  const { showSnackbar } = useSnackbar();
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

export const useGetBodySupportedNames = () => {
  const { showSnackbar } = useSnackbar();
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

export default axiosBody;
