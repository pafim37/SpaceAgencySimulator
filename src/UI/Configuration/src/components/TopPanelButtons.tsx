import React, { useEffect, useState, Dispatch, SetStateAction } from "react";
import {
  Box,
  Button,
  ButtonGroup,
  Checkbox,
  Collapse,
  Typography,
} from "@mui/material";
import Grid from "@mui/material/Grid2";
import AddBodyDialog from "./../dialogs/AddBodyDialog";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { useCreateBodyDefaultsRequest } from "./../axiosBase/useCreateBodyDefaultsRequest";
import { useGetBodySupportedNames } from "./../axiosBase/useGetBodySupportedNames";

interface ITopPanelButtons {
  currentBodyNames: string[];
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

const TopPanelButtons = (props: ITopPanelButtons) => {
  const [supportedNames, setSupportedNames] = useState<string[]>([]);
  const [namesToCreate, setNamesToCreate] = useState<string[]>([]);
  const [isExpand, setIsExpand] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const createBodyDefaultsRequest = useCreateBodyDefaultsRequest();
  const getBodySupportedNames = useGetBodySupportedNames();

  useEffect(() => {
    fetchSupportedNames();
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    setNamesToCreate(
      supportedNames.filter((sn) => !props.currentBodyNames.includes(sn))
    );
    // eslint-disable-next-line
  }, [supportedNames]);

  const fetchSupportedNames = async () => {
    const currSupportedNames = await getBodySupportedNames();
    setSupportedNames(currSupportedNames);
    setIsLoading(false);
  };

  const handleCheckbox = (event: React.ChangeEvent, name: string) => {
    if (event.target.checked) {
      setNamesToCreate((prev: string[]) => [...prev, name]);
    } else {
      setNamesToCreate((prev: string[]) => [...prev.filter((p) => p !== name)]);
    }
  };

  const sendRequest = async () => {
    const result: string[] = await createBodyDefaultsRequest(namesToCreate);
    if (result !== undefined) {
      setNamesToCreate((prev: string[]) =>
        prev.filter((name) => !result.some((r) => r === name))
      );
    }
  };

  return (
    <Box display="flex" justifyContent={"flex-end"} sx={{ p: 2 }}>
      {!isLoading ? (
        <Grid container>
          <Grid size={12} sx={{ pr: 2 }}>
            <ButtonGroup>
              <Button
                variant="outlined"
                onClick={sendRequest}
                style={{ backgroundColor: "#1fb89f", color: "black" }}
                disabled={namesToCreate.length < 1}
              >
                Create default bodies ({namesToCreate.length} /{" "}
                {
                  supportedNames.filter(
                    (sn) => !props.currentBodyNames.some((cbn) => cbn === sn)
                  ).length
                }
                )
              </Button>
              <Button
                onClick={() => {
                  setIsExpand(!isExpand);
                }}
                style={{ backgroundColor: "#1fb89f", color: "black" }}
              >
                {isExpand ? <ExpandLess /> : <ExpandMore />}
              </Button>
            </ButtonGroup>
          </Grid>
          <Grid size={12}>
            <Box
              style={{
                position: "relative",
                width: "100%",
              }}
            >
              <Collapse in={isExpand} timeout="auto">
                <Box
                  style={{
                    position: "absolute",
                    backgroundColor: "rgba(0,0,0,0.3)",
                    zIndex: 1,
                    width: "100%",
                    borderRadius: 20,
                  }}
                >
                  {supportedNames.map((name: string, index: number) => (
                    <Grid
                      container
                      key={index}
                      alignItems={"center"}
                      justifyContent={"flex-start"}
                    >
                      <Checkbox
                        disabled={props.currentBodyNames.includes(name)}
                        checked={namesToCreate.includes(name)}
                        onChange={(e: React.ChangeEvent) =>
                          handleCheckbox(e, name)
                        }
                      />
                      <Typography key={index}>{name}</Typography>
                    </Grid>
                  ))}
                </Box>
              </Collapse>
            </Box>
          </Grid>
        </Grid>
      ) : (
        <></>
      )}
      <AddBodyDialog setBodies={props.setBodies} />
    </Box>
  );
};

export default TopPanelButtons;
