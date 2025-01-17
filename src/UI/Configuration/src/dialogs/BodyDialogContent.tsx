import React, { useState, useEffect, Dispatch, SetStateAction } from "react";
import Grid from "@mui/material/Grid2";
import TextField from "@mui/material/TextField";
import set from "lodash/set";

interface IBodyDialogContent {
  body: BodyStringType;
  setBody: Dispatch<SetStateAction<BodyStringType>>;
  setIsValidBodyForm: Dispatch<SetStateAction<boolean>>;
}

export default function BodyDialogContent(props: IBodyDialogContent) {
  const [bodyForm, setBodyForm] = useState<BodyStringType>();
  const [errorForm, setErrorForm] = useState<BodyStringType>();
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
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

    setErrorForm({
      name: props.body.name === "" ? "Name cannot be empty" : "",
      mass: "",
      radius: "",
      position: {
        x: "",
        y: "",
        z: "",
      },
      velocity: {
        x: "",
        y: "",
        z: "",
      },
    });
    setIsLoading(false);
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    props.setIsValidBodyForm(areAllFieldsEmpty(errorForm));
    // eslint-disable-next-line
  }, [errorForm]);

  function areAllFieldsEmpty(obj: any): boolean {
    if (obj === undefined) return false;
    return Object.values(obj).every((value) => {
      if (typeof value === "object" && value !== null) {
        return areAllFieldsEmpty(value);
      }
      return value === "";
    });
  }

  const isValidPositiveNumberString = (value: string): string => {
    const normalizedValue = value.replace(",", ".");
    if (!normalizedValue.trim()) return "Value cannot be empty";
    if (parseFloat(normalizedValue) === 0) return "Value cannot be zero";
    if (value.includes("e")) {
      if (value.includes(".")) {
        const regex = /^(-)?(\d+)[.](\d+)[e](\d+)$/;
        return regex.test(normalizedValue)
          ? ""
          : "Value must be a valid number";
      } else {
        const regex = /^(\d+)[e](-)?(\d+)$/;
        return regex.test(normalizedValue)
          ? ""
          : "Value must be a valid number";
      }
    } else if (value.includes(".")) {
      const regex = /^(\d+)[.](\d+)$/;
      return regex.test(normalizedValue) ? "" : "Value must be a valid float";
    } else {
      const regex = /^(\d+)$/;
      return regex.test(normalizedValue) ? "" : "Value must be a positive";
    }
  };

  const isValidNegativeNumberString = (value: string): string => {
    const normalizedValue = value.replace(",", ".");
    if (!normalizedValue.trim()) return "Value cannot be empty";
    if (value.includes("e")) {
      if (value.includes(".")) {
        const regex = /^(-)?(\d+)[.](\d+)[e](\d+)$/;
        return regex.test(normalizedValue)
          ? ""
          : "Value must be a valid number";
      } else {
        const regex = /^(-)?(\d+)[e](\d+)$/;
        return regex.test(normalizedValue)
          ? ""
          : "Value must be a valid number";
      }
    } else if (value.includes(".")) {
      const regex = /^(-)?(\d+)[.](\d+)$/;
      return regex.test(normalizedValue) ? "" : "Value must be a valid float";
    } else {
      const regex = /^(-)?(\d+)$/;
      return regex.test(normalizedValue) ? "" : "Value must be a positive";
    }
  };

  const isValidName = (value: string): boolean => {
    return !!value.trim();
  };

  const setBodyOrError = (isValid, name, value, message) => {
    setBodyForm((prev) => {
      const updatedValues = { ...prev };
      set(updatedValues, name, value.replace(",", "."));
      return updatedValues;
    });
    props.setBody((prev) => {
      const updatedValues = { ...prev };
      set(updatedValues, name, value.replace(",", "."));
      return updatedValues;
    });
    if (isValid) {
      setErrorForm((prev) => {
        const updatedValues = { ...prev };
        set(updatedValues, name, "");
        return updatedValues;
      });
    } else {
      setErrorForm((prev) => {
        const updatedValues = { ...prev };
        set(updatedValues, name, message);
        return updatedValues;
      });
    }
  };

  const handlePositiveNumber = (event) => {
    const { name, value } = event.target;
    const regex = /^(-)?(\d*)?[.,]?(\d*)[e]?(\d*)$/;
    if (regex.test(value)) {
      const message = isValidPositiveNumberString(value);
      setBodyOrError(message === "", name, value, message);
    }
  };

  const handleNegativeNumber = (event) => {
    const { name, value } = event.target;
    const regex = /^(-)?(\d*)?[.,]?(\d*)[e]?(\d*)$/;
    if (regex.test(value)) {
      const message = isValidNegativeNumberString(value);
      setBodyOrError(message === "", name, value, message);
    }
  };

  const handleName = (event) => {
    const { name, value } = event.target;
    const isValid = isValidName(value);
    setBodyOrError(isValid, name, value, "Name cannot be empty");
  };

  return (
    <>
      {!isLoading ? (
        <>
          <Grid container spacing={2}>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="name"
                label="Name"
                value={bodyForm.name}
                error={bodyForm.name === ""}
                helperText={errorForm.name}
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
                value={bodyForm.mass}
                error={errorForm.mass !== ""}
                helperText={errorForm.mass}
                onChange={handlePositiveNumber}
              />
            </Grid>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="radius"
                label="Radius"
                value={bodyForm.radius}
                error={errorForm.radius !== ""}
                helperText={errorForm.radius}
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
                value={bodyForm.position.x}
                error={errorForm.position.x !== ""}
                helperText={errorForm.position.x}
                onChange={handleNegativeNumber}
              />
            </Grid>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="position.y"
                label="Y Position"
                value={bodyForm.position.y}
                error={errorForm.position.y !== ""}
                helperText={errorForm.position.y}
                onChange={handleNegativeNumber}
              />
            </Grid>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="position.z"
                label="Z Position"
                value={bodyForm.position.z}
                error={errorForm.position.z !== ""}
                helperText={errorForm.position.z}
                onChange={handleNegativeNumber}
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
                value={bodyForm.velocity.x}
                error={errorForm.velocity.x !== ""}
                helperText={errorForm.velocity.x}
                onChange={handleNegativeNumber}
              />
            </Grid>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="velocity.y"
                label="Y Velocity"
                value={bodyForm.velocity.y}
                error={errorForm.velocity.y !== ""}
                helperText={errorForm.velocity.y}
                onChange={handleNegativeNumber}
              />
            </Grid>
            <Grid size={4}>
              <TextField
                margin="dense"
                variant="standard"
                name="velocity.z"
                label="Z Velocity"
                value={bodyForm.velocity.z}
                error={errorForm.velocity.z !== ""}
                helperText={errorForm.velocity.z}
                onChange={handleNegativeNumber}
              />
            </Grid>
          </Grid>
        </>
      ) : (
        <></>
      )}
    </>
  );
}
