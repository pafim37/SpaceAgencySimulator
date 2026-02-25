import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import {Button, IconButton, Checkbox, FormControlLabel} from "@mui/material";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import EditIcon from "@mui/icons-material/Edit";
import { useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import BodyDialogContent from "./BodyDialogContent";
import { useUpdateBodyRequest } from "../axiosBase/useUpdateBodyRequest";
import SnackbarAlert from "../alerts/SnackbarAlert";

interface IBodyDialog {
  body: BodyType;
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
  const [autoUpdate, setAutoUpdate] = useState<boolean>(false);
  const updateBodyRequest = useUpdateBodyRequest();

  useEffect(() => {
    if (openDialog) {
      setBodyForm({
        name: props.body.name,
        mass: props.body.mass.toString(),
        radius: props.body.radius.toString(),
        position: {
          x: props.body.position.x.toString(),
          y: props.body.position.y.toString(),
          z: props.body.position.z.toString(),
        },
        velocity: {
          x: props.body.velocity.x.toString(),
          y: props.body.velocity.y.toString(),
          z: props.body.velocity.z.toString(),
        },
      });
      setIsLoading(false);
    }
  }, [openDialog, props.body.id]);

  useEffect(() => {
    if (!bodyForm || !isValidBodyForm || !autoUpdate) return;

    const timer = setTimeout(async () => {
      const bodyToSend: BodyType = {
        id: props.body.id,
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
      await updateBodyRequest(bodyToSend);
    }, 200);

    return () => clearTimeout(timer);
  }, [bodyForm, isValidBodyForm, autoUpdate]);

  const handleOpenDialog = async () => {
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setAutoUpdate(false);
    setOpenDialog(false);
  };

  const handleSubmit = async () => {
    if (isValidBodyForm) {
      const bodyToSend: BodyType = {
        id: props.body.id,
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
      const updatedBody: BodyType = await updateBodyRequest(bodyToSend);
      if (updatedBody !== undefined) {
        props.setBodies((prev: BodyType[]) =>
          prev.map((body) =>
            body.id === props.body.id ? (bodyToSend as BodyType) : body
          )
        );
        handleCloseDialog();
      }
    } else {
      setOpenSnackbar(true);
    }
  };

  return (
    <React.Fragment>
      <IconButton
        onClick={handleOpenDialog}
        style={{ color: "#1fb89f" }}
      >
        <EditIcon />
      </IconButton>

      <Dialog
        open={openDialog}
        onClose={handleCloseDialog}
        fullScreen={fullScreen}
        maxWidth="md"
      >
        <DialogTitle>{"Edit body"}</DialogTitle>
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
        <DialogActions >
          <FormControlLabel
            control={
              <Checkbox
                checked={autoUpdate}
                onChange={(e) => setAutoUpdate(e.target.checked)}
              />
            }
            label="Auto-update"
          />
            <Button variant="outlined" onClick={handleCloseDialog}>
              Cancel
            </Button>
            <Button
              variant="contained"
              onClick={handleSubmit}
              disabled={autoUpdate}
              style={{ marginLeft: "8px" }}
            >
              Update
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
