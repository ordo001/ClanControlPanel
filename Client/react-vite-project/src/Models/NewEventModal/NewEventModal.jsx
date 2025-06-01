import React, { useEffect, useState } from "react";
import axios from "axios";
import Button from "../Button/Button";
import "./NewEventModal.css";
import NewEventModalWrapper from "../NewEventModalWrapper/NewEventModalWrapper";

export default function NewEventModal({ open, onClose, onSuccess }) {
  const [eventTypes, setEventTypes] = useState([]);
  const [stagesCount, setStagesCount] = useState(0);
  const [date, setDate] = useState("");
  const [eventTypeId, setEventTypeId] = useState("");
  const [eventTypeName, setEventTypeName] = useState("");
  const [status, setStatus] = useState(0);
  const [goldAmount, setGoldAmount] = useState("");
  const [stages, setStages] = useState([]);

  const apiUrl = import.meta.env.VITE_API_URL;

  useEffect(() => {
    axios
      .get(`${apiUrl}/api/Event/Types`, { withCredentials: true })
      .then((res) => setEventTypes(res.data))
      .catch(console.error);
  }, []);

  useEffect(() => {
    if (!open) {
      resetForm();
    } else {
      setDate(new Date().toISOString().slice(0, 10));
    }
  }, [open]);

  const resetForm = () => {
    setDate("");
    setEventTypeId("");
    setEventTypeName("");
    setStatus(0);
    setGoldAmount("");
    setStages([]);
  };

  const handleAddStage = () => {
    setStages((prev) => [...prev, { description: "", amount: "" }]);
  };

  const handleChangeStage = (index, key, value) => {
    setStages((prev) => {
      const updated = [...prev];
      updated[index][key] = value;
      return updated;
    });
  };

  const handleDeleteStage = (index) => {
    setStages((prev) => prev.filter((_, i) => i !== index));
  };

  const parseAmount = (value) => {
    if (!value) return 0;
    return parseFloat(String(value).replace(/\s/g, "").replace(",", ".")) || 0;
  };

  const handleSubmit = async () => {
    try {
      if (!eventTypeId) {
        alert("Выберите тип события");
        return;
      }
      const selectedType = eventTypes.find((et) => et.id === eventTypeId);
      const isGold = selectedType?.eventTypeName === "Голд";

      const eventRes = await axios.post(
        `${apiUrl}/api/Event`,
        {
          date: new Date(date).toISOString(),
          eventTypeId,
          status: isGold ? null : parseInt(status),
        },
        { withCredentials: true }
      );

      //const response = eventRes.json();

      //const eventId = response.data.idEvent;
      const eventId = eventRes.data;

      if (isGold) {
        await axios.post(
          `${apiUrl}/api/Event/${eventId}/stages`,
          {
            stageNumber: null,
            amount: parseAmount(goldAmount),
            description: "Вынесли",
          },
          { withCredentials: true }
        );
      } else {
        for (let i = 0; i < stages.length; i++) {
          const stage = stages[i];
          await axios.post(
            `${apiUrl}/api/Event/${eventId}/stages`,
            {
              stageNumber: i + 1,
              amount: parseAmount(stage.amount),
              description: stage.description,
            },
            { withCredentials: true }
          );
        }
      }

      onSuccess();
      onClose();
    } catch (err) {
      console.error("Ошибка при добавлении события:", err);
      alert("Ошибка при добавлении события");
    }
  };

  useEffect(() => {
    setStages((prevStages) => {
      const count = stagesCount;
      if (count === prevStages.length) return prevStages;

      if (count > prevStages.length) {
        // Добавляем пустые этапы
        const newStages = [...prevStages];
        for (let i = prevStages.length; i < count; i++) {
          newStages.push({ description: "", amount: "" });
        }
        return newStages;
      } else {
        // Урезаем массив
        return prevStages.slice(0, count);
      }
    });
  }, [stagesCount]);

  if (!open) return null;

  return (
    <>
      <div className="modal-overlay" onClick={onClose} />
      <div className="modal-container" onClick={(e) => e.stopPropagation()}>
        <img
          className="imageCloseModal"
          src="/krest.png"
          alt="Закрыть"
          onClick={onClose}
        />
        <h2 style={{ marginTop: 0 }}>Новое событие</h2>

        <div className="form-group">
          <label>
            Дата:
            <input
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </label>

          <label>
            Тип события:
            <select
              value={eventTypeId}
              onChange={(e) => {
                const selectedId = e.target.value;
                setEventTypeId(selectedId);
                const selected = eventTypes.find((t) => t.id === selectedId);
                setEventTypeName(selected?.eventTypeName || "");
              }}
            >
              <option value="">-- Выберите тип --</option>
              {eventTypes.map((type) => (
                <option key={type.id} value={type.id}>
                  {type.eventTypeName}
                </option>
              ))}
            </select>
          </label>

          {eventTypeName !== "Голд" && (
            <>
              <label>
                Количество этапов:
                <input
                  type="number"
                  min={0}
                  max={20}
                  value={stagesCount}
                  onChange={(e) => {
                    const val = Math.max(
                      0,
                      Math.min(20, Number(e.target.value))
                    );
                    setStagesCount(val);
                  }}
                />
              </label>

              {/* Можно убрать кнопку + Добавить этап, т.к. этапы создаются автоматически */}
              {/* <div style={{ margin: "10px 0" }}>
      <Button onClick={handleAddStage}>+ Добавить этап</Button>
    </div> */}

              <div className="stages-wrapper">
                {stages.length === 0 && (
                  <p className="empty-stages-message">Этапы не добавлены</p>
                )}
                {stages.map((stage, index) => (
                  <div key={index} className="stage-card">
                    <label>
                      <span className="label-text">Описание:</span>
                      <input
                        type="text"
                        value={stage.description}
                        onChange={(e) =>
                          handleChangeStage(
                            index,
                            "description",
                            e.target.value
                          )
                        }
                      />
                    </label>

                    <label>
                      <span className="label-text">Казна:</span>
                      <input
                        type="text"
                        value={stage.amount}
                        onChange={(e) =>
                          handleChangeStage(index, "amount", e.target.value)
                        }
                        onBlur={(e) =>
                          handleChangeStage(
                            index,
                            "amount",
                            parseAmount(e.target.value).toLocaleString("ru-RU")
                          )
                        }
                      />
                    </label>

                    <Button
                      onClick={() => {
                        handleDeleteStage(index);
                        setStagesCount((prev) => prev - 1); // уменьшаем счетчик
                      }}
                      color="red"
                      className="delete-stage-button"
                    >
                      Удалить этап
                    </Button>
                  </div>
                ))}
              </div>
            </>
          )}

          {/* {eventTypeName !== "Голд" ? (
            <>
              <label>
                Результат (0–4):
                <input
                  className="result-input"
                  type="number"
                  min={0}
                  max={4}
                  value={status}
                  onChange={(e) => setStatus(e.target.value)}
                />
              </label>

              <div style={{ margin: "10px 0" }}>
                <Button onClick={handleAddStage}>+ Добавить этап</Button>
              </div>

              <div className="stages-wrapper">
                {stages.length === 0 && (
                  <p className="empty-stages-message">Этапы не добавлены</p>
                )}
                {stages.map((stage, index) => (
                  <div key={index} className="stage-card">
                    <label>
                      <span className="label-text">Описание:</span>
                      <input
                        type="text"
                        value={stage.description}
                        onChange={(e) =>
                          handleChangeStage(
                            index,
                            "description",
                            e.target.value
                          )
                        }
                      />
                    </label>

                    <label>
                      <span className="label-text">Казна:</span>
                      <input
                        type="text"
                        value={stage.amount}
                        onChange={(e) =>
                          handleChangeStage(index, "amount", e.target.value)
                        }
                        onBlur={(e) =>
                          handleChangeStage(
                            index,
                            "amount",
                            parseAmount(e.target.value).toLocaleString("ru-RU")
                          )
                        }
                      />
                    </label>

                    <Button
                      onClick={() => handleDeleteStage(index)}
                      color="red"
                      className="delete-stage-button"
                    >
                      Удалить этап
                    </Button>
                  </div>
                ))}
              </div>
            </>
          ) : (
            <>
              <label>
                Казна:
                <input
                  className="gold-amount-input"
                  type="text"
                  value={goldAmount}
                  onChange={(e) => setGoldAmount(e.target.value)}
                  onBlur={(e) =>
                    setGoldAmount(
                      parseAmount(e.target.value).toLocaleString("ru-RU")
                    )
                  }
                />
              </label>
            </>
          )} */}
        </div>

        <div className="actions-wrapper">
          <Button onClick={handleSubmit}>Создать событие</Button>
        </div>
      </div>
    </>
  );
}

