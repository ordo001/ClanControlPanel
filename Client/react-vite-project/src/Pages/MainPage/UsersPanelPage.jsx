import "./UsersPanelPage.css";
import { Navigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderUsersPage from "../../Models/HeaderUsersPage/HeaderUsersPage";
import UserCard from "../../Models/UserCard/UserCard";
import { HubConnectionBuilder } from "@microsoft/signalr";

const apiUrl = import.meta.env.VITE_API_URL;

export default function UsersPanelPage() {
  // const { isAuthenticated, loading } = useAuth();
  const [users, setUsers] = useState([]);
  const [connection, setConnection] = useState(null);

  const fetchUsers = useCallback(async () => {
    const response = await fetch(`${apiUrl}/api/Users`, {
      method: "GET",
      credentials: "include",
    });
    const users = await response.json();
    setUsers(users);
  }, [apiUrl]);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/userHub`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    newConnection
      .start()
      .then(() => console.log("SignalR Connected"))
      .catch((err) => console.error("SignalR Connection Error: ", err));

    return () => {
      newConnection.stop();
    };
  }, [apiUrl]);

  // Обработка события обновления пользователей
  useEffect(() => {
    if (connection) {
      connection.on("UsersUpdated", fetchUsers);
    }

    return () => {
      if (connection) {
        connection.off("UsersUpdated");
      }
    };
  }, [connection, fetchUsers]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  return (
    <div>
      <HeaderUsersPage onUpdateListPlayers={fetchUsers} />
      {users.length > 0 ? (
        users.map((user) => (
          <UserCard
            key={user.id}
            Name={user.name}
            Login={user.login}
            Id={user.id}
            onUpdateListUsers={fetchUsers}
          />
        ))
      ) : (
        <p style={{ textAlign: "center", fontSize: "24px", marginTop: "20px" }}>
          Пользователей нет
        </p>
      )}
    </div>
  );
}
