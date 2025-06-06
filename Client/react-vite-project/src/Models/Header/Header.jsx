// src/Models/Header/Header.jsx
import { useNavigate } from "react-router-dom";
import { useLogout } from "../../Func/Logout";
import "./Header.css";

export default function Header({ children }) {
  const logout = useLogout();
  const navigate = useNavigate();
  return (
    <header className="header">
      <div className="container-header">
        <div id="Name" onClick={() => navigate("/squads")}>
          Панель управления [SOWA]
        </div>
        <nav>
          <ul className="nav-links">
            {children}
            <li>
              <button className="nav-button" onClick={logout}>
                Выход
              </button>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
}
