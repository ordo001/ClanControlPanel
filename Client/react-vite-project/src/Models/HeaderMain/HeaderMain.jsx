import "./HeaderMain.css";
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
} from "react-router-dom";

export default function HeaderMain({ onUpdateListUsers }) {
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();
  function openModal() {
    setIsAddModalOpen(true);
  }

  function closeModal() {
    setIsAddModalOpen(false);
  }

  async function logout() {
    try {
      const response = await fetch("http://localhost:5000/api/Auth/logout", {
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
      const response = await fetch("http://localhost:5000/api/User/add", {
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
        <div id="Name">Панель управления</div>
        <nav>
          <ul className="nav-links">
            <li>
              <button className="nav-button" onClick={openModal}>
                Добавить
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

      {isAddModalOpen && (
        <Modal open={isAddModalOpen}>
          <img
            className="imageCloseModal"
            src="krest.png"
            alt="Закрыть"
            onClick={closeModal}
          />
          <h3>Добавление нового пользователя</h3>
          <p>Заполните поля ниже</p>

          <div className="form-group">
            <Input
              id="login"
              type="text"
              placeholder="Логин"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
            />
            <Input
              id="name"
              type="name"
              placeholder="Имя"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <Input
              id="password"
              type="password"
              placeholder="Пароль"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          <Button onClick={addUser}>Добавить</Button>
        </Modal>
      )}
    </header>
  );
}