// export default function NewEventModal({ open, onClose, onSuccess }) {
//   const [eventTypes, setEventTypes] = useState([]);
//   const [date, setDate] = useState("");
//   const [eventTypeId, setEventTypeId] = useState("");
//   const [eventTypeName, setEventTypeName] = useState("");
//   const [status, setStatus] = useState(0);
//   const [goldAmount, setGoldAmount] = useState("");
//   const [stages, setStages] = useState([]);

//   const apiUrl = import.meta.env.VITE_API_URL;

//   useEffect(() => {
//     axios
//       .get(`${apiUrl}/api/Event/Types`, { withCredentials: true })
//       .then((res) => setEventTypes(res.data))
//       .catch(console.error);
//   }, []);

//   useEffect(() => {
//     if (!open) {
//       resetForm(); // Сброс формы при закрытии
//     }
//   }, [open]);

//   const resetForm = () => {
//     setDate(new Date().toISOString().slice(0, 10));
//     setEventTypeId("");
//     setEventTypeName("");
//     setStatus(0);
//     setGoldAmount("");
//     setStages([]);
//   };

//   const handleAddStage = () => {
//     setStages((prev) => [...prev, { description: "", amount: "" }]);
//   };

//   const handleChangeStage = (index, key, value) => {
//     setStages((prev) => {
//       const updated = [...prev];
//       updated[index][key] = value;
//       return updated;
//     });
//   };

