export type ICurrentUser = {
  userId: string;
  name: string;
  email: string;
  role: "student" | "teacher";
};

export interface IContextType {
  user: ICurrentUser;
  isLoading: boolean;
  isAuthenticated: boolean;
  error: string | null;
  setUser: React.Dispatch<React.SetStateAction<ICurrentUser>>;
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>;
  checkAuthUser: () => Promise<boolean>;
  login: (
    email: string,
    password: string
  ) => Promise<{ success: boolean; message?: string }>;
  logout: () => void;
  register: (userData: {
    name: string;
    email: string;
    password: string;
    role: "teacher" | "student";
  }) => Promise<{ success: boolean; message?: string }>;
  clearError: () => void;
}

export type INavLink = {
  imgURL: string;
  route: string;
  label: string;
};
