import { useNavigate } from "react-router-dom";
import useAuth from "./useAuth";

export function useLogout() {
  const navigate = useNavigate();
  const { setIsAuthenticated } = useAuth();

  return async function logout() {
    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/Auth/Logout`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        }
      );

      if (response.ok) {
        setIsAuthenticated(false);
        navigate("/");
      }
    } catch (error) {
      console.error("Нет связи с сервером.", error);
    }
  };
}
