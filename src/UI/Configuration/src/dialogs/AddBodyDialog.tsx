import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import BodyDialogContent from "./BodyDialogContent";
import { useCreateBodyRequest } from "../axiosBase/axiosBody";
import SnackbarAlert from "../alerts/SnackbarAlert";

interface IBodyDialog {
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

export default function BodyDialog(props: IBodyDialog) {
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [bodyForm, setBodyForm] = useState<BodyStringType>();
  const [isValidBodyForm, setIsValidBodyForm] = useState<boolean>(false);
  const [openSnackbar, setOpenSnackbar] = useState<boolean>(false);
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const createBodyRequest = useCreateBodyRequest();

  useEffect(() => {
    setBodyForm({
      name: "",
      mass: "1",
      radius: "1",
      position: {
        x: "50",
        y: "0",
        z: "0",
      },
      velocity: {
        x: "0",
        y: "1",
        z: "0",
      },
    } as BodyStringType);
    setIsLoading(false);
  }, []);

  const handleOpenDialog = async () => {
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleSubmit = async () => {
    if (isValidBodyForm) {
      const bodyToSend: NewBodyType = {
        enabled: true,
        name: bodyForm.name,
        mass: parseFloat(bodyForm.mass),
        radius: parseFloat(bodyForm.radius),
        position: {
          x: parseFloat(bodyForm.position.x),
          y: parseFloat(bodyForm.position.y),
          z: parseFloat(bodyForm.position.z),
        },
        velocity: {
          x: parseFloat(bodyForm.velocity.x),
          y: parseFloat(bodyForm.velocity.y),
          z: parseFloat(bodyForm.velocity.z),
        },
      };
      const createdBody: BodyType = await createBodyRequest(bodyToSend);
      if (createdBody !== undefined) {
        props.setBodies((prev: BodyType[]) => [
          ...prev,
          bodyToSend as BodyType,
        ]);
        handleCloseDialog();
      } else {
        setOpenSnackbar(true);
      }
    } else {
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
              body={bodyForm}
              setBody={setBodyForm}
              setIsValidBodyForm={setIsValidBodyForm}
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
        message={"Could not send a request"}
      />
    </React.Fragment>
  );
}
