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

  return (
    // <>
    //   <Router>
    //     <Routes>
    //       <Route
    //         path="/"
    //         element={
    //           !isAuthenticated ? <LoginPage /> : <Navigate to="/MainPanel" />
    //           //<LoginPage />
    //         }
    //       />
    //       {/* <Route path="/" element={<MainPage />} /> */}
    //       <Route
    //         path="/MainPanel"
    //         element={
    //           isAuthenticated ? <MainPanel /> : <Navigate to="/" />
    //           //<MainPanel />
    //         }
    //       />
    //     </Routes>
    //   </Router>
    // </>
    <>
      <Router>
        <Routes>
          <Route path="/" element={<MainPage />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
