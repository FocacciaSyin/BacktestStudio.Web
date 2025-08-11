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
}