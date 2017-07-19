using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class Customer 
    {

        public int Id { get; set; }
        
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
        public string ConfirmPassword {get; set;}
        
        public CustomerData CustomerData { get; set; }

        public Meal Meal { get; set; }
        
    }

}