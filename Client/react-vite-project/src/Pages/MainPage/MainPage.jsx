import "./MainPage.css";
import { Navigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import useAuth from "../../Func/useAuth";
import HeaderMain from "../../Models/HeaderMain/HeaderMain";
import UserCard from "../../Models/UserCard/UserCard";

const apiUrl = import.meta.env.VITE_API_URL;

export default function MainPage() {
  // const { isAuthenticated, loading } = useAuth();
  const [users, setUsers] = useState([]);

  const fetchUsers = useCallback(async () => {
    // const response = await fetch("https://localhost:44307/api/User", {
    const response = await fetch(`${apiUrl}/api/User`, {
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
            key={user.idUser}
            Name={user.name}
            Login={user.login}
            Id={user.idUser}
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
