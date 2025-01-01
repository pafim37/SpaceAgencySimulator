import React, { Dispatch, SetStateAction } from "react";
import { Snackbar } from "@mui/material";
import Alert from "@mui/material/Alert";

interface ISnackbarAlert {
  openSnackbarAlert: boolean;
  setOpenSnackbarAlert: Dispatch<SetStateAction<boolean>>;
  message: string;
}

export default function SnackbarAlert(props: ISnackbarAlert) {
  const autoHiddenDuration = 6000;
  const handleCloseSnack = () => {
    props.setOpenSnackbarAlert(false);
  };
  return (
    <Snackbar
      open={props.openSnackbarAlert}
      autoHideDuration={autoHiddenDuration}
      onClose={handleCloseSnack}
      anchorOrigin={{ vertical: "top", horizontal: "center" }}
    >
      <Alert onClose={handleCloseSnack} severity="error" sx={{ width: "100%" }}>
        {props.message}
      </Alert>
    </Snackbar>
  );
}
