import React from "react";
import "./ComboBox.css";

const ComboBox = ({ options, onChange, selectedId }) => {
  const handleChange = (event) => {
    const selectedId = event.target.value === "" ? null : event.target.value;
    onChange(selectedId);
  };

  return (
    <select
      id="comboBox"
      name="comboBox"
      value={selectedId || ""}
      onChange={handleChange}
      className="comboBox"
    >
      <option value="" disabled className="disabledOption">
        Выберите отряд
      </option>
      <option key="" value="" className="option">
        Без отряда
      </option>
      {options.map((option) => (
        <option key={option.id} value={option.id} className="option">
          {option.name}
        </option>
      ))}
    </select>
  );
};

export default ComboBox;
