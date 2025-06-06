import React, { useEffect, useState } from "react";
import axios from "axios";
import EventItem from "../../Models/EventItem/EventItem";
import Header from "../../Models/Header/Header";
import NewEventModal from "../../Models/NewEventModal/NewEventModal";
import { useNavigate } from "react-router-dom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import useAuth from "../../Func/useAuth";

export default function EventsPage() {
  const { user } = useAuth();
  const [connection, setConnection] = useState(null);
  const [events, setEvents] = useState([]);
  const [filteredEvents, setFilteredEvents] = useState([]);
  const [stagesMap, setStagesMap] = useState({});
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [dateFilter, setDateFilter] = useState("");
  const [typeFilter, setTypeFilter] = useState("");
  const navigate = useNavigate();
  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    fetchEvents();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [events, dateFilter, typeFilter]);

  const fetchEvents = async () => {
    try {
      const res = await axios.get(`${apiUrl}/api/Event`, {
        withCredentials: true,
      });
      const data = Array.isArray(res.data) ? res.data : [];

      const stagesData = await Promise.all(
        data.map((event) =>
          axios
            .get(`${apiUrl}/api/Event/${event.idEvent}/stages`, {
              withCredentials: true,
            })
            .then((res) => ({ eventId: event.idEvent, stages: res.data }))
        )
      );

      const stagesByEvent = {};
      stagesData.forEach(({ eventId, stages }) => {
        stagesByEvent[eventId] = stages;
      });

      setEvents(data);
      setStagesMap(stagesByEvent);
    } catch (error) {
      console.error("Ошибка при загрузке событий:", error);
      setEvents([]);
    }
  };

  const applyFilters = () => {
    const filtered = events.filter((event) => {
      const matchDate = dateFilter ? event.date?.startsWith(dateFilter) : true;
      const matchType = typeFilter ? event.eventTypeName === typeFilter : true;
      return matchDate && matchType;
    });
    setFilteredEvents(filtered);
  };

  const getTotalAmount = (stages) =>
    stages.reduce((sum, s) => sum + s.amount, 0);

  const getGlobalTotalAmount = () =>
    Object.values(stagesMap)
      .flat()
      .reduce((sum, s) => sum + s.amount, 0);

  const flatStages = Object.values(stagesMap).flat();

  const getStageCountByDescription = (desc) =>
    flatStages.filter((s) => s.description === desc).length;

  const getTournamentAndBrawlStagesCount = () =>
    events
      .filter((e) => ["Турнир", "Потасовка"].includes(e.eventTypeName))
      .reduce((sum, e) => sum + (stagesMap[e.idEvent]?.length || 0), 0);

  const uniqueEventTypes = [...new Set(events.map((e) => e.eventTypeName))];

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
      connection.on("EventUpdated", fetchEvents);
    }

    return () => {
      if (connection) {
        connection.off("EventUpdated");
      }
    };
  }, [connection, fetchEvents]);

  return (
    <div>
      <NewEventModal
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={fetchEvents}
      />

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
      </Header>

      <div className="Info">Здесь отображаются всё движение казны</div>

      <div className="Info" style={{ fontSize: "18px", lineHeight: "1.6em" }}>
        <strong>Всего событий:</strong> {events.length} <br />
        <strong>Казна:</strong> {getGlobalTotalAmount().toLocaleString()} <br />
        {/* <strong>Победы:</strong> {getStageCountByDescription("Победа")} |{" "}
        <strong>Поражения:</strong> {getStageCountByDescription("Поражение")}{" "} */}
        <br />
        <strong>Продались:</strong> {getStageCountByDescription("Продались")} |{" "}
        <strong>Купили:</strong> {getStageCountByDescription("Купили")} <br />
        <strong>Этапов в турнирах и потасовках:</strong>{" "}
        {getTournamentAndBrawlStagesCount()}
      </div>

      {/* ФИЛЬТРЫ */}
      <div
        style={{
          display: "flex",
          gap: "1rem",
          alignItems: "center",
          justifyContent: "center",
          margin: "20px 0",
          flexWrap: "wrap",
        }}
      >
        <input
          type="date"
          value={dateFilter}
          onChange={(e) => setDateFilter(e.target.value)}
          style={{ padding: "6px", fontSize: "16px" }}
        />
        <select
          value={typeFilter}
          onChange={(e) => setTypeFilter(e.target.value)}
          style={{ padding: "6px", fontSize: "16px" }}
        >
          <option value="">Все типы</option>
          {uniqueEventTypes.map((type) => (
            <option key={type}>{type}</option>
          ))}
        </select>
      </div>

      <div className="User-container">
        <div className="Add-user" onClick={() => setIsModalOpen(true)}>
          + Новое событие
        </div>

        {filteredEvents.length > 0 ? (
          filteredEvents.map((event) => (
            <EventItem
              key={event.idEvent}
              event={event}
              fetchEvents={fetchEvents}
              stages={stagesMap[event.idEvent] || []}
              totalAmount={getTotalAmount(stagesMap[event.idEvent] || [])}
            />
          ))
        ) : (
          <p
            style={{ textAlign: "center", fontSize: "24px", marginTop: "20px" }}
          >
            Событий нет
          </p>
        )}
      </div>
    </div>
  );
}
