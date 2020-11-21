namespace MyPlacesBox.Web.Areas.Administration.Controllers
{
    using MyPlacesBox.Common;
    using MyPlacesBox.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
