using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } = null!;

        public virtual User Receiver { get; set; } = null!;
    }
}
