﻿using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components;

public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => User.Identity?.IsAuthenticated == true
        ? View("UserInfo")
        : View();

}
