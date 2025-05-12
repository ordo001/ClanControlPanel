import FormLogin from "../../Models/FormLogin/FormLogin";
import HeaderLogin from "../../Models/HeaderLogin/HeaderLogin";
import "./LoginPage.css";
export default function LoginPage() {
  return (
    <>
      <div className="form">
        <HeaderLogin />
        <FormLogin />
      </div>
    </>
  );
}
