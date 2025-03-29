import { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { IContextType, ICurrentUser } from "./types";

export const INITIAL_USER = {
  userId: "",
  name: "",
  email: "",
  role: "" as "teacher" | "student",
};

const INITIAL_STATE = {
  user: INITIAL_USER,
  isLoading: false,
  isAuthenticated: false,
  setUser: () => {},
  setIsAuthenticated: () => {},
  checkAuthUser: async () => false as boolean,
  login: async () => false as boolean,
  logout: () => {},
  register: async () => false as boolean,
};

const AuthContext = createContext<IContextType>(INITIAL_STATE);

// Mock database in localStorage
const LOCAL_STORAGE_KEY = "GradeHubAuth";

const getLocalStorageUsers = () => {
  const data = localStorage.getItem(LOCAL_STORAGE_KEY);
  return data ? JSON.parse(data) : { users: [] };
};

const saveLocalStorageUsers = (data: any) => {
  localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(data));
};

// Initialize with some demo users if none exist
const initializeDemoUsers = () => {
  const data = getLocalStorageUsers();
  if (data.users.length === 0) {
    data.users = [
      {
        userId: "1",
        name: "Demo Teacher",
        email: "teacher@example.com",
        password: "12345678", // In real app, passwords should be hashed
        role: "teacher",
      },
      {
        userId: "2",
        name: "Demo Student",
        email: "student@example.com",
        password: "12345678",
        role: "student",
      },
    ];
    saveLocalStorageUsers(data);
  }
};

initializeDemoUsers();

const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<ICurrentUser>(INITIAL_USER);
  const [isLoading, setIsLoading] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  const navigate = useNavigate();

  // Check if user is logged in (from localStorage)
  const checkAuthUser = async () => {
    setIsLoading(true);
    try {
      const currentUser = JSON.parse(
        localStorage.getItem("currentUser") || "null"
      );

      if (currentUser) {
        setUser(currentUser);
        setIsAuthenticated(true);
        return true;
      }
      return false;
    } catch (error) {
      console.error("Auth check error:", error);
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  // Login function
  const login = async (email: string, password: string) => {
    setIsLoading(true);
    try {
      const data = getLocalStorageUsers();
      const user = data.users.find(
        (u: any) => u.email === email && u.password === password
      );

      if (user) {
        const currentUser = {
          userId: user.userId,
          name: user.name,
          email: user.email,
          role: user.role,
        };

        setUser(currentUser);
        setIsAuthenticated(true);
        localStorage.setItem("currentUser", JSON.stringify(currentUser));

        // Redirect based on role
        if (user.role === "teacher") {
          navigate("/");
        } else {
          navigate("/");
        }

        return true;
      }
      return false;
    } catch (error) {
      console.error("Login error:", error);
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  // Logout function
  const logout = () => {
    setUser(INITIAL_USER);
    setIsAuthenticated(false);
    localStorage.removeItem("currentUser");
    navigate("/sign-in");
  };

  // Register function
  const register = async (userData: {
    name: string;
    email: string;
    password: string;
    role: "teacher" | "student";
  }) => {
    setIsLoading(true);
    try {
      const data = getLocalStorageUsers();

      // Check if user already exists
      if (data.users.some((u: any) => u.email === userData.email)) {
        throw new Error("User with this email already exists");
      }

      const newUser = {
        userId: Date.now().toString(),
        name: userData.name,
        email: userData.email,
        password: userData.password,
        role: userData.role,
      };

      data.users.push(newUser);
      saveLocalStorageUsers(data);

      // Auto-login after registration
      const currentUser = {
        userId: newUser.userId,
        name: newUser.name,
        email: newUser.email,
        role: newUser.role,
      };

      setUser(currentUser);
      setIsAuthenticated(true);
      localStorage.setItem("currentUser", JSON.stringify(currentUser));

      // Redirect based on role
      if (newUser.role === "teacher") {
        navigate("/");
      } else {
        navigate("/");
      }

      return true;
    } catch (error) {
      console.error("Registration error:", error);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  // Check auth state on initial load
  useEffect(() => {
    checkAuthUser();
  }, []);

  const value = {
    user,
    setUser,
    isLoading,
    isAuthenticated,
    setIsAuthenticated,
    checkAuthUser,
    login,
    logout,
    register,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthProvider;

export const useAuthContext = () => useContext(AuthContext);
