using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPTemplateSite.Models
{
    public class Post:IIdentity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
        [ForeignKey("PostOwner")]
        public int UserId { get; set; }
        public User PostOwner { get; set; }

    }
}