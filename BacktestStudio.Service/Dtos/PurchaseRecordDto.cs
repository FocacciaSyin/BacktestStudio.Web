using System;
using System.ComponentModel.DataAnnotations;

namespace BacktestStudio.Service.Dtos
{
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date <= DateTime.Today;
            }
            return true;
        }
    }
    public class PurchaseRecordDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "買入日期為必填")]
        [DataType(DataType.Date)]
        [DateNotInFuture(ErrorMessage = "買入日期不能是未來日期")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "價格為必填")]
        [Range(0.01, double.MaxValue, ErrorMessage = "價格必須大於 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        public int Quantity { get; set; }

        /// <summary>
        /// 股票代號或商品符號
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// 停損點位（每股價格）
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "停損點位必須大於 0")]
        public decimal? StopLossPrice { get; set; }

        /// <summary>
        /// 獲利金額（總計）
        /// </summary>
        public decimal? ProfitAmount { get; set; }

        /// <summary>
        /// 結算日期（賣出或停損日期）
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? SettlementDate { get; set; }

        /// <summary>
        /// 計算投入金額（價格 × 數量）
        /// </summary>
        public decimal TotalInvestment => Price * Quantity;

        /// <summary>
        /// 計算當前價值（使用當前價格）
        /// </summary>
        public decimal CalculateCurrentValue(decimal currentPrice) => currentPrice * Quantity;

        /// <summary>
        /// 計算損益（當前價值 - 投入金額）
        /// </summary>
        public decimal CalculateProfitLoss(decimal currentPrice) => CalculateCurrentValue(currentPrice) - TotalInvestment;

        /// <summary>
        /// 計算損益百分比
        /// </summary>
        public decimal CalculateProfitLossPercentage(decimal currentPrice) =>
            TotalInvestment > 0 ? (CalculateProfitLoss(currentPrice) / TotalInvestment) * 100 : 0;
    }
}
