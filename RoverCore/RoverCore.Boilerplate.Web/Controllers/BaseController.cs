﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RoverCore.BreadCrumbs.Services;
using RoverCore.ToastNotification.Abstractions;

namespace RoverCore.Boilerplate.Web.Controllers;

public class BaseController : Controller
{
    private IBreadCrumbService _breadCrumbInstance;
    private INotyfService _toastInstance;

    protected INotyfService _toast => _toastInstance ??= HttpContext.RequestServices.GetService<INotyfService>();

    protected IBreadCrumbService _breadcrumbs =>
        _breadCrumbInstance ??= HttpContext.RequestServices.GetService<IBreadCrumbService>();
}