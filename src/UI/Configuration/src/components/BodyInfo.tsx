import React, { useState } from "react";
import { useDeleteBodyRequest } from "../axiosBase/useDeleteBodyRequest";
import DeleteIcon from "@mui/icons-material/Delete";
import { Button, Paper, Typography } from "@mui/material";
import Grid from "@mui/material/Grid2";
import EditBodyDialog from "../dialogs/EditBodyDialog";
import ConfirmationDialog from "../dialogs/ConfirmationDialog";

interface IBodyInfo {
  key: string;
  body: BodyType;
  setBodies: React.Dispatch<React.SetStateAction<BodyType[]>>;
  color: string;
}

const BodyInfo = (props: IBodyInfo) => {
  const [isOpenConfirmationDialog, setIsOpenConfirmationDialog] =
    useState<boolean>(false);
  const deleteBodyRequest = useDeleteBodyRequest();
  const color = props.color;

  const openConfirmationDialog = () => {
    setIsOpenConfirmationDialog(true);
  };

  const removeBody = async () => {
    const deletedBody: BodyType = await deleteBodyRequest(props.body.name);
    if (deletedBody !== undefined) {
      props.setBodies((prev: BodyType[]) =>
        prev.filter((b) => b.name !== deletedBody.name)
      );
    }
  };

  return (
    <Paper elevation={2} sx={{ p: 2, m: 2 }}>
      <Grid
        container
        spacing={2}
        display={"flex"}
        justifyContent={"flex-start"}
        sx={{
          border: 1,
          boxShadow: 1,
          borderColor: color,
          borderRadius: 4,
          p: 2,
          color: color,
        }}
      >
        <Grid size={12}>
          <Typography>Mass: {props.body.mass}</Typography>
        </Grid>
        <Grid size={12}>
          <Typography>Radius: {props.body.radius}</Typography>
        </Grid>
        <Grid size={12}>
          <Typography>
            Position: {"("}
            {props.body.position.x}, {props.body.position.y},{" "}
            {props.body.position.z}
            {")"}
          </Typography>
        </Grid>
        <Grid size={12}>
          <Typography>
            Velocity: {"("}
            {props.body.velocity.x}, {props.body.velocity.y},{" "}
            {props.body.velocity.z}
            {")"}
          </Typography>
        </Grid>
      </Grid>
      <Grid
        container
        spacing={2}
        display="flex"
        justifyContent="flex-end"
        sx={{ pt: 2 }}
      >
        <Button
          startIcon={<DeleteIcon />}
          variant="outlined"
          onClick={openConfirmationDialog}
          color="error"
        >
          <Typography>Delete body</Typography>
        </Button>
        <EditBodyDialog body={props.body} setBody={props.setBodies} />
      </Grid>
      {isOpenConfirmationDialog ? (
        <ConfirmationDialog
          open={isOpenConfirmationDialog}
          setOpen={setIsOpenConfirmationDialog}
          name={props.body.name}
          call={removeBody}
        />
      ) : (
        <></>
      )}
    </Paper>
  );
};

export default BodyInfo;
