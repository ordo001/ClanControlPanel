import Input from "../Input/Input";
import Button from "../Button/Button";
import Modal from "../Modal/Modal";
import { useEffect, useState } from "react";
import "./FormLogin.css";
import { useNavigate } from "react-router-dom";
import useAuth from "../../Func/useAuth";

export default function FormLogin() {
  const navigate = useNavigate();
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [modalNotFound, setModalNotFound] = useState(false);
  const [modalErrorServer, setModalErrorServer] = useState(false);
  const { setIsAuthenticated } = useAuth();

  const MainPage = () => navigate("/MainPanel");

  async function fetchAuth() {
    try {
      const response = await fetch("http://localhost:5000/api/Auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ login: login, password: password }),
        credentials: "include",
      });

      if (response.ok) {
        //console.log("aboba");
        setIsAuthenticated(true);
        navigate("/MainPanel");
      }

      if (response.status === 404) {
        setModalNotFound(true);
      }

      setModalErrorServer(false);
    } catch (error) {
      console.log("Нет связи с сервером.", error);
      setModalErrorServer(true);
    }
  }

  // useEffect(() => {
  //   fetchAuth();
  // }, []);

  return (
    <>
      <Modal open={modalNotFound}>
        <h3>Неверное имя пользователя или пароль</h3>
        Повторите попытку ещё раз <br />
        <Button
          onClick={() => {
            setModalNotFound(false);
          }}
        >
          Закрыть
        </Button>
      </Modal>

      <Modal open={modalErrorServer}>
        <h3>Ошибка соединения с сервеом</h3>
        Повторите попытку ещё раз <br />
        <Button
          onClick={() => {
            setModalErrorServer(false);
          }}
        >
          Закрыть
        </Button>
      </Modal>
      <div className="form-group">
        <Input
          id="login"
          type="text"
          placeholder="Логин"
          value={login}
          onChange={(event) => setLogin(event.target.value)}
        />
      </div>
      <div>
        <Input
          id="password"
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(event) => setPassword(event.target.value)}
        />
      </div>
      <Button
        disabled={!(password.length > 2 && login.length > 2)}
        onClick={fetchAuth}
      >
        Вход
      </Button>
    </>
  );
}
