import { Route, Routes } from "react-router-dom";
import AuthLayout from "./pages/_auth/AuthLayout";
import SigninForm from "./pages/_auth/SigninForm";
import SignupForm from "./pages/_auth/SignupForm";
import RootLayout from "./pages/RootLayout";
import Home from "./pages/Home";
import Profile from "./pages/Profile";

import "./globals.css";
import GradesPage from "./pages/GradesPage";
import ResetPassword from "./pages/ResetPassword";

function App() {
  return (
    <main className="flex h-screen">
      <Routes key={location.pathname} location={location}>
        <Route element={<AuthLayout />}>
          <Route path="/sign-in" element={<SigninForm />} />
          <Route path="/sign-up" element={<SignupForm />} />
        </Route>

        <Route element={<RootLayout />}>
          <Route index element={<Profile />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/my-grades" element={<GradesPage />} />
          <Route path="/class-grades" element={<GradesPage />} />
          <Route path="/reset-password" element={<ResetPassword />} />
        </Route>
      </Routes>
    </main>
  );
}

export default App;
