import React, { useEffect, useState } from "react";
import axios from "axios";
import Button from "../Button/Button";
import "./NewEventModal.css";
import { AnimatePresence, motion } from "framer-motion";
import NewEventModalWrapper from "../NewEventModalWrapper/NewEventModalWrapper";

export default function NewEventModal({ open, onClose, onSuccess }) {
  const [eventTypes, setEventTypes] = useState([]);
  const [stagesCount, setStagesCount] = useState(0);
  const [date, setDate] = useState("");
  const [eventTypeId, setEventTypeId] = useState("");
  const [eventTypeName, setEventTypeName] = useState("");
  const [status, setStatus] = useState(0);
  const [goldAmount, setGoldAmount] = useState("");
  const [playerName, setPlayerName] = useState("");
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
      const isBusts = selectedType?.eventTypeName === "Покупка бустов";
      const isCredit = selectedType?.eventTypeName === "Долг";

      const eventRes = await axios.post(
        `${apiUrl}/api/Event`,
        {
          date: new Date(date).toISOString(),
          eventTypeId,
          status: isGold || isBusts || isCredit ? null : parseInt(status),
        },
        { withCredentials: true }
      );
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
      }
      if (isCredit) {
        await axios.post(
          `${apiUrl}/api/Event/${eventId}/stages`,
          {
            stageNumber: null,
            amount: parseAmount(goldAmount),
            description: `Долг ${playerName}`,
          },
          { withCredentials: true }
        );
      } else if (isBusts) {
        await axios.post(
          `${apiUrl}/api/Event/${eventId}/stages`,
          {
            stageNumber: null,
            amount: parseAmount(goldAmount),
            description: `Бусты`,
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
              className="font-size"
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </label>

          <label>
            Тип события:
            <select
              value={eventTypeId}
              className="font-size"
              onChange={(e) => {
                const selectedId = e.target.value;
                setEventTypeId(selectedId);
                const selected = eventTypes.find((t) => t.id === selectedId);
                setEventTypeName(selected?.eventTypeName || "");
                setStagesCount("");
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

          {eventTypeName === "Потасовка" || eventTypeName === "Турнир" ? (
            <>
              <label>
                Количество этапов:
                <input
                  className="font-size"
                  type="number"
                  min={0}
                  max={4}
                  value={stagesCount}
                  onChange={(e) => {
                    const val =
                      Math.max(Math.min(4, Number(e.target.value))) || "";
                    setStagesCount(val);
                  }}
                />
              </label>
              <AnimatePresence>
                {stages.length > 0 && (
                  <div className="stages-outer-wrapper">
                    <motion.div
                      key="stages-wrapper"
                      className="stages-wrapper"
                      initial={{ opacity: 0, height: 0 }}
                      animate={{ opacity: 1, height: "auto" }}
                      exit={{ opacity: 0, height: 0 }}
                      transition={{ duration: 0.3 }}
                    >
                      {stages.map((stage, index) => (
                        <motion.div
                          key={index}
                          className="stage-card"
                          initial={{ opacity: 0, y: -10 }}
                          animate={{ opacity: 1, y: 0 }}
                          exit={{ opacity: 0, y: 10 }}
                          transition={{ duration: 0.2 }}
                        >
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
                                handleChangeStage(
                                  index,
                                  "amount",
                                  e.target.value
                                )
                              }
                              onBlur={(e) =>
                                handleChangeStage(
                                  index,
                                  "amount",
                                  parseAmount(e.target.value).toLocaleString(
                                    "ru-RU"
                                  )
                                )
                              }
                            />
                          </label>
                        </motion.div>
                      ))}
                    </motion.div>
                  </div>
                )}
              </AnimatePresence>
            </>
          ) : (
            <>
              <label className="credit-info">
                <span className="font-size">Сумма:</span>
                <input
                  type="text"
                  className="font-size"
                  value={goldAmount}
                  onChange={(e) => setGoldAmount(e.target.value)}
                  onBlur={(e) =>
                    setGoldAmount(
                      parseAmount(e.target.value).toLocaleString("ru-RU")
                    )
                  }
                />
                {eventTypeName === "Долг" && (
                  <>
                    <br />
                    <span className="font-size">Сумма:</span>
                    <input
                      type="text"
                      className="font-size"
                      value={goldAmount}
                      onChange={(e) => setGoldAmount(e.target.value)}
                      onBlur={(e) =>
                        setGoldAmount(
                          parseAmount(e.target.value).toLocaleString("ru-RU")
                        )
                      }
                    />
                  </>
                )}
              </label>
            </>
          )}
        </div>

        <div className="actions-wrapper">
          <Button onClick={handleSubmit}>Создать событие</Button>
        </div>
      </div>
    </>
  );
}
