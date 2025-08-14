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

    public decimal CalculateAndSetProfitAmount(PurchaseRecord record, decimal? currentPrice = null)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        // 如果已經有獲利金額且不重新計算，直接返回
        if (record.ProfitAmount.HasValue && !currentPrice.HasValue)
            return record.ProfitAmount.Value;

        decimal priceToUse;

        // 判斷使用哪個價格來計算
        if (record.SettlementDate.HasValue)
        {
            // 如果有結算日期，使用 ProfitAmount 反推結算價格，或使用當前價格
            if (record.ProfitAmount.HasValue)
            {
                return record.ProfitAmount.Value;
            }
            priceToUse = currentPrice ?? record.Price; // 如果沒有當前價格，使用買入價格
        }
        else if (currentPrice.HasValue && record.StopLossPrice.HasValue && 
                 IsStopLossTriggered(record, currentPrice.Value))
        {
            // 如果觸發停損，使用停損價格
            priceToUse = record.StopLossPrice.Value;
        }
        else
        {
            // 使用當前價格或買入價格
            priceToUse = currentPrice ?? record.Price;
        }

        // 計算獲利金額
        var profitAmount = (priceToUse - record.Price) * record.Quantity;
        
        // 更新記錄的獲利金額
        record.ProfitAmount = Math.Round(profitAmount, 2);
        
        return record.ProfitAmount.Value;
    }

    public bool IsStopLossTriggered(PurchaseRecord record, decimal currentPrice)
    {
        if (record == null || !record.StopLossPrice.HasValue)
            return false;

        // 如果當前價格低於或等於停損價格，則觸發停損
        return currentPrice <= record.StopLossPrice.Value;
    }

    public decimal CalculateSettlementProfit(PurchaseRecord record, decimal settlementPrice)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));

        if (settlementPrice < 0)
            throw new ArgumentException("結算價格不能小於零", nameof(settlementPrice));

        // 結算損益 = (結算價格 - 買入價格) × 數量
        var settlementProfit = (settlementPrice - record.Price) * record.Quantity;
        
        return Math.Round(settlementProfit, 2);
    }
}