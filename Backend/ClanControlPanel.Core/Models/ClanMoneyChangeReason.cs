namespace ClanControlPanel.Core.Models;

public enum ClanMoneyChangeReason
{
    Donation,        // Пополнение
    Sale,            // Продались
    Purchase,        // Покупка
    Gold,            // Деньги за голд
    Debt,            // Долг
    Other            // Другая причина (описание в CustomReason)
}