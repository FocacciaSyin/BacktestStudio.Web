using System;

namespace BacktestStudio.Service.Dtos
{
    public class PurchaseRecordDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
