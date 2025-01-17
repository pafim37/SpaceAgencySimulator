import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import EditIcon from "@mui/icons-material/Edit";
import { Typography } from "@mui/material";
import AddCircleOutlineIcon from "@mui/icons-material/AddCircleOutline";
import { useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";
import BodyDialogContent from "./BodyDialogContent";
import { useUpdateBodyRequest } from "../axiosBase/axiosBody";
import { useCreateBodyRequest } from "../axiosBase/axiosBody";
import SnackbarAlert from "../alerts/SnackbarAlert";

interface IBodyDialog {
  isModificationDialog: boolean;
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
  const updateBodyRequest = useUpdateBodyRequest();
  const createBodyRequest = useCreateBodyRequest();

  useEffect(() => {
    props.isModificationDialog
      ? setBodyForm({
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
        })
      : setBodyForm({
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
    console.log("handleSubmit", bodyForm.name, bodyForm);
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
      if (props.isModificationDialog) {
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
        console.log("send", bodyToSend);
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
      }
    }
  };

  return (
    <React.Fragment>
      {props.isModificationDialog ? (
        <Button
          startIcon={<EditIcon />}
          variant="contained"
          m={3}
          style={{ backgroundColor: "#1fb89f", color: "black" }}
          onClick={handleOpenDialog}
        >
          <Typography>Edit body</Typography>
        </Button>
      ) : (
        <Button
          startIcon={<AddCircleOutlineIcon />}
          variant="outlined"
          onClick={handleOpenDialog}
          style={{ backgroundColor: "#1fb89f", color: "black" }}
        >
          Add new body
        </Button>
      )}
      <Dialog
        open={openDialog}
        onClose={handleCloseDialog}
        fullScreen={fullScreen}
        maxWidth="md"
      >
        {props.isModificationDialog ? (
          <DialogTitle>{"Edit body"}</DialogTitle>
        ) : (
          <DialogTitle>{"Add new body"}</DialogTitle>
        )}
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
          {props.isModificationDialog ? (
            <Button variant="contained" onClick={handleSubmit}>
              Update
            </Button>
          ) : (
            <Button variant="contained" onClick={handleSubmit}>
              Add
            </Button>
          )}
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
