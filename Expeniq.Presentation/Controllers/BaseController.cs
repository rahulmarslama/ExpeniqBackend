using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expeniq.Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class BaseController : ControllerBase
    {
    }
}
