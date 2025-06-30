using Microsoft.AspNetCore.Mvc;

namespace BankBlazor.API.Controllers

}
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAccounts()
    {
        // Här kopplar du mot din databas med Database First
        return Ok(new[] { new { AccountId = 1, Balance = 1000 } });
    }
}
}