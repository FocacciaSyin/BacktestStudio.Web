using System;
using System.Collections.Generic;

namespace BacktestStudio.Repository.Models;

public partial class PurchaseRecord
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string Symbol { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
