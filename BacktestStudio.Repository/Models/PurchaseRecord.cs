using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BacktestStudio.Repository.Models;

public partial class PurchaseRecord
{
    /// <summary>
    /// 主鍵識別碼
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 購買日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 購買價格（每股）
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 購買數量（股數）
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 股票代號或商品符號
    /// </summary>
    public string Symbol { get; set; } = null!;

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
    public DateTime? SettlementDate { get; set; }

    /// <summary>
    /// 記錄建立時間
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 記錄最後更新時間
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
