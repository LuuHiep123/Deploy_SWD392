using System;
using System.Collections.Generic;

namespace DataLayer.Model;

public partial class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public string? ImageUrl { get; set; }

    public bool? PublicStatus { get; set; }

    public bool? ExchangedStatus { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Exchanged> Exchangeds { get; set; } = new List<Exchanged>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual User User { get; set; } = null!;
}
