import { useParams } from "react-router-dom";
import "./ProfilePage.css";

const apiUrl = import.meta.env.VITE_API_URL;

export default function ProfilePage() {
  const { userId } = useParams();
  return <>{userId}</>;
}
