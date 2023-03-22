using System;
using System.Collections.Generic;

namespace AuthUser.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
    }
}
