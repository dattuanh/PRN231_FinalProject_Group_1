using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_API.Models
{
    public partial class PaymentReminder
    {
        public int ReminderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}
