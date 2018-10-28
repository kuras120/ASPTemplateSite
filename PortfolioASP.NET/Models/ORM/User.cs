using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PortfolioASP.NET.Models
{
    public class User:IIdentity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [ForeignKey("UserSettings")]
        public int SettingsId { get; set; }
        public Settings UserSettings { get; set; }
        

    }
    public class ViewModelManager
    {
        public LoginViewModel LoginModel { get; set; }
        public RegisterViewModel RegisterModel { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Warunki korzystania muszą zostać zaakceptowane.")]
        public bool AcceptTerms { get; set; }
    }
}