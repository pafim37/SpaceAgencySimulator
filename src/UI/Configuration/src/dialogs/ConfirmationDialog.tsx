import React, { Dispatch, SetStateAction } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { DialogContentText, useMediaQuery } from "@mui/material";
import { useTheme } from "@mui/material/styles";

interface IConfirmationDialog {
  open: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
  name: string;
  call: () => void;
}

export default function ConfirmationDialog(props: IConfirmationDialog) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));

  const handleCloseDialog = () => {
    props.setOpen(false);
  };

  const handleSubmit = () => {
    props.call();
  };

  return (
    <React.Fragment>
      <Dialog
        open={props.open}
        onClose={handleCloseDialog}
        fullScreen={fullScreen}
        maxWidth="md"
      >
        <DialogTitle>{"Are you sure?"}</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Do you really want to remove body {props.name}?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleCloseDialog}>
            No
          </Button>
          <Button variant="contained" onClick={handleSubmit}>
            Yes
          </Button>
        </DialogActions>
      </Dialog>
    </React.Fragment>
  );
}
