import React, { useState, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import { axiosCreateBody } from "../axiosBase/axiosBody";
import BodyDialogContent from "./BodyDialogContent";
import SnackbarAlert from "../alerts/SnackbarAlert";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";

interface IAddBodyDialog {
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

export default function AddBodyDialog(props: IAddBodyDialog) {
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [openSnackbar, setOpenSnackbar] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [newBody, setNewBody] = useState<BodyType>();

  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  const handleOpenDialog = async () => {
    const body: BodyType = {
      name: "",
      mass: 1,
      radius: 1,
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

  const handleSubmit = () => {
    const message = validate(newBody);
    if (message === "") {
      axiosCreateBody(newBody);
      props.setBodies((prev: BodyType[]) => [...prev, newBody as BodyType]);
      handleCloseDialog();
    } else {
      setErrorMessage(message);
      setOpenSnackbar(true);
    }
  };

  const validate = (newBody: BodyType) => {
    let messages = "";
    if (!newBody.name.trim()) {
      messages += "Name is required. ";
    }
    if (newBody.mass < 0) {
      messages += "Mass must be higher than 0. ";
    }
    if (newBody.radius < 0) {
      messages += "Radius must be higher than 0. ";
    }
    return messages;
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
