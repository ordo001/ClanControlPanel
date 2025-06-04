import PlayerCardSquad from "../PlayerCardSquad/PlayerCardSquad";
import { Droppable } from "react-beautiful-dnd";
import "./SquadColumn.css";

export default function SquadColumn({ squad, isFull, isDropDisabled }) {
  const totalSlots = 5;
  const players = squad.players.filter(Boolean);
  const slots = [...players];
  while (slots.length < totalSlots) {
    slots.push(null);
  }

  return (
    <div className={`squad-column ${isFull ? "squad-full" : ""}`}>
      <h3 className="squad-title">{squad.nameSquad}</h3>

      <Droppable droppableId={squad.id} isDropDisabled={isDropDisabled}>
        {(provided, snapshot) => (
          <div
            className={`player-list ${
              snapshot.isDraggingOver ? "dragging-over" : ""
            } ${isDropDisabled ? "no-drop" : ""}`}
            ref={provided.innerRef}
            {...provided.droppableProps}
          >
            {slots.map((player, index) =>
              player ? (
                <PlayerCardSquad
                  key={player.id || `empty-${index}`}
                  player={player}
                  index={index}
                  isFirst={index === 0 && player} // передаем флаг для первого игрока
                />
              ) : (
                <div key={`empty-${index}`} className="empty-slot">
                  {"Пусто"}
                </div>
              )
            )}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </div>
  );
}
