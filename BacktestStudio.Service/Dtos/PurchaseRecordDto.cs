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
    }
}
