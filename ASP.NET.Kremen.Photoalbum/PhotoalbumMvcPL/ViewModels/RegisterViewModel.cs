using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PhotoalbumMvcPL.ViewModels
{
    public class RegisterViewModel
    {
        [ScaffoldColumn(false)]//The property will not be seen any form or in the code page
        public int Id { get; set; }
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string UserName { get; set; }

        [Display(Name = "Введите email")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать по крайней мере {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли должны совпадать.")]
        public string ConfirmPassword { get; set; }


        [ScaffoldColumn(false)]
        public int CaptchaValue { get; set; }



        [Required]
        public string Captcha { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationTime { get; set; }

        [Display(Name = "Аватар")]

        public byte[] UserPhotoe { get; set; }//The property will not be visible in the page code default type without attributes

        [HiddenInput(DisplayValue = false)]//The property will not be seen any form or in the code page
        public string UserPhotoMimeType { get; set; }
    }
}