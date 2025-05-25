import "./PlayerPanelPage.css";
import { Navigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderUsersPage from "../../Models/HeaderUsersPage/HeaderUsersPage";
import { HubConnectionBuilder } from "@microsoft/signalr";
import PlayerCard from "../../Models/PlayerCard/PlayerCard";
import HeaderPlayersPage from "../../Models/HeaderPlayersPage/HeaderPlayerPage";

const apiUrl = import.meta.env.VITE_API_URL;

export default function PlayerPanelPage() {
  // const { isAuthenticated, loading } = useAuth();
  const [players, setUsers] = useState([]);
  const [connection, setConnection] = useState(null);

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

  return (
    <div>
      <HeaderPlayersPage onUpdateListPlayers={fetchPlayers} />
      <div className="Info">
        Здесь отображается список всех участников клана
      </div>

      <div className="Info">Всего состава: {players.length}</div>
      <div className="User-container">
        {players.length > 0 ? (
          players.map((player) => (
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
    </div>
  );
}
