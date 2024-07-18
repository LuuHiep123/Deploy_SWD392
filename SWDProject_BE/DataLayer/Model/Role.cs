using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool Status { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
