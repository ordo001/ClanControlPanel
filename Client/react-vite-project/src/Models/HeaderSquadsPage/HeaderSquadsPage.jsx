import "./HeaderSquadsPage.css";
import { useEffect, useState } from "react";
import Modal from "../Modal/Modal";
import Button from "../Button/Button";
import Input from "../Input/Input";
import useAuth from "../../Func/useAuth";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
  useNavigate,
  redirect,
} from "react-router-dom";

export default function HeaderSquadsPage({ onUpdateListUsers }) {
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();
  const apiUrl = import.meta.env.VITE_API_URL;
  function openModal() {
    setIsAddModalOpen(true);
  }

  function closeModal() {
    setIsAddModalOpen(false);
  }

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

  async function addUser() {
    try {
      const response = await fetch(`${apiUrl}/api/Users`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ login: login, password: password, name: name }),
        credentials: "include",
      });

      if (response.ok) {
        console.log("aboba");
        onUpdateListUsers();
        setIsAddModalOpen(false);
      }
      //console.log(response.body);
    } catch (error) {
      console.log("Нет связи с сервером.", error);
    }
  }

  return (
    <header>
      <div className="container">
        <div id="Name">Панель управления [SOWA]</div>
        <nav>
          <ul className="nav-links">
            <li>
              <button
                className="nav-button"
                onClick={async (event) => {
                  navigate("/user-panel");
                }}
              >
                Аккаунты
              </button>
            </li>
            <li>
              <button
                className="nav-button"
                onClick={async (event) => {
                  navigate("/player-panel");
                }}
              >
                Редактировать
              </button>
            </li>
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
