import "./UsersPanelPage.css";
import { Navigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderMain from "../../Models/HeaderMain/HeaderMain";
import UserCard from "../../Models/UserCard/UserCard";

const apiUrl = import.meta.env.VITE_API_URL;

export default function UsersPanelPage() {
  // const { isAuthenticated, loading } = useAuth();
  const [users, setUsers] = useState([]);

  const fetchUsers = useCallback(async () => {
    const response = await fetch(`${apiUrl}/api/Users`, {
      method: "GET",
      credentials: "include",
    });
    const users = await response.json();
    setUsers(users);
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  return (
    <div>
      <HeaderMain onUpdateListUsers={fetchUsers} />
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
