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
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const apiUrl = import.meta.env.VITE_API_URL;

  async function confirmDelete() {
    const response = await fetch(`${apiUrl}/api/Users/${Id}`, {
      method: "DELETE",
      credentials: "include",
    });

    if (response.ok) {
      onUpdateListUsers();
    }
    setIsDeleteModalOpen(false);
  }

  async function UpdateUser() {
    const response = await fetch(`${apiUrl}/api/Users`, {
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

    if (response.ok) {
      onUpdateListUsers();
      closeModal();
    }
    if (response.status >= 400) {
      const errorData = await response.json();
      if (Array.isArray(errorData)) {
        const messages = errorData.map(
          (err) => err.errorMessage || "Неизвестная ошибка"
        );
        alert(`Ошибки:\n- ${messages.join("\n- ")}`);
      }
      // Если это объект с Message
      else if (errorData?.Message) {
        alert(`Ошибка: ${errorData.Message}`);
      }
      // Что-то ещё
      else {
        alert("Произошла неизвестная ошибка.");
      }
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
      {isDeleteModalOpen && (
        <Modal open={isDeleteModalOpen}>
          <img
            className="imageCloseModal"
            src="krest.png"
            alt="Закрыть"
            onClick={() => setIsDeleteModalOpen(false)}
          />
          <h3>Подтверждение удаления</h3>
          <p>Вы уверены, что хотите удалить пользователя {Name}?</p>
          <div style={{ display: "flex", gap: "10px" }}>
            <Button onClick={() => setIsDeleteModalOpen(false)}>Отмена</Button>
            <Button onClick={confirmDelete} style={{ background: "#ff4444" }}>
              Удалить
            </Button>
          </div>
        </Modal>
      )}

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
          Логин: {Login} <br />
          Имя: {Name}
        </div>

        <div className="Buttons">
          <Button onClick={() => openModal()}>Изменить</Button>
          <Button onClick={() => setIsDeleteModalOpen(true)}>Удалить</Button>
        </div>
      </div>
    </>
  );
}
