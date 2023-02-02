using FactoryProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace FactoryProject.Controllers
{
    [Authorize]
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


          private string GenerateToken([FromBody] Users LoginUser) {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            
            var Claims = new Claim[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, (LoginUser.id).ToString()),
                new Claim(JwtRegisteredClaimNames.Name, LoginUser.userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            Claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // GET: api/Users
        [HttpGet]
         public ActionResult GetUsers()
         {
             int CurrentUserActionsLeft = _usersBL.UserActionLimit();
             if(CurrentUserActionsLeft > 0)
             {
                return Ok(CurrentUserActionsLeft);
             }
             else
             {
                return Unauthorized("NO MORE ACTIONS LEFT FOR TODAY!");
             }

         }
     

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public int GetUser()
         {
            
             return 1;
         }

        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoginUser([FromBody] Users LoginTry)
        {
           var auth = _usersBL.LogInUser(LoginTry.userName, LoginTry.password);
            if (auth.numOfActions > 0)
            {
                var token = GenerateToken(auth);
                return Ok(token);
            }
            else if (auth.numOfActions == 0)
            {
                bool LogUserOut = _usersBL.LogOutUser();
                return Ok(LogUserOut);
            }
            else
            {
                return BadRequest("Wrong Username or Password, please try again.");
            }
           }




        [HttpPost("UserInfo")]
        public ActionResult UserInfo([FromBody] string token)
        {
            var oooo = _usersBL.UserInfoFromToken(token);
            return Ok(oooo);
        }




        // // PUT: api/Users/5
        [HttpPut("{id}")]
        public int UpdateLoggedUserActoinLinit()
        {
             return _usersBL.UserActionLimit();
        }

        //DELETE: api/Users/5
        [HttpDelete]
        public ActionResult LogOut()
        {
            bool x = _usersBL.LogOutUser();
            if (x == true)
            {
                return Ok("User Logged out successfuly");
            }
            else
            {
                return BadRequest("Oops, Cannot log user out!");
            }
            
        }
    }
}


//Tried to implement a cookie but not successfully, might try and fix later (?)
               // var TokenHandler = new JwtSecurityTokenHandler();
                // var SecurityToken = TokenHandler.ReadToken(token) as JwtSecurityToken;
                // var userId = SecurityToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
                // CookieOptions cookieOptions = new CookieOptions 
                // {
                //     HttpOnly = true,
                //     Secure = true, 
                //     SameSite = SameSiteMode.None,
                //     IsEssential = true
                // };
                // HttpContext.Response.Cookies.Append("Authorization", "Bearer " + token, cookieOptions);