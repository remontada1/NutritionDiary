using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;

namespace WebApplication1.Models
{
    public class User
    {

        /*   public int Id { get; set; }
        
           [DataType(DataType.EmailAddress)]
           public string Email{ get; set; }

           [Required(ErrorMessage = "Необходимо ввести пароль")]
           [StringLength(30,ErrorMessage="Пароль должен быть от 6 до 30 символов", MinimumLength=6)]
           [DataType(DataType.Password)]
           public string Password { get; set; }

           [Required(ErrorMessage = "Необходимо подтвердить пароль")]
           [StringLength(30, ErrorMessage = "Пароль должен быть от 6 до 30 символов", MinimumLength = 6)]
           [DataType(DataType.Password)]
           [Compare("Password", ErrorMessage = "Пароли не совпадают")]
           public string ConfirmPassword {get; set;} */

        private ICollection<ExternalLogin> _externalLogins;

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        public DateTime JoinDate { get; set; }
        public int Weight { get; set; }
        public CustomerData CustomerData { get; set; }

        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return _externalLogins ??
                    (_externalLogins = new List<ExternalLogin>());
            }
            set { _externalLogins = value; }
        }
    }

}