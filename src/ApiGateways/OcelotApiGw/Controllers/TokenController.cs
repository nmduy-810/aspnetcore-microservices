using Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Identity;

namespace OcelotApiGw.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
   private readonly ITokenService _tokenService;

   public TokenController(ITokenService tokenService)
   {
      _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
   }

   [HttpGet]
   [AllowAnonymous]
   public IActionResult GetToken()
   {
      var result = _tokenService.GetToken(new TokenRequest());
      return Ok(result);
   }
}