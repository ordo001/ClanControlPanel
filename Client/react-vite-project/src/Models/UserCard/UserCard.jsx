import Button from "../Button/Button";
import "./UserCard.css";
import Modal from "../Modal/Modal";
import { useState } from "react";
import Input from "../Input/Input";

export default function UserCard({ Name, Login, Id, onUpdateListUsers }) {
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [name, setName] = useState("");
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [enabledCheckBox, setEnabledCheckBox] = useState(false);

  async function DeleteUser() {
    const response = await fetch(`http://localhost:5000/api/User/${Id}`, {
      method: "DELETE",
      credentials: "include",
    });

    if (response.ok) {
      console.log("збс");
      onUpdateListUsers();
    } else {
      console.log("анлак");
    }
  }

  async function UpdateUser() {
    const response = await fetch(`http://localhost:5000/api/User/update`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        Id: Id,
        Login: login,
        Password: password,
        Name: name,
      }),
      credentials: "include",
    });

    console.log(
      JSON.stringify({
        Id: Id,
        Login: login,
        Password: password,
        Name: name,
      })
    );

    if (response.ok) {
      onUpdateListUsers();
      closeModal();
    }
  }

  function openModal() {
    setIsAddModalOpen(true);
  }

  function closeModal() {
    setIsAddModalOpen(false);
  }

  return (
    <>
      {isAddModalOpen && (
        <Modal open={isAddModalOpen}>
          <img
            className="imageCloseModal"
            src="krest.png"
            alt="Закрыть"
            onClick={closeModal}
          />
          <h3>Изменить данные пользователя</h3>
          <p>
            Измените поля ниже <br></br>
            (Оставьте строки пустыми, если хотите оставить поля без изменений)
          </p>

          <div className="form-group">
            <Input
              id="login"
              type="text"
              placeholder="Логин"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
            />
            <Input
              id="name"
              type="name"
              placeholder="Имя"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <label>
              <input
                type="checkbox"
                onChange={() => setEnabledCheckBox((prev) => !prev)}
              ></input>
              Изменить пароль
            </label>
            {enabledCheckBox && (
              <Input
                id="password"
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            )}
          </div>

          <Button onClick={UpdateUser}>Обновить</Button>
        </Modal>
      )}
      <div className="Card">
        <div className="UserInfo">
          Имя: {Name} <br />
          Логин: {Login}
        </div>
        <div className="Buttons">
          <Button onClick={() => openModal()}>Изменить</Button>
          <Button onClick={() => DeleteUser()}>Удалить</Button>
        </div>
      </div>
    </>
  );
}
