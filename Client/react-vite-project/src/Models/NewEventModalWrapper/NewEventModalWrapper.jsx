import { createPortal } from "react-dom";

export default function NewEventModalWrapper({ children, open }) {
  if (!open) return null;

  return createPortal(
    <dialog
      open={open}
      style={
        {
          // width: "1100px",
        }
      }
    >
      {children}
    </dialog>,
    document.getElementById("modal")
  );
}
