import { Route, Routes } from "react-router-dom";
import AuthLayout from "./pages/_auth/AuthLayout";
import SigninForm from "./pages/_auth/SigninForm";
import SignupForm from "./pages/_auth/SignupForm";
import RootLayout from "./pages/RootLayout";
import Home from "./pages/Home";
import Profile from "./pages/Profile";

import "./globals.css";

function App() {
  return (
    <main className="flex h-screen">
      <Routes key={location.pathname} location={location}>
        <Route element={<AuthLayout />}>
          <Route path="/sign-in" element={<SigninForm />} />
          <Route path="/sign-up" element={<SignupForm />} />
        </Route>

        <Route element={<RootLayout />}>
          <Route index element={<Home />} />
          {/* <Route path="/profile/:id/*" element={<Profile />} /> */}
          <Route path="/profile" element={<Profile />} />
        </Route>
      </Routes>
    </main>
  );
}

export default App;
