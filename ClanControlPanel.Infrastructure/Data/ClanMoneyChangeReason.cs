namespace ClanControlPanel.Infrastructure.Data;

public enum ClanMoneyChangeReason
{
    Donation,        // Пополнение
    Sale,            // Продались
    Purchase,        // Покупка
    Gold,            // Деньги за голд
    Debt,            // Долг
    Other            // Другая причина (описание в CustomReason)
}