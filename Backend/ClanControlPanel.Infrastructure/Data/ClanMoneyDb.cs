using ClanControlPanel.Core.Models;

namespace ClanControlPanel.Infrastructure.Data;

/// <summary>
/// Казна клана
/// </summary>
public class ClanMoneyDb
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
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
    /// Произвольная причина (только если Reason == Other)
    /// </summary>
    public string? CustomReason { get; set; }
}