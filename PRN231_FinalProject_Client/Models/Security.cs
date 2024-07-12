using System;
using System.Collections.Generic;

namespace PRN231_FinalProject_Client.Models
{
    public partial class Security
    {
        public int SecurityId { get; set; }
        public int? UserId { get; set; }
        public string? EncryptionKey { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual User? User { get; set; }
    }
}
