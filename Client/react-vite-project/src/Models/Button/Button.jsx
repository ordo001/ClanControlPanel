import "./Button.css";

export default function Button({ disabled, onClick, children }) {
  //const [count, setCount] = useState(0)

  return (
    <button disabled={disabled} onClick={onClick}>
      {children}
    </button>
  );
}
