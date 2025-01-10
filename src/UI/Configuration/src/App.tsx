import React, { useEffect, useState } from "react";
import { Box, Paper, Skeleton, Typography } from "@mui/material";
import { useGetBodiesRequest } from "./axiosBase/axiosBody";
import RocketLaunchIcon from "@mui/icons-material/RocketLaunch";
import useNotification from "./notifications/UseNotification";
import TopPanelButtons from "./components/TopPanelButtons";
import MainPanel from "./components/BodyListPanel";

const App = () => {
  const [bodies, setBodies] = useState<BodyType[]>([]);
  const [fetchedBodyNames, setFetchedBodyNames] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const getBodiesRequest = useGetBodiesRequest();

  useEffect(() => {
    setIsLoading(true);
    fetchBodies();
  }, []);

  useEffect(() => {
    setFetchedBodyNames(
      bodies.map((body: BodyType) => {
        return body.name;
      })
    );
  }, [bodies]);

  const fetchBodies = async () => {
    const currBodies: Array<BodyType> = await getBodiesRequest();
    setBodies(currBodies);
    setIsLoading(false);
  };

  useNotification({ onDatabaseChanged: fetchBodies });

  return (
    <>
      <AppHeader />
      {!isLoading ? (
        <Paper elevation={0}>
          <TopPanelButtons
            currentBodyNames={fetchedBodyNames}
            setBodies={setBodies}
          />
          <MainPanel bodies={bodies} setBodies={setBodies} />
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
