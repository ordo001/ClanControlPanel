export default function Input(props, handelChange) {
  return (
    <input
      type={props.type}
      placeholder={props.placeholder}
      onChange={props.onChange}
    ></input>
  );
}
