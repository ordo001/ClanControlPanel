import { createContext, useContext, useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
const AuthContext = createContext();

// export function AuthProvider({ children }) {
//   const [isAuthenticated, setIsAuthenticated] = useState(false);
//   const [loading, setLoading] = useState(true);
//   const [user, setUser] = useState(null);
//   const apiUrl = import.meta.env.VITE_API_URL;

//   async function checkAuth() {
//     try {
//       const response = await fetch(`${apiUrl}/api/Auth/Validate-token`, {
//         method: "GET",
//         credentials: "include",
//       });

//       if (response.ok) {
//         const data = await response.json();
//         setIsAuthenticated(true);
//         setUser(data.user);
//         return data.user;
//       } else {
//         setIsAuthenticated(false);
//         setUser(null);
//       }
//     } catch (error) {
//       setIsAuthenticated(false);
//       setUser(null);
//     } finally {
//       setLoading(false);
//     }
//   }

//   useEffect(() => {
//     checkAuth();
//   }, [apiUrl]);

//   return (
//     <AuthContext.Provider
//       value={{ isAuthenticated, setIsAuthenticated, loading, user, checkAuth }}
//     >
//       {children}
//     </AuthContext.Provider>
//   );
// }

// export default function useAuth() {
//   return useContext(AuthContext);
// }

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  const apiUrl = import.meta.env.VITE_API_URL;

  const checkAuth = async () => {
    try {
      const response = await fetch(`${apiUrl}/api/Auth/Validate-token`, {
        method: "GET",
        credentials: "include",
      });

      if (response.ok) {
        const data = await response.json();
        setUser(data.user);
        setIsAuthenticated(true);
      } else {
        setUser(null);
        setIsAuthenticated(false);
      }
    } catch (error) {
      console.error("Ошибка при проверке авторизации:", error);
      setUser(null);
      setIsAuthenticated(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        setUser,
        isAuthenticated,
        setIsAuthenticated,
        loading,
        checkAuth,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export default function useAuth() {
  return useContext(AuthContext);
}
