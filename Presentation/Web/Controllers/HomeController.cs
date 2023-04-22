using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PimsPublisher.Application;
using PimsPublisher.Web.Models;
using MediatR;

namespace PimsPublisher.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;

    public HomeController(ILogger<HomeController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator; 
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult StartSynch(string btnValue)
    {
        TempData["synchronizationInfo"] = $"New Synchronization has stated at {DateTime.Now.ToUniversalTime()}";

        _mediator.Send(CreateSynchronizationSessionCommand.For(projectCode: "0230", modelCode:"Q0230-shop-str"));

        return View();
    }
}
