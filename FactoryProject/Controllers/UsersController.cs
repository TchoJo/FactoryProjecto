using FactoryProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace FactoryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersBL _usersBL;
        private readonly IConfiguration _config;

        public UsersController(UsersBL usersBL, IConfiguration config)
        {
            _usersBL = usersBL;
            _config = config;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            return _usersBL.GetUsers();
        }

        int LoggedUserId = 0;

        // // GET: api/Users/5
        // [HttpGet("{id}", Name = "GetUser")]
        // public string GetUser(int id)
        // {
        //     return "value";
        // }

        // POST: api/Users
        [HttpPost]
        public ActionResult LoginUser([FromBody] Users LoginTry)
        {
           var auth = _usersBL.LogInUser(LoginTry.userName, LoginTry.password);
           if(auth != null) {
            LoggedUserId = auth.id;
            var token = GenerateToken(LoginTry);
            HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, Secure = true, Expires = DateTime.Now.AddMinutes(30)});
             
            return Ok(token);
           }
           else {
            return BadRequest();
           }
        }

        private string GenerateToken(Users LoginUser) {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var Claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, LoggedUserId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            Claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }










        // // PUT: api/Users/5
        // [HttpPut("{id}")]
        // public void PutUser(int id, [FromBody] string value)
        // {
        // }

        // // DELETE: api/Users/5
        // [HttpDelete("{id}")]
        // public void DeleteUser(int id)
        // {
        // }
    }
}
