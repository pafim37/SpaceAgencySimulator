import React, { useState, Dispatch, SetStateAction, ReactNode } from "react";
import { Checkbox, IconButton } from "@mui/material";
import { GridColDef, GridRenderCellParams } from '@mui/x-data-grid';
import { useChangeStateBodyRequest } from "../axiosBase/useChangeStateBodyRequest";
import { DataGrid } from '@mui/x-data-grid';
import EditBodyDialog from "../dialogs/EditBodyDialog";
import { useDeleteBodyRequest } from "../axiosBase/useDeleteBodyRequest";
import DeleteIcon from "@mui/icons-material/Delete";
import ConfirmationDialog from "../dialogs/ConfirmationDialog";
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import { useCloneBodyRequest } from "../axiosBase/useCloneBodyRequest";

interface IMainPanel {
  bodies: BodyType[];
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

const COLUMN_WIDTHS = {
  CHECKBOX: 80,
  ACTION: 80,
  NAME: 150,
  NUMBER: 100,
} as const;

interface EnabledCellProps {
  value: boolean;
  bodyName: string;
  onToggle: (event: React.ChangeEvent<HTMLInputElement>, bodyName: string) => void;
}

const EnabledCell = ({ value, bodyName, onToggle }: EnabledCellProps): ReactNode => (
  <Checkbox
    checked={!!value}
    onChange={(event: React.ChangeEvent<HTMLInputElement>) => onToggle(event, bodyName)}
  />
);

interface RemoveCellProps {
  bodyName: string;
  onRemoveClick: (name: string) => void;
}

const RemoveCell = ({ bodyName, onRemoveClick }: RemoveCellProps): ReactNode => (
  <IconButton onClick={() => onRemoveClick(bodyName)} color="error">
    <DeleteIcon />
  </IconButton>
);

interface CloneCellProps {
  bodyName: string;
  onCloneClick: (name: string) => void;
}

const CloneCell = ({ bodyName, onCloneClick }: CloneCellProps): ReactNode => (
  <IconButton onClick={() => onCloneClick(bodyName)} color="primary">
    <ContentCopyIcon />
  </IconButton>
);

interface EditCellProps {
  body: BodyType | undefined;
  setBodies: Dispatch<SetStateAction<BodyType[]>>;
}

const EditCell = ({ body, setBodies }: EditCellProps): ReactNode =>
  body ? <EditBodyDialog body={body} setBodies={setBodies} /> : null;

const BodyListPanel = (props: IMainPanel) => {
  const [bodyNameToRemove, setBodyNameToRemove] = useState<string>("");
  const [isOpenConfirmationDialog, setIsOpenConfirmationDialog] = useState<boolean>(false);
  const changeStateBodyRequest = useChangeStateBodyRequest();
  const deleteBodyRequest = useDeleteBodyRequest();
  const cloneBodyRequest = useCloneBodyRequest();

  const openConfirmationDialog = (name: string): void => {
    setBodyNameToRemove(name);
    setIsOpenConfirmationDialog(true);
  };

  const closeConfirmationDialog = (): void => {
    setIsOpenConfirmationDialog(false);
  };

  const removeBody = async (): Promise<void> => {
    const deletedBody = await deleteBodyRequest(bodyNameToRemove);
    if (deletedBody !== undefined) {
      props.setBodies((prev) => prev.filter((b) => b.name !== deletedBody.name));
    }
    closeConfirmationDialog();
  };

  const handleEnableCheckbox = async (
    event: React.ChangeEvent<HTMLInputElement>,
    bodyName: string
  ): Promise<void> => {
    const updatedBody = await changeStateBodyRequest(bodyName, event.target.checked);
    if (updatedBody !== undefined) {
      props.setBodies((prev) =>
        prev.map((body) => (body.name === bodyName ? updatedBody : body))
      );
    }
  };

  const rows = props.bodies.map((body) => ({
    id: body.id,
    name: body.name,
    mass: body.mass,
    radius: body.radius,
    enabled: body.enabled,
    posX: body.position.x,
    posY: body.position.y,
    posZ: body.position.z,
    velX: body.velocity.x,
    velY: body.velocity.y,
    velZ: body.velocity.z,
  }));

  const columns: GridColDef[] = [
    {
      field: 'enabled',
      headerName: 'Enabled',
      width: COLUMN_WIDTHS.CHECKBOX,
      renderCell: (params: GridRenderCellParams) => (
        <EnabledCell
          value={params.value}
          bodyName={params.row.name}
          onToggle={handleEnableCheckbox}
        />
      ),
    },
    { field: 'name', headerName: 'Name', width: COLUMN_WIDTHS.NAME },
    { field: 'mass', headerName: 'Mass [kg]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'radius', headerName: 'Radius [m]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'posX', headerName: 'Pos X [m]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'posY', headerName: 'Pos Y [m]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'posZ', headerName: 'Pos Z [m]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'velX', headerName: 'Vel X [km/s]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'velY', headerName: 'Vel Y [km/s]', width: COLUMN_WIDTHS.NUMBER },
    { field: 'velZ', headerName: 'Vel Z [km/s]', width: COLUMN_WIDTHS.NUMBER },
    {
      field: 'edit',
      headerName: 'Edit',
      width: COLUMN_WIDTHS.ACTION,
      renderCell: (params: GridRenderCellParams) => (
        <EditCell
          body={props.bodies.find((b) => b.id === params.row.id)}
          setBodies={props.setBodies}
        />
      ),
    },
    {
      field: 'remove',
      headerName: 'Remove',
      width: COLUMN_WIDTHS.ACTION,
      renderCell: (params: GridRenderCellParams) => (
        <RemoveCell bodyName={params.row.name} onRemoveClick={openConfirmationDialog} />
      ),
    },
    {
      field: 'clone',
      headerName: 'Clone',
      width: COLUMN_WIDTHS.ACTION,
      renderCell: (params: GridRenderCellParams) => (
        <CloneCell bodyName={params.row.name} onCloneClick={cloneBodyRequest} />
      ),
    }
  ];

  return (
    <>
      <DataGrid rows={rows} columns={columns} />
      {isOpenConfirmationDialog && (
        <ConfirmationDialog
          open={isOpenConfirmationDialog}
          setOpen={closeConfirmationDialog}
          name={bodyNameToRemove}
          call={removeBody}
        />
      )}
    </>
  );
};

export default BodyListPanel;
