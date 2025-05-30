import React, { useEffect, useState } from "react";
import axios from "axios";
import EventItem from "../../Models/EventItem/EventItem";
import Header from "../../Models/Header/Header";
import { useNavigate } from "react-router-dom";

// export default function EventsPage() {
//   const [events, setEvents] = useState([]);
//   const [expandedEventId, setExpandedEventId] = useState(null);
//   const apiUrl = import.meta.env.VITE_API_URL;

//   useEffect(() => {
//     fetchEvents();
//   }, []);

//   const fetchEvents = async () => {
//     try {
//       const res = await axios.get(`${apiUrl}/api/Event`, {
//         withCredentials: true,
//       });
//       console.log("Ответ от API /api/Event:", res.data);
//       setEvents(Array.isArray(res.data) ? res.data : []);
//     } catch (error) {
//       console.error("Ошибка при загрузке событий:", error);
//       setEvents([]);
//     }
//   };

//   const toggleExpanded = (eventId) => {
//     setExpandedEventId(expandedEventId === eventId ? null : eventId);
//   };

//   const fetchStages = async (eventId) => {
//     try {
//       const res = await axios.get(`${apiUrl}/api/Event/${eventId}/stages`, {
//         withCredentials: true,
//       });
//       return res.data;
//     } catch (error) {
//       console.error("Ошибка при загрузке этапов:", error);
//       return [];
//     }
//   };

//   const getTotalAmount = (stages) => {
//     return stages.reduce((sum, stage) => sum + stage.amount, 0);
//   };

//   return (
//     <>
//       <Header />
//       <div>
//         <h2>Список событий</h2>
//         {events.map((event) => (
//           <EventItem
//             key={event.idEvent}
//             event={event}
//             expanded={expandedEventId === event.idEvent}
//             onToggle={toggleExpanded}
//             fetchStages={fetchStages}
//             getTotalAmount={getTotalAmount}
//           />
//         ))}
//       </div>
//     </>
//   );
// }

export default function EventsPage() {
  const [events, setEvents] = useState([]);
  const navigate = useNavigate();
  const [stagesMap, setStagesMap] = useState({});
  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    fetchEvents();
  }, []);

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

  const getTotalAmount = (stages) => {
    return stages.reduce((sum, s) => sum + s.amount, 0);
  };

  const getGlobalTotalAmount = () => {
    return Object.values(stagesMap)
      .flat()
      .reduce((sum, stage) => sum + stage.amount, 0);
  };

  return (
    <div>
      <Header>
        <li>
          <button
            className="nav-button"
            onClick={() => navigate("/user-panel")}
          >
            Аккаунты
          </button>
        </li>
        <li>
          <button className="nav-button" onClick={() => navigate("/squads")}>
            Отряды
          </button>
        </li>
      </Header>

      <div className="Info">Здесь отображаются все события</div>
      <div className="Info">
        Всего событий: {events.length} | Казна:{" "}
        {getGlobalTotalAmount().toLocaleString()}
      </div>

      <div className="User-container">
        <div className="Add-user">+ Новое событие</div>

        {events.length > 0 ? (
          events.map((event) => (
            <EventItem
              key={event.idEvent}
              event={event}
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
