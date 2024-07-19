using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRN231_FinalProject_Client.Models
{
    public partial class Income
    {
        public int IncomeId { get; set; }
        public int? UserId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IncomeDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,0}₫")]
        public decimal Amount { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
