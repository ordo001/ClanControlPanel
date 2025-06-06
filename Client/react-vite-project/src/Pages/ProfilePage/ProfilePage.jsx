import { useParams } from "react-router-dom";
import "./ProfilePage.css";
import Header from "../../Models/Header/Header";

const apiUrl = import.meta.env.VITE_API_URL;

export default function ProfilePage() {
  const { userId } = useParams();
  return (
    <>
      <Header></Header>
      <img src="https://github.com/EXBO-Studio/stalcraft-database/raw/main/ru/icons/weapon/pistol/y32z0.png" />
      <img src="https://github.com/EXBO-Studio/stalcraft-database/raw/main/ru/icons/armor/combat/0rn7d.png" />
      <div className="profile-container">
        <div>aboba</div>
      </div>
    </>
  );
}
