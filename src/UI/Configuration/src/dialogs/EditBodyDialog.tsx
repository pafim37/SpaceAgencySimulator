import React, { useState, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { Typography, useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import { axiosUpdateBody } from "../axiosBase/axiosBody";
import BodyDialogContent from "./BodyDialogContent";
import EditIcon from "@mui/icons-material/Edit";
import SnackbarAlert from "../alerts/SnackbarAlert";

interface IEditBodyDialog {
  body: BodyType;
  setBody: Dispatch<SetStateAction<BodyType[]>>;
}

export default function EditBodyDialog(props: IEditBodyDialog) {
  const [open, setOpen] = useState<boolean>(false);
  const [openSnackbar, setOpenSnackbar] = useState<boolean>(false);
  const [editableBody, setEditableBody] = useState<BodyType>(props.body);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  const handleOpen = async () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const update = () => {
    const message = validate(editableBody);
    if (message === "") {
      axiosUpdateBody(editableBody);
      props.setBody((prev: BodyType[]) =>
        prev.map((body) =>
          body.name === editableBody.name ? (editableBody as BodyType) : body
        )
      );
      handleClose();
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
        startIcon={<EditIcon />}
        variant="contained"
        m={3}
        style={{ backgroundColor: "#1fb89f", color: "black" }}
        onClick={handleOpen}
      >
        <Typography>Edit body</Typography>
      </Button>

      <Dialog
        open={open}
        onClose={handleClose}
        fullScreen={fullScreen}
        maxWidth="md"
      >
        <DialogTitle>{"Edit body"}</DialogTitle>
        <DialogContent>
          <BodyDialogContent
            body={editableBody}
            setBody={setEditableBody}
            isNameDisabled={true}
          />
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleClose}>
            Cancel
          </Button>
          <Button variant="contained" onClick={update}>
            Update
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
