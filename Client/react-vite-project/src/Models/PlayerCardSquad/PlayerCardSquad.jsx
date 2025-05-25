import "./PlayerCardSquad.css";
import { Draggable } from "react-beautiful-dnd";
import korona from "../../assets/free-icon-star-1163655.png";

export default function PlayerCardSquad({ player, index, isFirst }) {
  if (!player) {
    return <div className="player-card empty-slot" />;
  }

  return (
    <Draggable draggableId={player.id} index={index}>
      {(provided) => (
        <div
          className="player-card"
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
        >
          <div className="player-name">{player.name}</div>
          {isFirst && (
            <img src={korona} alt="Корона" className="player-crown" />
          )}
        </div>
      )}
    </Draggable>
  );
}
