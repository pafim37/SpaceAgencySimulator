import React, { createContext, useContext, useState } from "react";

interface IConnectionIdContext {
  connectionId: string | null;
  setConnectionId: (id: string) => void;
}

const ConnectionIdContext = createContext<IConnectionIdContext>(undefined);

export const ConnectionIdProvider: React.FC = ({ children }) => {
  const [connectionId, setConnectionId] = useState<string>("sas");

  return (
    <ConnectionIdContext.Provider value={{ connectionId, setConnectionId }}>
      {children}
    </ConnectionIdContext.Provider>
  );
};

export const useConnectionId = () => {
  const context = useContext(ConnectionIdContext);
  if (!context) {
    throw new Error(
      "useConnectionId must be used within a ConnectionIdProvider"
    );
  }
  return context;
};
