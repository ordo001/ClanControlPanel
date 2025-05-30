import "./HeaderPlayerPage.css";
import { useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
  useNavigate,
} from "react-router-dom";

export default function HeaderPlayersPage({ onUpdateListPlayers }) {
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();
  const apiUrl = import.meta.env.VITE_API_URL;

  async function logout() {
    try {
      const response = await fetch(`${apiUrl}/api/Auth/Logout`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        setIsAuthenticated(false);
        navigate("/");
      }
    } catch (error) {
      console.log("Нет связи с сервером.", error);
    }
  }
  return (
    <header>
      <div className="container">
        <div
          id="Name"
          onClick={async (event) => {
            navigate("/squads");
          }}
        >
          Панель управления [SOWA]
        </div>
        <nav>
          <ul className="nav-links">
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
