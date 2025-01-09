import React, { useEffect, useState } from "react";
import AddBodyDialog from "./dialogs/AddBodyDialog";
import {
  Box,
  Checkbox,
  Collapse,
  List,
  ListItemButton,
  Paper,
  Skeleton,
  Typography,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import BodyInfo from "./components/BodyInfo";
import {
  useGetBodiesRequest,
  useChangeStateBodyRequest,
} from "./axiosBase/axiosBody";
import RocketLaunchIcon from "@mui/icons-material/RocketLaunch";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import useNotification from "./notifications/UseNotification";

const App = () => {
  const [bodies, setBodies] = useState<BodyType[]>([]);
  const [fetchedBodies, setFetchedBodies] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [openNestedList, setOpenNestedList] = useState<boolean[]>([]);
  const getBodiesRequest = useGetBodiesRequest();
  const changeStateBodyRequest = useChangeStateBodyRequest();

  useEffect(() => {
    fetchBodies();
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    console.log(bodies);
  }, [bodies]);

  const fetchBodies = async () => {
    setIsLoading(true);
    const currBodies: Array<BodyType> = await getBodiesRequest();
    setBodies(currBodies);
    setFetchedBodies(
      currBodies.map((body) => {
        return body.name;
      })
    );
    setOpenNestedList(Array(currBodies.length).fill(false));
    setIsLoading(false);
  };

  useNotification({ onDatabaseChanged: fetchBodies });

  const handleNestedList = (index: number) => {
    const newNestedList = [...openNestedList];
    newNestedList[index] = !newNestedList[index];
    setOpenNestedList(newNestedList);
  };

  const handleEnableCheckbox = (event: React.ChangeEvent, bodyname: string) => {
    const isSuccess = changeStateBodyRequest(bodyname, event.target.checked);
    if (isSuccess) {
      setBodies((prev: BodyType[]) =>
        prev.map((body) =>
          body.name === bodyname ? { ...body, enabled: !body.enabled } : body
        )
      );
    }
  };

  return (
    <>
      <AppHeader />
      {!isLoading ? (
        <Paper elevation={0}>
          <Box display="flex" justifyContent={"flex-end"} sx={{ p: 2 }}>
            <AddBodyDialog setBodies={setBodies} />
          </Box>
          <Box
            sx={{
              maxHeight: "calc(90vh - 100px)",
              overflow: "auto",
              borderRadius: 2,
            }}
          >
            <List>
              {bodies.map((body: BodyType, index: number) => (
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
                      onClick={() => handleNestedList(index)}
                    >
                      <Typography
                        color={
                          fetchedBodies.includes(body.name)
                            ? "#30c9b0"
                            : "#d32f2f"
                        }
                      >
                        {body.name}
                      </Typography>
                      {openNestedList[index] ? <ExpandLess /> : <ExpandMore />}
                    </ListItemButton>
                  </Grid>
                  <Collapse in={openNestedList[index]} timeout="auto">
                    <BodyInfo
                      key={body.name}
                      body={body}
                      setBodies={setBodies}
                      color={
                        fetchedBodies.includes(body.name)
                          ? "#30c9b0"
                          : "#d32f2f"
                      }
                    />
                  </Collapse>
                </Paper>
              ))}
            </List>
          </Box>
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