//   const handleDeleteStage = (index) => {
//     setStages((prev) => prev.filter((_, i) => i !== index));
//   };

//   const parseAmount = (value) =>
//     parseFloat(String(value).replace(/\s/g, "").replace(",", "."));

//   const handleSubmit = async () => {
//     try {
//       const selectedType = eventTypes.find((et) => et.id === eventTypeId);
//       const isGold = selectedType?.eventTypeName === "Голд";

//       const eventRes = await axios.post(
//         `${apiUrl}/api/Event`,
//         {
//           date: new Date(date).toISOString(),
//           eventTypeId,
//           status: isGold ? null : parseInt(status),
//         },
//         { withCredentials: true }
//       );

//       const eventId = eventRes.data.idEvent;

//       if (isGold) {
//         await axios.post(
//           `${apiUrl}/api/Event/${eventId}/stages`,
//           {
//             stageNumber: null,
//             amount: parseAmount(goldAmount),
//             description: "Вынесли",
//           },
//           { withCredentials: true }
//         );
//       } else {
//         for (let i = 0; i < stages.length; i++) {
//           const stage = stages[i];
//           await axios.post(
//             `${apiUrl}/api/Event/${eventId}/stages`,
//             {
//               stageNumber: i,
//               amount: parseAmount(stage.amount),
//               description: stage.description,
//             },
//             { withCredentials: true }
//           );
//         }
//       }

//       onSuccess();
//       onClose();
//     } catch (err) {
//       console.error("Ошибка при добавлении события:", err);
//     }
//   };

//   return (
//     <NewEventModalWrapper open={open}>
//       <img
//         className="imageCloseModal"
//         src="/krest.png"
//         alt="Закрыть"
//         onClick={onClose}
//       />
//       <h2>Новое событие</h2>

//       <div className="form-group">
//         <label>Дата:</label>
//         <input
//           type="date"
//           value={date}
//           onChange={(e) => setDate(e.target.value)}
//         />

//         <label>Тип события:</label>
//         <select
//           value={eventTypeId}
//           onChange={(e) => {
//             const selectedId = e.target.value;
//             setEventTypeId(selectedId);
//             const selected = eventTypes.find((t) => t.id === selectedId);
//             setEventTypeName(selected?.eventTypeName || "");
//           }}
//         >
//           <option value="">-- Выберите тип --</option>
//           {eventTypes.map((type) => (
//             <option key={type.id} value={type.id}>
//               {type.eventTypeName}
//             </option>
//           ))}
//         </select>

//         {eventTypeName !== "Голд" ? (
//           <>
//             <label>Результат (0–4):</label>
//             <input
//               type="number"
//               min={0}
//               max={4}
//               value={status}
//               onChange={(e) => setStatus(e.target.value)}
//             />

//             <div style={{ margin: "10px 0" }}>
//               <Button onClick={handleAddStage}>+ Добавить этап</Button>
//             </div>

//             {stages.map((stage, index) => (
//               <div
//                 key={index}
//                 style={{
//                   backgroundColor: "#444",
//                   padding: "10px",
//                   marginBottom: "10px",
//                   borderRadius: "8px",
//                 }}
//               >
//                 <label>Описание:</label>
//                 <input
//                   type="text"
//                   value={stage.description}
//                   onChange={(e) =>
//                     handleChangeStage(index, "description", e.target.value)
//                   }
//                 />

//                 <label>Казна:</label>
//                 <input
//                   type="text"
//                   value={stage.amount}
//                   onChange={(e) =>
//                     handleChangeStage(index, "amount", e.target.value)
//                   }
//                   onBlur={(e) =>
//                     handleChangeStage(
//                       index,
//                       "amount",
//                       parseAmount(e.target.value).toLocaleString("ru-RU")
//                     )
//                   }
//                 />

//                 <Button onClick={() => handleDeleteStage(index)} color="red">
//                   Удалить этап
//                 </Button>
//               </div>
//             ))}
//           </>
//         ) : (
//           <>
//             <label>Казна:</label>
//             <input
//               type="text"
//               value={goldAmount}
//               onChange={(e) => setGoldAmount(e.target.value)}
//               onBlur={(e) =>
//                 setGoldAmount(
//                   parseAmount(e.target.value).toLocaleString("ru-RU")
//                 )
//               }
//             />
//           </>
//         )}
//       </div>

//       <Button onClick={handleSubmit}>Создать событие</Button>
//     </NewEventModalWrapper>
//   );
// }
