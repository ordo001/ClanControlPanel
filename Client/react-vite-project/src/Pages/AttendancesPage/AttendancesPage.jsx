import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import Header from "../../Models/Header/Header";
import "./AttendancesPage.css";

export default function AttendancesPage() {
  const { eventId } = useParams();
  const [players, setPlayers] = useState([]);
  const [attendances, setAttendances] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    fetchData();
  }, [eventId]);

  const fetchData = async () => {
    try {
      const [playersRes, attendancesRes] = await Promise.all([
        axios.get(`${apiUrl}/api/Player`, { withCredentials: true }),
        axios.get(`${apiUrl}/api/Events/${eventId}/Attendances`, {
          withCredentials: true,
        }),
      ]);
      setPlayers(playersRes.data);
      setAttendances(attendancesRes.data);
    } catch (error) {
      console.error("Ошибка при загрузке данных:", error);
    }
  };

  const getAttendanceStatus = (playerId) => {
    const record = attendances.find((a) => a.playerId === playerId);
    return record ? record.attendance : null;
  };

  const updateAttendance = async (playerId, status) => {
    try {
      await axios.patch(
        `${apiUrl}/api/Event/${eventId}/attendances/${playerId}`,
        { status },
        { withCredentials: true }
      );
      setAttendances((prev) => {
        const updated = prev.filter((a) => a.playerId !== playerId);
        return [...updated, { playerId, attendance: status }];
      });
      setEditingId(null);
    } catch (err) {
      console.error("Ошибка при обновлении статуса:", err);
    }
  };

  const statusPriority = {
    Present: 0,
    AbsentExcused: 1,
    AbsentUnexcused: 2,
    null: 99,
  };

  const sortedPlayers = [...players].sort((a, b) => {
    const aStatus = getAttendanceStatus(a.id) ?? null;
    const bStatus = getAttendanceStatus(b.id) ?? null;
    return (statusPriority[aStatus] ?? 99) - (statusPriority[bStatus] ?? 99);
  });

  const getStatusLabel = (status) => {
    switch (status) {
      case "Present":
        return { label: "✅ Пришёл", className: "status-present" };
      case "AbsentUnexcused":
        return { label: "❌ Без причины", className: "status-unexcused" };
      case "AbsentExcused":
        return { label: "📩 Предупредил", className: "status-excused" };
      default:
        return { label: "❌ Без причины", className: "status-unexcused" };
    }
  };

  const statusOptions = [
    { value: "Present", label: "✅ Пришёл" },
    { value: "AbsentExcused", label: "📩 Предупредил" },
    { value: "AbsentUnexcused", label: "❌ Без причины" },
  ];

  return (
    <>
      <Header />
      <div className="container">
        <h2 className="title">Посещаемость события</h2>

        <ul className="list">
          {sortedPlayers.map((player) => {
            const status = getAttendanceStatus(player.id);
            const { label, className } = getStatusLabel(status);
            const isEditing = editingId === player.id;

            return (
              <li key={player.id} className="list-item">
                <div>
                  <p className="player-name">{player.name}</p>
                  <p className="player-squad">{player.squadName}</p>
                </div>

                <div className="status">
                  {isEditing ? (
                    <select
                      autoFocus
                      value={status || ""}
                      onChange={(e) =>
                        updateAttendance(player.id, e.target.value)
                      }
                      onBlur={() => setEditingId(null)}
                    >
                      <option value="">—</option>
                      {statusOptions.map((opt) => (
                        <option key={opt.value} value={opt.value}>
                          {opt.label}
                        </option>
                      ))}
                    </select>
                  ) : (
                    <span
                      className={className}
                      style={{ cursor: "pointer" }}
                      onClick={() => setEditingId(player.id)}
                    >
                      {label}
                    </span>
                  )}
                </div>
              </li>
            );
          })}
        </ul>
      </div>
    </>
  );
}
