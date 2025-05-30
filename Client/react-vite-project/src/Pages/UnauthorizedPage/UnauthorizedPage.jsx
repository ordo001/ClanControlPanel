import React from "react";
import { useNavigate } from "react-router-dom";
import "./UnauthorizedPage.css";

export const UnauthorizePage = () => {
  const navigate = useNavigate();

  return (
    <div className="unauth-container">
      <h1 className="unauth-message">Вы не имеете доступ на данную страницу</h1>
      <button className="unauth-button" onClick={() => navigate("/squads")}>
        На главную
      </button>
    </div>
  );
};
