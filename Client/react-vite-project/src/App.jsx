import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

import "./App.css";
import MainPanel from "./Pages/MainPanelPage/MainPanelPage";
import LoginPage from "./Pages/LoginForm/LoginPage";
import useAuth from "./Func/useAuth";
import MainPage from "./Pages/MainPage/MainPage";

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
      <Router>
        <Routes>
          <Route
            path="/"
            element={!isAuthenticated ? <LoginPage /> : <MainPage />}
          />
        </Routes>
      </Router>
    </>
  );
}

export default App;
