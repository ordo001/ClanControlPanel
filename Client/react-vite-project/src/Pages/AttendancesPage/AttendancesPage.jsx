import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import Header from "../../Models/Header/Header";
import "./AttendancesPage.css";
import { HubConnectionBuilder } from "@microsoft/signalr";
import useAuth from "../../Func/useAuth";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";

export default function AttendancesPage() {
  const { user } = useAuth();
  const [connection, setConnection] = useState(null);
  const { eventId } = useParams();
  const navigate = useNavigate();
  const [players, setPlayers] = useState([]);
  const [eventData, setEventData] = useState(null);
  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    fetchData();
  }, [eventId]);

  const fetchData = async () => {
    try {
      const [playersRes, eventRes] = await Promise.all([
        axios.get(`${apiUrl}/api/Player`, { withCredentials: true }),
        axios.get(`${apiUrl}/api/Events/${eventId}/Attendances`, {
          withCredentials: true,
        }),
      ]);
      setPlayers(playersRes.data);
      setEventData(eventRes.data);
    } catch (error) {
      console.error("Ошибка при загрузке данных:", error);
    }
  };

  const getAttendanceStatus = (playerId) => {
    const record = eventData?.attendances.find((a) => a.playerId === playerId);
    return record ? record.attendance : null;
  };

  const updateAttendance = async (playerId, status) => {
    try {
      await axios.patch(
        `${apiUrl}/api/Event/${eventId}/attendances/${playerId}`,
        { status },
        { withCredentials: true }
      );

      setEventData((prev) => {
        const updated = prev.attendances.filter((a) => a.playerId !== playerId);
        return {
          ...prev,
          attendances: [...updated, { playerId, attendance: status }],
        };
      });
    } catch (err) {
      console.error("Ошибка при обновлении статуса:", err);
    }
  };

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/eventHub`)
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
      connection.on("AttendanceUpdated", fetchData);
    }

    return () => {
      if (connection) {
        connection.off("AttendanceUpdated");
      }
    };
  }, [connection]);

  const presentPlayers =
    eventData?.attendances
      .filter((a) => a.attendance === "Present")
      .sort((a, b) => {
        const nameA = a.playerName || "";
        const nameB = b.playerName || "";
        return nameA.localeCompare(nameB);
      }) || [];

  const otherPlayers =
    eventData?.attendances
      .filter((a) => a.attendance !== "Present")
      .sort((a, b) => {
        const nameA = a.playerName || "";
        const nameB = b.playerName || "";
        return nameA.localeCompare(nameB);
      }) || [];

  const onDragEnd = (result) => {
    const { source, destination, draggableId } = result;
    if (!destination || source.droppableId === destination.droppableId) return;

    const playerId = draggableId;
    const newStatus =
      destination.droppableId === "present" ? "Present" : "AbsentUnexcused";

    updateAttendance(playerId, newStatus);
  };

  const renderDraggablePlayer = (attendance, index) => (
    <Draggable
      draggableId={attendance.playerId}
      index={index}
      key={attendance.attendanceId}
    >
      {(provided) => (
        <li
          className="list-item"
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
        >
          <div className="attendance-card">
            <div className="player-info">
              <p className="player-attendance-name">{attendance.playerName}</p>
              <p className="player-squad">{attendance.squadName}</p>
            </div>

            {attendance.attendance !== "Present" && (
              <button
                className="toggle-status-btn"
                onClick={() => {
                  const newStatus =
                    attendance.attendance === "AbsentExcused"
                      ? "AbsentUnexcused"
                      : "AbsentExcused";
                  updateAttendance(attendance.playerId, newStatus);
                }}
              >
                {attendance.attendance === "AbsentExcused"
                  ? "✅ Предупредил"
                  : "⚠️ Без причины"}
              </button>
            )}
          </div>
        </li>
      )}
    </Draggable>
  );

  return (
    <>
      <Header>
        {user.role === "Admin" && (
          <li>
            <button
              className="nav-button"
              onClick={() => navigate("/user-panel")}
            >
              Аккаунты
            </button>
          </li>
        )}
        {(user.role === "Moder" || user.role === "Admin") && (
          <li>
            <button
              className="nav-button"
              onClick={() => navigate("/player-panel")}
            >
              Состав
            </button>
          </li>
        )}
        <li>
          <button className="nav-button" onClick={() => navigate("/squads")}>
            Отряды
          </button>
        </li>
        <li>
          <button className="nav-button" onClick={() => navigate("/events")}>
            Казна
          </button>
        </li>
      </Header>

      <div className="container-attendances">
        <h2 className="title">Посещаемость события</h2>
        {eventData && (
          <div className="event-meta">
            <p>
              <strong>Тип:</strong> {eventData.eventTypeName}
            </p>
            <p>
              <strong>Дата:</strong>{" "}
              {new Date(eventData.date).toLocaleDateString()}
            </p>
          </div>
        )}

        <DragDropContext onDragEnd={onDragEnd}>
          <div className="grid">
            <Droppable droppableId="present">
              {(provided) => (
                <div>
                  <h3 className="subtitle">✅ Пришли</h3>
                  <h4 className="subtitle-2">
                    {presentPlayers.length}/{players.length}
                  </h4>
                  <ul
                    className="list-1"
                    ref={provided.innerRef}
                    {...provided.droppableProps}
                  >
                    {presentPlayers.map(renderDraggablePlayer)}
                    {provided.placeholder}
                  </ul>
                </div>
              )}
            </Droppable>

            <Droppable droppableId="absent">
              {(provided) => (
                <div>
                  <h3 className="subtitle">🚫 Отсутствуют / Неотмечены</h3>
                  <h4 className="subtitle-2">
                    {otherPlayers.length}/{players.length}
                  </h4>
                  <ul
                    className="list-2"
                    ref={provided.innerRef}
                    {...provided.droppableProps}
                  >
                    {otherPlayers.map(renderDraggablePlayer)}
                    {provided.placeholder}
                  </ul>
                </div>
              )}
            </Droppable>
          </div>
        </DragDropContext>
      </div>
    </>
  );
}
