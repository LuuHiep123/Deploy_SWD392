using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
    public class CommentResponseModel
    {
        public int id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? Date { get; set; }
        public bool Status { get; set; }
        public UserResponse User { get; set; } = null!;
    }
}
