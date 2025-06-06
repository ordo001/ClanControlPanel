import React from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import useAuth from "./Func/useAuth";

import PlayerPanelPage from "./Pages/PlayerPanelPage/PlayerPanelPage";
import EventsPage from "./Pages/EventsPage/EventsPage";
import { ProtectedRoute } from "./Models/ProtectedRoute";
import UsersPanelPage from "./Pages/MainPage/UsersPanelPage";
import LoginPage from "./Pages/LoginForm/LoginPage";
import SquadsPage from "./Pages/SquadsPage/SquadsPage";
import ProfilePage from "./Pages/ProfilePage/ProfilePage";
import { UnauthorizePage } from "./Pages/UnauthorizedPage/UnauthorizedPage";
import AttendancesPage from "./Pages/AttendancesPage/AttendancesPage";

function App() {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div className="loading-container">
        <div className="loading-content">
          <div className="loading-spinner"></div>
          <p className="loading-text">Загрузка...</p>
        </div>
      </div>
    );
  }

  return (
    <>
      {/* <Router>
        <Routes>
          <Route
            path="/"
            element={!isAuthenticated ? <LoginPage /> : <UsersPanelPage />}
          />
        </Routes>
      </Router> */}
      <Router>
        <Routes>
          {/* Страница логина*/}
          <Route path="/login" element={<LoginPage />} />

          <Route
            path="/"
            element={
              isAuthenticated ? (
                <Navigate to="/squads" replace />
              ) : (
                <Navigate to="/login" replace />
              )
            }
          />

          {/* Общий доступ для всех авторизованных */}
          <Route
            path="/squads"
            element={
              <ProtectedRoute allowedRoles={["Member", "Moder", "Admin"]}>
                <SquadsPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/player/:userId"
            element={
              <ProtectedRoute
                allowedRoles={["User", "Member", "Moder", "Admin"]}
              >
                <ProfilePage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/events"
            element={
              <ProtectedRoute allowedRoles={["Member", "Moder", "Admin"]}>
                <EventsPage />
              </ProtectedRoute>
            }
          />

          <Route
            path="/events/:eventId/attendances"
            element={
              <ProtectedRoute allowedRoles={["Member", "Moder", "Admin"]}>
                <AttendancesPage />
              </ProtectedRoute>
            }
          />

          {/* Модераторы и выше */}
          <Route
            path="/player-panel"
            element={
              <ProtectedRoute allowedRoles={["Moder", "Admin"]}>
                <PlayerPanelPage />
              </ProtectedRoute>
            }
          />

          {/* Только админы */}
          <Route
            path="/user-panel"
            element={
              <ProtectedRoute allowedRoles={"Admin"}>
                <UsersPanelPage />
              </ProtectedRoute>
            }
          />
          {/* Страница по умолчанию */}
          <Route path="*" element={<Navigate to="/squads" replace />} />

          {/* Страница для не авторизованный чувачков*/}
          <Route path="/unauthorized" element={<UnauthorizePage />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
