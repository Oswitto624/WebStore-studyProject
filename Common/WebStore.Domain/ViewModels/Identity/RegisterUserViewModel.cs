﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels.Identity;

public class RegisterUserViewModel
{
    [Required]
    [Display(Name = "Имя пользователя")]
    [Remote("IsFreeName", "Account")]
    public string UserName { get; set; } = null!;

    [Required]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; } = null!;

}
