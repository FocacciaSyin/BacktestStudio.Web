using BacktestStudio.Repository.Models;
using BacktestStudio.Service.Dtos;

namespace BacktestStudio.Service;

/// <summary>
/// 損益計算服務介面
/// </summary>
public interface IProfitLossService
{
    /// <summary>
    /// 計算單筆買入記錄的損益
    /// </summary>
    Task<decimal> CalculateProfitLossAsync(PurchaseRecord record, decimal currentPrice);

    /// <summary>
    /// 計算所有買入記錄的總損益
    /// </summary>
    Task<decimal> CalculateTotalProfitLossAsync(string symbol, decimal currentPrice);

    /// <summary>
    /// 計算損益百分比
    /// </summary>
    Task<decimal> CalculateProfitLossPercentageAsync(string symbol, decimal currentPrice);

    /// <summary>
    /// 計算當前市值
    /// </summary>
    Task<decimal> CalculateCurrentMarketValueAsync(string symbol, decimal currentPrice);

    /// <summary>
    /// 取得損益摘要資訊
    /// </summary>
    Task<ProfitLossSummaryDto> GetProfitLossSummaryAsync(string symbol, decimal currentPrice);

    /// <summary>
    /// 計算並設定記錄的獲利金額（根據當前價格或停損價格）
    /// </summary>
    decimal CalculateAndSetProfitAmount(PurchaseRecord record, decimal? currentPrice = null);

    /// <summary>
    /// 檢查是否觸發停損條件
    /// </summary>
    bool IsStopLossTriggered(PurchaseRecord record, decimal currentPrice);

    /// <summary>
    /// 計算實際結算損益（如果有結算日期）
    /// </summary>
    decimal CalculateSettlementProfit(PurchaseRecord record, decimal settlementPrice);
}