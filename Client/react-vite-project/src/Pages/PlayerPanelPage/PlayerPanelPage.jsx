import "./PlayerPanelPage.css";
import { Navigate, useNavigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderUsersPage from "../../Models/HeaderUsersPage/HeaderUsersPage";
import { HubConnectionBuilder } from "@microsoft/signalr";
import PlayerCard from "../../Models/PlayerCard/PlayerCard";
import HeaderPlayersPage from "../../Models/HeaderPlayersPage/HeaderPlayerPage";
import ComboBox from "../../Models/ComboBox/ComboBox";
import Modal from "../../Models/Modal/Modal";
import Input from "../../Models/Input/Input";
import Button from "../../Models/Button/Button";
import Header from "../../Models/Header/Header";

const apiUrl = import.meta.env.VITE_API_URL;

export default function PlayerPanelPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [players, setUsers] = useState([]);
  const [connection, setConnection] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedSquad, setSelectedSquad] = useState("Все");
  const [squads, setSquads] = useState([]);
  const [selectedId, setSelectedId] = useState(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [name, setName] = useState("");

  async function openModal() {
    setIsAddModalOpen(true);
    await getSquads();
  }

  function closeModal() {
    setIsAddModalOpen(false);
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
        closeModal();
        setIsAddModalOpen(false);
      }
      if (response.status >= 400) {
        const errorData = await response.json();
        const errorMessage = errorData.Message;
        alert(`Ошибка: ${errorMessage}`);
      }
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

  const fetchPlayers = useCallback(async () => {
    const response = await fetch(`${apiUrl}/api/Player`, {
      method: "GET",
      credentials: "include",
    });
    const users = await response.json();
    setUsers(users);
  }, [apiUrl]);

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

  useEffect(() => {
    if (connection) {
      connection.on("PlayersUpdated", fetchPlayers);
    }

    return () => {
      if (connection) {
        connection.off("PlayersUpdated");
      }
    };
  }, [connection, fetchPlayers]);

  useEffect(() => {
    fetchPlayers();
  }, [fetchPlayers]);

  const squadNames = [
    "Все",
    ...Array.from(new Set(players.map((p) => p.squadName).filter(Boolean))),
  ];

  const filteredPlayers = players.filter((player) => {
    const matchesName = player.name
      .toLowerCase()
      .includes(searchTerm.toLowerCase());
    const matchesSquad =
      selectedSquad === "Все" || player.squadName === selectedSquad;
    return matchesName && matchesSquad;
  });

  return (
    <div>
      <Header>
        {user.role === "Admin" && (
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
        )}
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
              navigate("/events");
            }}
          >
            Казна
          </button>
        </li>
      </Header>

      <div className="Info">
        Здесь отображается список всех участников клана
      </div>

      <div className="Info">Всего состава: {players.length}</div>

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
          placeholder="Поиск по нику..."
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

        <select
          value={selectedSquad}
          onChange={(e) => setSelectedSquad(e.target.value)}
          style={{
            padding: "8px 12px",
            fontSize: "16px",
            borderRadius: "6px",
            border: "1px solid #ccc",
          }}
        >
          {squadNames.map((name) => (
            <option key={name} value={name}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className="User-container">
        <div className="Add-user" onClick={openModal}>
          + Новый участник
        </div>
        {filteredPlayers.length > 0 ? (
          filteredPlayers.map((player) => (
            <PlayerCard
              key={player.id}
              Name={player.name}
              SquadName={player.squadName}
              SquadId={player.squadId}
              Id={player.id}
              onUpdateListPlayers={fetchPlayers}
            />
          ))
        ) : (
          <p
            style={{ textAlign: "center", fontSize: "24px", marginTop: "20px" }}
          >
            Игроков нет
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
    </div>
  );
}
