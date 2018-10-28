using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortfolioASP.NET.Models
{
    public class Comment:IIdentity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
        [ForeignKey("CommentOwner")]
        public int? UserId { get; set; }
        public User CommentOwner { get; set; }
        [ForeignKey("ParentPost")]
        public int PostId { get; set; }
        public Post ParentPost { get; set; }
    }
}