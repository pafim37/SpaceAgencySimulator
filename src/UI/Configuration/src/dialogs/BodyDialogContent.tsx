import React, { Dispatch, SetStateAction } from "react";
import Grid from "@mui/material/Grid2";
import TextField from "@mui/material/TextField";
import set from "lodash/set";

interface IBodyDialogContent {
  body: BodyType;
  setBody: Dispatch<SetStateAction<BodyType>>;
  isNameDisabled: boolean;
}

export default function BodyDialogContent(props: IBodyDialogContent) {
  const handleName = (event) => {
    const { name, value } = event.target;
    props.setBody((prevValues) => {
      const updatedValues = { ...prevValues };
      set(updatedValues, name, value);
      return updatedValues;
    });
  };

  const handleNumber = (event) => {
    const regex = /^-?(\d*)?[.,]?(\d*)?$/;
    validateAndUpdate(event, regex);
  };

  const handlePositiveNumber = (event) => {
    const regex = /^(\d*)?[.,]?(\d*)?$/;
    validateAndUpdate(event, regex);
  };

  const validateAndUpdate = (event, regex: RegExp) => {
    const { name, value } = event.target;
    if (regex.test(value)) {
      if (value === "" || value === "-") {
        updateBody(name, value);
      } else {
        const normalizedValue = value.replace(",", ".");
        const floatValue = parseFloat(normalizedValue);
        if (!isNaN(floatValue)) {
          updateBody(name, floatValue);
        }
      }
    }
  };

  const updateBody = (name: string, value: string | number) => {
    props.setBody((prevValues) => {
      const updatedValues = { ...prevValues };
      set(updatedValues, name, value);
      return updatedValues;
    });
  };

  return (
    <>
      <Grid container spacing={2}>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="name"
            label="Name"
            disabled={props.isNameDisabled}
            value={props.body.name}
            error={props.body.name === ""}
            helperText={props.body.name === "" ? "Name cannot be empty." : ""}
            onChange={handleName}
            autoFocus
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="mass"
            label="Mass"
            value={props.body.mass}
            error={props.body.mass <= 0}
            helperText={
              props.body.mass <= 0 ? "Mass must be positive number." : ""
            }
            onChange={handlePositiveNumber}
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="radius"
            label="Radius"
            value={props.body.radius}
            error={props.body.radius <= 0}
            helperText={
              props.body.radius <= 0 ? "Radius must be positive number." : ""
            }
            onChange={handlePositiveNumber}
          />
        </Grid>
      </Grid>
      <Grid container spacing={2}>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="position.x"
            label="X Position"
            value={props.body.position.x}
            onChange={handleNumber}
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="position.y"
            label="Y Position"
            value={props.body.position.y}
            onChange={handleNumber}
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="position.z"
            label="Z Position"
            value={props.body.position.z}
            onChange={handleNumber}
          />
        </Grid>
      </Grid>
      <Grid container spacing={2}>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="velocity.x"
            label="X Velocity"
            value={props.body.velocity.x}
            onChange={handleNumber}
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="velocity.y"
            label="Y Velocity"
            value={props.body.velocity.y}
            onChange={handleNumber}
          />
        </Grid>
        <Grid size={4}>
          <TextField
            margin="dense"
            variant="standard"
            name="velocity.z"
            label="Z Velocity"
            value={props.body.velocity.z}
            onChange={handleNumber}
          />
        </Grid>
      </Grid>
    </>
  );
}
