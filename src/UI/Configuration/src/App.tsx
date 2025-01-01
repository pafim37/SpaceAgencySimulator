import React, { useEffect, useState } from "react";
import AddBodyDialog from "./dialogs/AddBodyDialog";
import {
  Box,
  Collapse,
  List,
  ListItemButton,
  Paper,
  Skeleton,
  Typography,
} from "@mui/material";
import BodyInfo from "./components/BodyInfo";
import { axiosGetBodies } from "./axiosBase/axiosBody";
import SnackbarAlert from "./alerts/SnackbarAlert";
import RocketLaunchIcon from "@mui/icons-material/RocketLaunch";
import { ExpandLess, ExpandMore } from "@mui/icons-material";

const App = () => {
  const [bodies, setBodies] = useState<BodyType[]>([]);
  const [fetchedBodies, setFetchedBodies] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [openSnackbarAlert, setOpenSnackbarAlert] = useState<boolean>(false);
  const [openNestedList, setOpenNestedList] = useState<boolean[]>([]);

  useEffect(() => {
    const fetchBodies = async () => {
      const currBodies: Array<BodyType> = await axiosGetBodies();
      if (currBodies === undefined) {
        setOpenSnackbarAlert(true);
      } else {
        setBodies(currBodies);
        setFetchedBodies(
          currBodies.map((body) => {
            return body.name;
          })
        );
        setOpenNestedList(Array(currBodies.length).fill(false));
        setIsLoading(false);
      }
    };
    fetchBodies();
  }, []);

  const handleNestedList = (index: number) => {
    const newNestedList = [...openNestedList];
    newNestedList[index] = !newNestedList[index];
    setOpenNestedList(newNestedList);
  };

  return (
    <>
      <AppHeader />
      {!isLoading ? (
        <Paper elevation={0}>
          <Box display="flex" justifyContent={"flex-end"} sx={{ p: 2 }}>
            <AddBodyDialog setBodies={setBodies} />
          </Box>
          <List>
            {bodies.map((body: BodyType, index) => (
              <Paper elevation={1} key={index} sx={{ p: 1, m: 1 }}>
                <ListItemButton
                  key={index}
                  onClick={() => handleNestedList(index)}
                >
                  <Typography>{body.name}</Typography>
                  {openNestedList[index] ? <ExpandLess /> : <ExpandMore />}
                </ListItemButton>
                <Collapse in={openNestedList[index]} timeout="auto">
                  <BodyInfo
                    key={body.name}
                    body={body}
                    setBodies={setBodies}
                    color={
                      fetchedBodies.includes(body.name) ? "#30c9b0" : "#d32f2f"
                    }
                  />
                </Collapse>
              </Paper>
            ))}
          </List>
        </Paper>
      ) : (
        <>
          <Box
            sx={{
              width: "100%",
              display: "flex",
              justifyContent: "center",
            }}
          >
            <Skeleton variant="rectangular" width={"85%"} height={"90vh"} />
          </Box>
          {openSnackbarAlert ? (
            <SnackbarAlert
              openSnackbarAlert={openSnackbarAlert}
              setOpenSnackbarAlert={setOpenSnackbarAlert}
              message={"Cannot fetch bodies from the server"}
            />
          ) : (
            <></>
          )}
        </>
      )}
    </>
  );
};

const AppHeader = () => {
  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        pb: 1,
      }}
    >
      <RocketLaunchIcon sx={{ color: "#30c9b0", fontSize: "3.2vw", pr: 1 }} />
      <Typography sx={{ color: "#30c9b0", fontSize: "3.2vw" }}>
        Space Agency Simulator
      </Typography>
    </Box>
  );
};
export default App;
