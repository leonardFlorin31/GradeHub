import { Link, NavLink, useNavigate, useLocation } from "react-router-dom";
import { sidebarLinks } from "../constants";
import { INavLink } from "../types";
import { useAuthContext } from "../AuthContext";

const LeftSidebar = () => {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const { user, logout } = useAuthContext();

  if (!user) {
    return <div>Loading...</div>; // Or redirect to login, or show a fallback UI
  }

  return (
    <nav className="hidden md:flex px-6 py-10 flex-col justify-between w-[300px] bg-lm-secondary dark:bg-dm-dark">
      <div className="flex flex-col gap-8">
        <Link
          to={`/profile/${user.userId}`}
          className="flex gap-3 items-center"
        >
          <div className="flex flex-col">
            <p className="body-bold">{user.name}</p>
          </div>
        </Link>

        <ul className="flex flex-col gap-4">
          {sidebarLinks.map((link: INavLink) => {
            const isActive = pathname === link.route;

            return (
              <li
                key={link.label}
                className={`rounded-lg base-medium hover:dark:bg-dm-secondary hover:bg-lm-primary hover:text-lm-light transition group ${
                  isActive &&
                  " hover:dark:bg-dm-secondary hover:bg-lm-primary hover:text-lm-light bg-lm-primary dark:bg-dm-secondary text-lm-light"
                }`}
              >
                <NavLink
                  to={link.route}
                  className="flex gap-4 items-center p-4"
                >
                  {link.label}
                </NavLink>
              </li>
            );
          })}
        </ul>
      </div>

      <button
        className="flex gap-4 items-center justify-start hover:bg-transparent hover:text-white  mt-9"
        onClick={() => {
          logout();
        }}
      >
        Logout
      </button>
    </nav>
  );
};

export default LeftSidebar;
