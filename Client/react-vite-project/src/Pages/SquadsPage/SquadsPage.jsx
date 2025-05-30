import HeaderSquadsPage from "../../Models/HeaderSquadsPage/HeaderSquadsPage";
import SquadColumn from "../../Models/SquadColumn/SquadColumn";
import { DragDropContext } from "react-beautiful-dnd";
import { HubConnectionBuilder } from "@microsoft/signalr";
import "./SquadsPage.css";
import { useEffect, useState } from "react";
import Header from "../../Models/Header/Header";
import { useNavigate } from "react-router-dom";
import { useLogout } from "../../Func/Logout";
import setIsAuthenticated from "../../Func/useAuth";

export default function SquadsPage() {
  const apiUrl = import.meta.env.VITE_API_URL;
  const [squads, setSquads] = useState([]);
  const navigate = useNavigate();
  const [draggingFromId, setDraggingFromId] = useState(null);
  const [connection, setConnection] = useState(null);
  const logout = useLogout();

  const fetchSquads = () => {
    fetch(`${apiUrl}/api/Squad`, {
      credentials: "include",
    })
      .then((res) => res.json())
      .then((data) => {
        const filledSquads = data.map((squad) => {
          const sortedPlayers = squad.players
            .filter((p) => p !== null)
            .sort((a, b) => a.position - b.position);

          return {
            ...squad,
            players: sortedPlayers,
          };
        });
        setSquads(filledSquads);
      })
      .catch((err) => console.error("Ошибка при загрузке отрядов:", err));
  };

  useEffect(() => {
    fetchSquads();
  }, []);

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
      connection.on("PlayersUpdated", fetchSquads);
    }

    return () => {
      if (connection) {
        connection.off("PlayersUpdated");
      }
    };
  }, [connection, fetchSquads]);

  const handleDragStart = (start) => {
    setDraggingFromId(start.source.droppableId);
  };

  const handleDragEnd = (result) => {
    setDraggingFromId(null);
    const { source, destination } = result;
    if (!destination) return;

    const sourceSquad = squads.find((s) => s.id === source.droppableId);
    const destSquad = squads.find((s) => s.id === destination.droppableId);
    const movedPlayer = sourceSquad?.players[source.index];

    if (!movedPlayer) return;

    const isSameSquad = source.droppableId === destination.droppableId;

    const destPlayersCount = destSquad.players.filter(Boolean).length;

    if (!isSameSquad && destPlayersCount >= 5) {
      return;
    }

    fetch(
      `${apiUrl}/api/Squads/${destination.droppableId}/Players/${movedPlayer.id}?position=${destination.index}`,
      {
        method: "POST",
        credentials: "include",
      }
    )
      .then((res) => {
        if (!res.ok) {
          throw new Error("Ошибка при обновлении позиции игрока");
        }
        return res.json().catch(() => ({}));
      })
      .then(() => {
        fetchSquads();
      })
      .catch((err) => {
        console.error("Ошибка при обновлении:", err);
      });
  };

  const fullSquadIds = squads
    .filter((s) => s.players.filter(Boolean).length >= 5)
    .map((s) => s.id);

  return (
    <>
      {/* <HeaderSquadsPage /> */}
      <Header>
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
      <div className="squads-container">
        <DragDropContext
          onDragEnd={handleDragEnd}
          onDragStart={handleDragStart}
        >
          {squads.map((squad) => (
            <SquadColumn
              key={squad.id}
              squad={squad}
              isFull={fullSquadIds.includes(squad.id)}
              isDropDisabled={
                draggingFromId !== squad.id && fullSquadIds.includes(squad.id)
              }
            />
          ))}
        </DragDropContext>
      </div>
    </>
  );
}
