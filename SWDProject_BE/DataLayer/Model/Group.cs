using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Group
    {
        public Group()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserExchangeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User UserExchange { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; }
    }
}
