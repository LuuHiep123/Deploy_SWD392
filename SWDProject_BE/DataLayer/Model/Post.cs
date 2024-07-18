using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Exchangeds = new HashSet<Exchanged>();
            Groups = new HashSet<Group>();
            Ratings = new HashSet<Rating>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? ImageUrl { get; set; }
        public bool? PublicStatus { get; set; }
        public bool? ExchangedStatus { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Exchanged> Exchangeds { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
