import "./UsersPanelPage.css";
import { Navigate, useNavigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderUsersPage from "../../Models/HeaderUsersPage/HeaderUsersPage";
import UserCard from "../../Models/UserCard/UserCard";
import { HubConnectionBuilder } from "@microsoft/signalr";
import Modal from "../../Models/Modal/Modal";
import Input from "../../Models/Input/Input";
import Button from "../../Models/Button/Button";
import Header from "../../Models/Header/Header";

const apiUrl = import.meta.env.VITE_API_URL;

export default function UsersPanelPage() {
  // const { isAuthenticated, loading } = useAuth();
  const navigate = useNavigate();
  const [users, setUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [connection, setConnection] = useState(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [name, setName] = useState("");

  function openModal() {
    setIsAddModalOpen(true);
  }

  function closeModal() {
    setIsAddModalOpen(false);
  }

  const fetchUsers = useCallback(async () => {
    const response = await fetch(`${apiUrl}/api/Users`, {
      method: "GET",
      credentials: "include",
    });
    const users = await response.json();
    setUsers(users);
  }, [apiUrl]);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/userHub`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection
      .start()
      .then(() => console.log("SignalR Connected"))
      .catch((err) => console.error("SignalR Connection Error: ", err));

    return () => {
      newConnection.stop();
    };
  }, [apiUrl]);

  // Обработка события обновления пользователей
  useEffect(() => {
    if (connection) {
      connection.on("UsersUpdated", fetchUsers);
    }

    return () => {
      if (connection) {
        connection.off("UsersUpdated");
      }
    };
  }, [connection, fetchUsers]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

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
        fetchUsers();
        setIsAddModalOpen(false);
      }
    } catch (error) {
      console.log("Нет связи с сервером.", error);
    }
  }

  const filteredUsers = users.filter((user) => {
    const matchesName = user.login
      .toLowerCase()
      .includes(searchTerm.toLowerCase());
    return matchesName;
  });

  return (
    <div>
      <Header>
        <li>
          <button
            className="nav-button"
            onClick={async (event) => {
              navigate("/squads");
            }}
          >
            Отряды
          </button>
        </li>
        <li>
          <button
            className="nav-button"
            onClick={async (event) => {
              navigate("/player-panel");
            }}
          >
            Состав
          </button>
        </li>
        <li>
          <button
            className="nav-button"
            onClick={async (event) => {
              navigate("/events");
            }}
          >
            Казна
          </button>
        </li>
      </Header>

      <div className="Info">
        Здесь отображается список всех пользователей в базе данных
      </div>

      <div className="Info">Всего хлопцев: {users.length}</div>

      <div
        style={{
          margin: "20px 0",
          display: "flex",
          gap: "16px",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <input
          type="text"
          placeholder="Поиск по логину..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          style={{
            padding: "8px 12px",
            fontSize: "16px",
            width: "400px",
            borderRadius: "6px",
            border: "1px solid #ccc",
          }}
        />
      </div>

      <div className="User-container">
        <div className="Add-user" onClick={openModal}>
          + Новый хлопец
        </div>
        {filteredUsers.length > 0 ? (
          filteredUsers.map((user) => (
            <UserCard
              key={user.id}
              Name={user.name}
              Login={user.login}
              Id={user.id}
              onUpdateListUsers={fetchUsers}
            />
          ))
        ) : (
          <p
            style={{ textAlign: "center", fontSize: "24px", marginTop: "20px" }}
          >
            Пользователей нет
          </p>
        )}
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
    </div>
  );
}
