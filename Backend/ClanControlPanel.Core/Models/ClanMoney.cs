namespace ClanControlPanel.Core.Models;

/// <summary>
/// Казна клана
/// </summary>
public class ClanMoney
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Дата изменения
    /// </summary>
    public DateTime ActionDate { get; set; }
    /// <summary>
    /// Сумма казны после действия
    /// </summary>
    public decimal TotalAmountAfterAction { get; set; }
    /// <summary>
    /// Сумма изменения (+ или -)
    /// </summary>
    public decimal ChangeAmount { get; set; }
    /// <summary>
    /// Фиксированная причина (enum)
    /// </summary>
    public ClanMoneyChangeReason Reason { get; set; } 
    /// <summary>
    /// Произвольная причина (только если Reason == Other)
    /// </summary>
    public string? CustomReason { get; set; }
}