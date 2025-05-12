import { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  async function checkAuth() {
    try {
      const response = await fetch(
        "http://localhost:5000/api/Auth/validateToke",
        {
          method: "GET",
          credentials: "include",
        }
      );

      setIsAuthenticated(response.ok);
    } catch (error) {
      setIsAuthenticated(false);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    checkAuth();
  }, []);

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, setIsAuthenticated, loading }}
    >
      {children}
    </AuthContext.Provider>
  );
}

// Хук для использования авторизации
export default function useAuth() {
  return useContext(AuthContext);
}
