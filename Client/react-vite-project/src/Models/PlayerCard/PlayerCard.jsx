import Button from "../Button/Button";
import "./PlayerCard.css";
import Modal from "../Modal/Modal";
import { useState, useEffect } from "react";
import Input from "../Input/Input";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";

export default function PlayerCard({
  Name,
  Id,
  SquadName,
  SquadId,
  onUpdateListPlayers,
}) {
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [name, setName] = useState("");
  const [connection, setConnection] = useState(null);
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();
  const [enabledCheckBox, setEnabledCheckBox] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const apiUrl = import.meta.env.VITE_API_URL;

  async function confirmDelete() {
    const response = await fetch(`${apiUrl}/api/Players/${Id}`, {
      method: "DELETE",
      credentials: "include",
    });

    if (response.ok) {
      onUpdateListPlayers();
    }
    setIsDeleteModalOpen(false);
  }

  async function UpdatePlayer() {
    const response = await fetch(`${apiUrl}`, {
      // ДОДЕЛАТЬ!!!!
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        Id: Id,
        Login: login,
        Password: password,
        Name: name,
      }),
      credentials: "include",
    });

    if (response.ok) {
      onUpdateListPlayers();
      closeModal();
    }
  }

  function openModal() {
    setIsAddModalOpen(true);
  }

  function closeModal() {
    setIsAddModalOpen(false);
  }

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/playerHub`)
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
      connection.on("PlayersUpdated", onUpdateListPlayers);
    }

    return () => {
      if (connection) {
        connection.off("PlayersUpdated");
      }
    };
  }, [connection, onUpdateListPlayers]);

  return (
    <>
      {isDeleteModalOpen && (
        <Modal open={isDeleteModalOpen}>
          <img
            className="imageCloseModal"
            src="krest.png"
            alt="Закрыть"
            onClick={() => setIsDeleteModalOpen(false)}
          />
          <h3>Подтверждение удаления</h3>
          <p>Вы уверены, что хотите удалить игрока {Name}?</p>
          <div style={{ display: "flex", gap: "10px" }}>
            <Button onClick={() => setIsDeleteModalOpen(false)}>Отмена</Button>
            <Button onClick={confirmDelete} style={{ background: "#ff4444" }}>
              Удалить
            </Button>
          </div>
        </Modal>
      )}

      {isAddModalOpen && (
        <Modal open={isAddModalOpen}>
          <img
            className="imageCloseModal"
            src="krest.png"
            alt="Закрыть"
            onClick={closeModal}
          />
          <h3>Изменить данные игрока</h3>
          <p>
            Измените поля ниже <br></br>
            (Оставьте строки пустыми, если хотите оставить поля без изменений)
          </p>

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
            <label>
              <input
                type="checkbox"
                onChange={() => setEnabledCheckBox((prev) => !prev)}
              ></input>
              Изменить пароль
            </label>
            {enabledCheckBox && (
              <Input
                id="password"
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            )}
          </div>

          <Button onClick={UpdatePlayer}>Обновить</Button>
        </Modal>
      )}
      <div className="Player-card">
        <div className="UserInfo">
          Ник: {Name} <br />
          Отряд: {SquadName}
        </div>

        <div className="Buttons">
          <Button
            onClick={async (event) => {
              navigate(`/player/${Id}`);
            }}
          >
            Профиль
          </Button>
          <Button onClick={() => setIsDeleteModalOpen(true)}>Удалить</Button>
        </div>
      </div>
    </>
  );
}
