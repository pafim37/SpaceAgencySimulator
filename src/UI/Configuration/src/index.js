import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { SnackbarProvider } from "./providers/SnackbarContext";
import { ThemeProvider, createTheme } from '@mui/material/styles';

const root = ReactDOM.createRoot(document.getElementById('root'));

const darkTheme = createTheme({
  palette: {
    mode: 'dark',
  },
});

root.render (
    <ThemeProvider theme={darkTheme}>
      <SnackbarProvider>
        <App />
      </SnackbarProvider>
    </ThemeProvider>
  );