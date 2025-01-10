import React, { useState, Dispatch, SetStateAction } from "react";
import {
  Box,
  Checkbox,
  Collapse,
  List,
  ListItemButton,
  Paper,
  Typography,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import BodyInfo from "./BodyInfo";
import { useChangeStateBodyRequest } from "../axiosBase/axiosBody";
import { ExpandLess, ExpandMore } from "@mui/icons-material";

interface IMainPanel {
  bodies: BodyType[];
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

const BodyListPanel = (props: IMainPanel) => {
  const [openNestedList, setOpenNestedList] = useState<string[]>([]);
  const changeStateBodyRequest = useChangeStateBodyRequest();
  const fetchedBodyNames: string[] = props.bodies.map((b) => {
    return b.name;
  });

  const handleNestedList = (e, name: string) => {
    if (openNestedList.includes(name)) {
      setOpenNestedList((prev: string[]) =>
        prev.filter((element) => element !== name)
      );
    } else {
      setOpenNestedList((prev: string[]) => [...prev, name]);
    }
  };

  const handleEnableCheckbox = async (
    event: React.ChangeEvent,
    bodyname: string
  ) => {
    const updatedBody = await changeStateBodyRequest(
      bodyname,
      event.target.checked
    );
    if (updatedBody !== undefined) {
      props.setBodies((prev: BodyType[]) =>
        prev.map((body) => (body.name === bodyname ? updatedBody : body))
      );
    }
  };

  return (
    <Box
      sx={{
        maxHeight: "calc(90vh - 100px)",
        overflow: "auto",
        borderRadius: 2,
      }}
    >
      <List>
        {props.bodies.map((body: BodyType) => (
          <Paper elevation={1} key={body.name} sx={{ p: 1, m: 1 }}>
            <Grid container>
              <Checkbox
                checked={body.enabled}
                onChange={(event: React.ChangeEvent) =>
                  handleEnableCheckbox(event, body.name)
                }
              />
              <ListItemButton
                key={body.name}
                selected={openNestedList.includes(body.name)}
                onClick={(e) => handleNestedList(e, body.name)}
              >
                <Typography
                  color={
                    fetchedBodyNames.includes(body.name) ? "#30c9b0" : "#d32f2f"
                  }
                >
                  {body.name}
                </Typography>
                {openNestedList.includes(body.name) ? (
                  <ExpandLess />
                ) : (
                  <ExpandMore />
                )}
              </ListItemButton>
            </Grid>
            <Collapse in={openNestedList.includes(body.name)} timeout="auto">
              <BodyInfo
                key={body.name}
                body={body}
                setBodies={props.setBodies}
                color={
                  fetchedBodyNames.includes(body.name) ? "#30c9b0" : "#d32f2f"
                }
              />
            </Collapse>
          </Paper>
        ))}
      </List>
    </Box>
  );
};

export default BodyListPanel;
