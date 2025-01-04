import React, { createContext, useContext, useState } from "react";
import { Snackbar } from "@mui/material";
import Alert from "@mui/material/Alert";

const SnackbarContext = createContext<any>(null);

export const SnackbarProvider: React.FC = ({ children }) => {
  const autoHiddenDuration = 6000;
  const [snackbar, setSnackbar] = useState({
    message: "",
    type: "info",
    open: false,
  });

  const showSnackbar = (
    message: string,
    type: "success" | "error" | "info" | "warning"
  ) => {
    setSnackbar({ message, type, open: true });
  };

  const handleClose = () => {
    setSnackbar((prev) => ({ ...prev, open: false }));
  };

  return (
    <SnackbarContext.Provider value={{ showSnackbar }}>
      {children}
      <Snackbar
        open={snackbar.open}
        autoHideDuration={autoHiddenDuration}
        onClose={handleClose}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleClose}
          severity={snackbar.type}
          sx={{ width: "100%" }}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </SnackbarContext.Provider>
  );
};

export const useSnackbar = () => useContext(SnackbarContext);
