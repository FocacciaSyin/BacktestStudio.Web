namespace BacktestStudio.Service.Dtos;

/// <summary>
/// 損益摘要資訊
/// </summary>
public class ProfitLossSummaryDto
{
    /// <summary>
    /// 總投入金額
    /// </summary>
    public decimal TotalInvestment { get; set; }

    /// <summary>
    /// 當前市值
    /// </summary>
    public decimal CurrentMarketValue { get; set; }

    /// <summary>
    /// 總損益金額
    /// </summary>
    public decimal TotalProfitLoss { get; set; }

    /// <summary>
    /// 損益百分比
    /// </summary>
    public decimal ProfitLossPercentage { get; set; }

    /// <summary>
    /// 總持有數量
    /// </summary>
    public int TotalQuantity { get; set; }

    /// <summary>
    /// 平均買入價格
    /// </summary>
    public decimal AveragePurchasePrice { get; set; }

    /// <summary>
    /// 當前價格
    /// </summary>
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// 股票代碼
    /// </summary>
    public string Symbol { get; set; } = string.Empty;
}
