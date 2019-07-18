using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPTemplateSite.Models
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    public class Logs:IIdentity
    {
        public int Id { get; set; }
        public LogType Type { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }
        [ForeignKey("UserLogs")]
        public int UserId { get; set; }
        public User UserLogs { get; set; }

    }
}