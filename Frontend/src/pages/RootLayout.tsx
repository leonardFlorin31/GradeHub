// import LeftSidebar from "@/components/shared/LeftSidebar";
import { Outlet } from "react-router-dom";
import LeftSidebar from "../components/LeftSidebar";

const RootLayout = () => {
  return (
    <div className="w-full md:flex">
      <LeftSidebar />

      <section className="flex flex-1 h-full bg-dm-dark">
        <Outlet />
      </section>
    </div>
  );
};

export default RootLayout;
