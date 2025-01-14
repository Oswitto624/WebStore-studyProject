﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<User> _UserManager;
    private readonly SignInManager<User> _SignInManager;
    private readonly ILogger<AccountController> _Logger;

    public AccountController(UserManager<User> UserManager, 
                             SignInManager<User> SignInManager, 
                             ILogger<AccountController> Logger)
    {
        _UserManager = UserManager;
        _SignInManager = SignInManager;
        _Logger = Logger;
    }

    [AllowAnonymous]
    public async Task<IActionResult> IsFreeName(string UserName)
    {
        var user = await _UserManager.FindByNameAsync(UserName);

        _Logger.LogInformation("Валидация наличия пользователя {0} - {1}",
            UserName,
            user is null ? "отсутствует" : "существует");

        return Json(user is null ? true : $"Пользователь с таким именем {UserName} уже существует");
    }

    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterUserViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserViewModel Model)
    {
        if(!ModelState.IsValid)
            return View(Model);

        var user = new User
        {
            UserName = Model.UserName,
        };

        using (_Logger.BeginScope("Регистрация нового пользователя {0}", user.UserName))
        {
            var creation_result = await _UserManager.CreateAsync(user, Model.Password);

            if (creation_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} зарегистрирован", Model.UserName);

                await _SignInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in creation_result.Errors)
                ModelState.AddModelError("", error.Description);

            var error_info = string.Join(", ", creation_result.Errors.Select(x => x.Description));
            _Logger.LogWarning("Ошибка при регистрации пользователя {0}: {1}", Model.UserName, error_info);


            return View(Model);
        }
    }

    [AllowAnonymous]
    public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl});

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel Model)
    {
        if (!ModelState.IsValid)
            return View(Model);

        var login_result = await _SignInManager.PasswordSignInAsync(
            Model.UserName,
            Model.Password,
            Model.RememberMe,
            true);

        if (login_result.Succeeded)
        {
            //return Redirect("Index", "Home"); //не безопасно

            //if(Url.IsLocalUrl(model.ReturnUrl))
            //    return Redirect(model.ReturnUrl);
            //return RedirectToAction("Index", "Home");

            _Logger.LogInformation("Пользователь {0} успешно вошёл в систему", Model.UserName);
            return LocalRedirect(Model.ReturnUrl ?? "/");
        }
        //else if (login_result.RequiresTwoFactor)
        //{
        //    //выполнение двухфакторной проверки
        //}
        //else if (!login_result.IsLockedOut)
        //{
        //    //отправить пользователю информацию о том что он заблокирован
        //}

        ModelState.AddModelError("", "Неверное имя пользователя или пароль!");

        _Logger.LogWarning("Ошибка входа пользователя {0}", Model.UserName);

        return View(Model);
    }

    public async Task<IActionResult> Logout()
    {
        var user_name = User.Identity!.Name;

        await _SignInManager.SignOutAsync();
        
        _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

}
