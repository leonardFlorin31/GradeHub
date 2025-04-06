import { createContext, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { IContextType, ICurrentUser } from "./types";
import { set } from "zod";

export const INITIAL_USER = {
  userId: "",
  name: "",
  email: "",
  role: "" as "teacher" | "student",
};

const INITIAL_STATE: IContextType = {
  user: INITIAL_USER,
  isLoading: false,
  isAuthenticated: false,
  error: null, // Add error state
  setUser: () => {},
  setIsAuthenticated: () => {},
  checkAuthUser: async () => false as boolean,
  login: async () => ({ success: false, message: "" }), // Updated return type
  logout: () => {},
  register: async () => ({ success: false, message: "" }), // Updated return type
  clearError: () => {}, // Add clearError function
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
  const [error, setError] = useState<string | null>(null);

  const clearError = () => setError(null);

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
    setError(null);
    try {
      const res = await fetch("https://localhost:64060/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!res.ok) {
        const msg =
          res.status === 401 ? "Invalid email or password" : "Login failed";
        setError(msg);
        return { success: false, message: msg };
      }

      const data = await res.json();
      const currentUser = {
        userId: data.id,
        name: data.name,
        email: data.email,
        role: data.role.toLowerCase(), // Convert "Teacher"/"Student" to enum
      };

      setUser(currentUser);
      setIsAuthenticated(true);
      localStorage.setItem("currentUser", JSON.stringify(currentUser));

      navigate(currentUser.role === "teacher" ? "/" : "/");
      return { success: true };
    } catch (err) {
      setError("Login failed. Please try again.");
      return { success: false, message: "Login failed" };
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
  const register = async ({
    name,
    email,
    password,
    role,
    className, // display name (e.g. "Math 101")
    classId, // unique ID used for backend linking (e.g. "Math101")
  }: {
    name: string;
    email: string;
    password: string;
    role: "teacher" | "student";
    className?: string;
    classId?: string;
  }) => {
    setIsLoading(true);
    setError(null);

    try {
      const endpoint =
        role === "student"
          ? "https://localhost:64060/api/students"
          : "https://localhost:64060/api/teachers";

      const body =
        role === "student"
          ? { name, email, password }
          : { name, email, password, classId, className };

      const res = await fetch(endpoint, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });

      if (!res.ok) {
        setError("Registration failed.");
        return { success: false, message: "Registration failed." };
      }

      const data = await res.json();
      const currentUser = {
        userId: data.id ?? data.teacherId ?? data.classId,
        name: data.name ?? name,
        email,
        role,
      };

      setUser(currentUser);
      setIsAuthenticated(true);
      localStorage.setItem("currentUser", JSON.stringify(currentUser));
      navigate(role === "teacher" ? "/" : "/");

      return { success: true };
    } catch (error) {
      setError("Something went wrong during registration.");
      return { success: false, message: "Something went wrong." };
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
    error,
    clearError,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthProvider;

export const useAuthContext = () => useContext(AuthContext);
