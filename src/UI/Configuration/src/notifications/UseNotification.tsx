import { useEffect, useState } from "react";
import { useConnectionId } from "../providers/ConnectionIdContext";
import * as signalR from "@microsoft/signalr";

interface INotification {
  onDatabaseChanged: () => void;
}
const useNotification = ({ onDatabaseChanged }: INotification) => {
  const { setConnectionId } = useConnectionId();
  const [connection, setConnection] = useState(null);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:6443/notification")
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          connection.invoke("GetConnectionId").then((id) => {
            console.log("Connection Id", id);
            setConnectionId(id);
          });
          console.log("Connected!");
        })
        .catch((e) => console.log("Connection failed", e));

      return () => {
        connection?.stop();
      };
    }
  }, [connection]);

  useEffect(() => {
    if (connection) {
      const handleDatabaseChanged = () => {
        console.log("Received BodyDatabaseChanged");
        onDatabaseChanged();
      };

      connection.on("BodyDatabaseChanged", handleDatabaseChanged);

      return () => {
        connection.off("BodyDatabaseChanged", handleDatabaseChanged);
      };
    }
  }, [connection, onDatabaseChanged]);

  return connection;
};

export default useNotification;
