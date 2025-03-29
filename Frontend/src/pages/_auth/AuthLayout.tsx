import { useAuthContext } from "@/AuthContext";
import { Outlet, Navigate } from "react-router-dom";

const AuthLayout = () => {
  const { isAuthenticated } = useAuthContext();
  return (
    <>
      {isAuthenticated ? (
        <Navigate to="/" />
      ) : (
        <div className="relative h-screen w-screen bg-rich-black">
          <section className="absolute inset-0 flex justify-center items-center z-10">
            <div className="sm:w-420 flex-center flex-col bg-dark bg-opacity-70 text-white p-6 rounded-lg shadow-lg">
              <Outlet />
            </div>
          </section>
        </div>
      )}
    </>
  );
};

export default AuthLayout;
