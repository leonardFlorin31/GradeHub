import { createContext, useContext, useState, ReactNode } from "react";

// Define the type for the current user
export type ICurrentUser = {
  userId: string;
  name: string;
  username: string;
};

// Define the context value type
interface AuthContextType {
  currentUser: ICurrentUser | null;
  setCurrentUser: (user: ICurrentUser | null) => void;
}

// Define the initial user state
const initialUser: ICurrentUser = {
  userId: "0000", // Default user ID
  name: "Guest User", // Default name
  username: "guest", // Default username
};

// Create the context with a default value
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Create a provider component
export const AuthProvider = ({ children }: { children: ReactNode }) => {
  // Set the initial state to the initialUser
  const [currentUser, setCurrentUser] = useState<ICurrentUser | null>(
    initialUser
  );

  return (
    <AuthContext.Provider value={{ currentUser, setCurrentUser }}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook to use the AuthContext
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
