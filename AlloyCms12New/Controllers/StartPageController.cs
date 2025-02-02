using AlloyCms12New.Models.Pages;
using AlloyCms12New.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace AlloyCms12New.Controllers;

public class StartPageController(ILogger<StartPageController> logger) : PageControllerBase<StartPage>
{
    public IActionResult Index(StartPage currentPage)
    {

        logger.LogInformation("entering start page controller");

        var model = PageViewModel.Create(currentPage);

        // Check if it is the StartPage or just a page of the StartPage type.
        if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentPage.ContentLink))
        {
            // Connect the view models logotype property to the start page's to make it editable
            var editHints = ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();
            editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
            editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
            editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);
            editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
            editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);
        }

        return View(model);
    }
}
