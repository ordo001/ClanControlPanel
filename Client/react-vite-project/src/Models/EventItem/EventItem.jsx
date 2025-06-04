import { useEffect, useState } from "react";
import "./EventItem.css";
import Button from "../Button/Button";
import { useNavigate } from "react-router-dom";

export default function EventItem({ event, stages, totalAmount }) {
  const [expanded, setExpanded] = useState(false);
  const navigate = useNavigate();

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
        <div>
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
            Казна:{" "}
            {totalAmount >= 0
              ? `+${totalAmount.toLocaleString("ru-RU")}`
              : totalAmount.toLocaleString("ru-RU")}
          </span>
        </div>
        {(event.eventTypeName === "Потасовка" ||
          event.eventTypeName === "Турнир") && (
          <div className="Buttons">
            <Button
              onClick={() => navigate(`/events/${event.idEvent}/attendances`)}
            >
              Посещаемость
            </Button>
          </div>
        )}
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
                {stage.amount >= 0
                  ? `+${stage.amount.toLocaleString("ru-RU")}`
                  : stage.amount.toLocaleString("ru-RU")}
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
