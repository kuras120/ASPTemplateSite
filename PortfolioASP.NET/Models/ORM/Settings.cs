using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortfolioASP.NET.Models
{
    public class Settings:IIdentity
    {
        public int Id { get; set; }
        public int BackgroundType { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? BanTime { get; set; }
    }
}