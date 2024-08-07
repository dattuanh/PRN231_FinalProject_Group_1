﻿using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_API.Models
{
    public partial class Income
    {
        public int IncomeId { get; set; }
        public int? UserId { get; set; }
        public DateTime? IncomeDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Source { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
