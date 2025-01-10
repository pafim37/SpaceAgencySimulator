import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { Typography, useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import BodyDialogContent from "./BodyDialogContent";
import EditIcon from "@mui/icons-material/Edit";
import SnackbarAlert from "../alerts/SnackbarAlert";
import { useUpdateBodyRequest } from "../axiosBase/axiosBody";
import useBodyValidate from "../helpers/BodyValidate";

interface IEditBodyDialog {
  body: BodyType;
  setBody: Dispatch<SetStateAction<BodyType[]>>;
}

export default function EditBodyDialog(props: IEditBodyDialog) {
  const [open, setOpen] = useState<boolean>(false);
  const [openSnackbar, setOpenSnackbar] = useState<boolean>(false);
  const [editableBody, setEditableBody] = useState<BodyType>(props.body);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const { validateBody, validateErrors } = useBodyValidate();
  const updateBodyRequest = useUpdateBodyRequest();

  useEffect(() => {
    validateBody(editableBody);
    // eslint-disable-next-line
  }, [editableBody]);

  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  const handleOpen = async () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const update = async () => {
    if (validateErrors.length === 0) {
      const updatedBody: BodyType = await updateBodyRequest(editableBody);
      if (updatedBody !== undefined) {
        props.setBody((prev: BodyType[]) =>
          prev.map((body) =>
            body.name === editableBody.name ? (editableBody as BodyType) : body
          )
        );
        handleClose();
      }
    } else {
      setErrorMessage(validateErrors.join(" "));
      setOpenSnackbar(true);
    }
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
