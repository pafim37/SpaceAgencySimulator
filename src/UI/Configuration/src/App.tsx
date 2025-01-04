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
import { useGetBodiesRequest } from "./axiosBase/axiosBody";
import RocketLaunchIcon from "@mui/icons-material/RocketLaunch";
import { ExpandLess, ExpandMore } from "@mui/icons-material";

const App = () => {
  const [bodies, setBodies] = useState<BodyType[]>([]);
  const [fetchedBodies, setFetchedBodies] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [openNestedList, setOpenNestedList] = useState<boolean[]>([]);
  const getBodiesRequest = useGetBodiesRequest();

  useEffect(() => {
    setIsLoading(true);
    fetchBodies();
    setIsLoading(false);
    // eslint-disable-next-line
  }, []);

  const fetchBodies = async () => {
    const currBodies: Array<BodyType> = await getBodiesRequest();
    setBodies(currBodies);
    setFetchedBodies(
      currBodies.map((body) => {
        return body.name;
      })
    );
    setOpenNestedList(Array(currBodies.length).fill(false));
  };

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
          <Box
            sx={{
              maxHeight: "calc(90vh - 100px)",
              overflow: "auto",
              borderRadius: 2,
            }}
          >
            <List>
              {bodies.map((body: BodyType, index: number) => (
                <Paper elevation={1} key={index} sx={{ p: 1, m: 1 }}>
                  <ListItemButton
                    key={index}
                    onClick={() => handleNestedList(index)}
                  >
                    <Typography color="#30cb0">{body.name}</Typography>
                    {openNestedList[index] ? <ExpandLess /> : <ExpandMore />}
                  </ListItemButton>
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
