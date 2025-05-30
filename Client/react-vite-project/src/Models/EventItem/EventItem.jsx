import { useEffect, useState } from "react";
import "./EventItem.css";

export default function EventItem({ event, stages, totalAmount }) {
  const [expanded, setExpanded] = useState(false);

  const handleToggle = () => {
    setExpanded((prev) => !prev);
  };

  return (
    <div
      className="Event-card"
      onClick={handleToggle}
      style={{
        cursor: "pointer",
        flexDirection: "column",
        alignItems: "flex-start",
      }}
    >
      <div className="Event-main-info">
        <span className="Event-param">
          📅 {new Date(event.date).toLocaleDateString()} <br />
        </span>
        <span className="Event-param">
          Тип: {event.eventTypeName ?? "Без типа"} <br />
        </span>
        <span className="Event-param">
          {event.status != null ? (
            <>
              Этапов: {stages.length}
              <br />
            </>
          ) : null}
        </span>
        <span className="Event-param">
          Казна: {totalAmount >= 0 ? `+${totalAmount}` : totalAmount}
        </span>
      </div>

      {expanded && (
        <div style={{ paddingTop: "10px", width: "100%", fontSize: "18px" }}>
          {stages.length > 0 ? (
            stages.map((stage) => (
              <div key={stage.id} style={{ marginBottom: "8px" }}>
                {stage.stageNumber === null
                  ? ""
                  : "Этап " + stage.stageNumber + " |"}
                {"     Описание: " + stage.description || "—"} | Казна:{"   "}
                {stage.amount >= 0 ? `+${stage.amount}` : stage.amount}
              </div>
            ))
          ) : (
            <div>Этапов нет</div>
          )}
        </div>
      )}
    </div>
  );
}
