import "./HeaderPlayerPage.css";
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
import ComboBox from "../ComboBox/ComboBox";

export default function HeaderPlayersPage({ onUpdateListPlayers }) {
  const [squads, setSquads] = useState([]);
  const [connection, setConnection] = useState(null);
  const [selectedId, setSelectedId] = useState(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");
  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();
  const apiUrl = import.meta.env.VITE_API_URL;
  async function openModal() {
    setIsAddModalOpen(true);
    await getSquads();
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

  const handleSelectionChange = (id) => {
    setSelectedId(id);
  };

  async function addPlayer() {
    try {
      const response = await fetch(`${apiUrl}/api/Player`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ name: name, squadId: selectedId }),
        credentials: "include",
      });

      if (response.ok) {
        onUpdateListPlayers();
        setIsAddModalOpen(false);
      }
      if (response.status >= 400) {
        const errorData = await response.json();
        const errorMessage = errorData.Message;
        alert(`Ошибка: ${errorMessage}`);
      }
      //console.log(response.body);
    } catch (error) {
      console.log("Нет связи с сервером.", error);
    }
  }

  async function getSquads() {
    try {
      const response = await fetch(`${apiUrl}/api/Squad`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });

      if (response.ok) {
        const data = await response.json();
        setSquads(data);
      }
    } catch (error) {
      console.error("Нет связи с сервером:", error);
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
          <h3>Добавление нового игрока</h3>
          <p>Заполните поля ниже</p>

          <div className="form-group">
            <Input
              id="name"
              type="text"
              placeholder="Ник"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <ComboBox
              options={squads.map((item) => ({
                id: item.id,
                name: item.nameSquad,
              }))}
              onChange={handleSelectionChange}
              selectedId={selectedId}
            />
          </div>

          <Button onClick={addPlayer}>Добавить</Button>
        </Modal>
      )}
    </header>
  );
}
