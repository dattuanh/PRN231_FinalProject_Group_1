using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class FinancialProduct
    {
        public FinancialProduct()
        {
            ProductComparisons = new HashSet<ProductComparison>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ProductComparison> ProductComparisons { get; set; }
    }
}
