using BacktestStudio.Repository.Models;
using BacktestStudio.Service.Dtos;

namespace BacktestStudio.Service;

public class ProfitLossService : IProfitLossService
{
    private readonly IPurchaseRecordService _purchaseRecordService;

    public ProfitLossService(IPurchaseRecordService purchaseRecordService)
    {
        _purchaseRecordService = purchaseRecordService;
    }

    public async Task<decimal> CalculateProfitLossAsync(PurchaseRecord record, decimal currentPrice)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        if (currentPrice < 0)
            throw new ArgumentException("當前價格不能小於零", nameof(currentPrice));

        // 損益 = (當前價格 - 買入價格) × 買入數量
        return (currentPrice - record.Price) * record.Quantity;
    }

    public async Task<decimal> CalculateTotalProfitLossAsync(string symbol, decimal currentPrice)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentException("股票代碼不能為空", nameof(symbol));

        if (currentPrice < 0)
            throw new ArgumentException("當前價格不能小於零", nameof(currentPrice));

        var records = await _purchaseRecordService.GetBySymbolAsync(symbol);
        var totalProfitLoss = 0m;

        foreach (var record in records)
        {
            totalProfitLoss += await CalculateProfitLossAsync(record, currentPrice);
        }

        return totalProfitLoss;
    }

    public async Task<decimal> CalculateProfitLossPercentageAsync(string symbol, decimal currentPrice)
    {
        var totalInvestment = await _purchaseRecordService.GetTotalInvestmentAsync(symbol);
        
        if (totalInvestment == 0)
            return 0;

        var totalProfitLoss = await CalculateTotalProfitLossAsync(symbol, currentPrice);
        
        // 損益百分比 = (總損益 / 總投入金額) × 100%
        return Math.Round((totalProfitLoss / totalInvestment) * 100, 2);
    }

    public async Task<decimal> CalculateCurrentMarketValueAsync(string symbol, decimal currentPrice)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentException("股票代碼不能為空", nameof(symbol));

        if (currentPrice < 0)
            throw new ArgumentException("當前價格不能小於零", nameof(currentPrice));

        var totalQuantity = await _purchaseRecordService.GetTotalQuantityAsync(symbol);
        
        // 當前市值 = 當前價格 × 總持有數量
        return currentPrice * totalQuantity;
    }

    public async Task<ProfitLossSummaryDto> GetProfitLossSummaryAsync(string symbol, decimal currentPrice)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentException("股票代碼不能為空", nameof(symbol));

        if (currentPrice < 0)
            throw new ArgumentException("當前價格不能小於零", nameof(currentPrice));

        var totalInvestment = await _purchaseRecordService.GetTotalInvestmentAsync(symbol);
        var currentMarketValue = await CalculateCurrentMarketValueAsync(symbol, currentPrice);
        var totalProfitLoss = await CalculateTotalProfitLossAsync(symbol, currentPrice);
        var profitLossPercentage = await CalculateProfitLossPercentageAsync(symbol, currentPrice);
        var totalQuantity = await _purchaseRecordService.GetTotalQuantityAsync(symbol);
        var averagePurchasePrice = await _purchaseRecordService.GetAveragePurchasePriceAsync(symbol);

        return new ProfitLossSummaryDto
        {
            TotalInvestment = Math.Round(totalInvestment, 2),
            CurrentMarketValue = Math.Round(currentMarketValue, 2),
            TotalProfitLoss = Math.Round(totalProfitLoss, 2),
            ProfitLossPercentage = profitLossPercentage,
            TotalQuantity = totalQuantity,
            AveragePurchasePrice = Math.Round(averagePurchasePrice, 2),
            CurrentPrice = currentPrice,
            Symbol = symbol
        };
    }
}