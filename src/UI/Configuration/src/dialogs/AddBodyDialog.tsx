import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import { useCreateBodyRequest } from "../axiosBase/useCreateBodyRequest";
import BodyDialogContent from "./BodyDialogContent";
import SnackbarAlert from "../alerts/SnackbarAlert";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import useBodyValidate from "../helpers/BodyValidate";

interface IAddBodyDialog {
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

export default function AddBodyDialog(props: IAddBodyDialog) {
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [openSnackbar, setOpenSnackbar] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [newBody, setNewBody] = useState<BodyType>();
  const createBodyRequest = useCreateBodyRequest();
  const { validateBody, validateErrors } = useBodyValidate();
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  useEffect(() => {
    validateBody(newBody);
    // eslint-disable-next-line
  }, [newBody]);

  const handleOpenDialog = async () => {
    const body: BodyType = {
      name: "",
      mass: 1,
      radius: 1,
      enabled: true,
      position: {
        x: 50,
        y: 0,
        z: 0,
      },
      velocity: {
        x: 0,
        y: 1,
        z: 0,
      },
    };
    setNewBody(body);
    setOpenDialog(true);
    setIsLoading(false);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleSubmit = async () => {
    if (validateErrors.length === 0) {
      const createdBody: BodyType = await createBodyRequest(newBody);
      if (createdBody !== undefined) {
        props.setBodies((prev: BodyType[]) => [...prev, newBody as BodyType]);
        handleCloseDialog();
      }
    } else {
      setErrorMessage(validateErrors.join(" "));
      setOpenSnackbar(true);
    }
  };

  return (
    <React.Fragment>
      <Button
        startIcon={<AddCircleOutlineIcon />}
        variant="outlined"
        onClick={handleOpenDialog}
        style={{ backgroundColor: "#1fb89f", color: "black" }}
      >
        Add new body
      </Button>
      <Dialog
        open={openDialog}
        onClose={handleCloseDialog}
        fullScreen={fullScreen}
        maxWidth="md"
      >
        <DialogTitle>{"Add new body"}</DialogTitle>
        <DialogContent>
          {!isLoading ? (
            <BodyDialogContent
              body={newBody}
              setBody={setNewBody}
              isNameDisabled={false}
            />
          ) : (
            <></>
          )}
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleCloseDialog}>
            Cancel
          </Button>
          <Button variant="contained" onClick={handleSubmit}>
            Add
          </Button>
        </DialogActions>
      </Dialog>
      <SnackbarAlert
        openSnackbarAlert={openSnackbar}
        setOpenSnackbarAlert={setOpenSnackbar}
        message={errorMessage}
      />
    </React.Fragment>
  );
}
