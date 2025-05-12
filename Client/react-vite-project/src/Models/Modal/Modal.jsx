import { createPortal } from "react-dom";
import "./Modal.css";

export default function Modal({ children, open }) {
  return createPortal(
    <dialog open={open}>{children}</dialog>,
    document.getElementById("modal")
  );
}
