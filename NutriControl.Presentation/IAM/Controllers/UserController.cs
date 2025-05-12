using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using _1_API.Response;
using AutoMapper;
using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.IAM.Models.Comands;
using NutriControl.Domain.IAM.Queries;
using NutriControl.Domain.IAM.Services;
using NutriControl.Presentation.Filters;


namespace NutriControl.Presentation.IAM.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;
        
        public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService, IMapper mapper)
        {
            _userCommandService = userCommandService;
            _userQueryService = userQueryService;
            _mapper = mapper;
        }

        // GET: api/v1/User
        ///<summary>Obtain all the active User</summary>
        /// <remarks>
        /// GET /api/User
        ///   </remarks>
        /// <response code="200">Returns all the User</response>
        /// <response code="404">If there are no User</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [ProducesResponseType( typeof(List<UserResponse>), 200)]
        [ProducesResponseType( typeof(void),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        //[AllowAnonymous]
        [CustomAuthorize("Admin")]
        [Route("getall")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _userQueryService.Handle(new GetUserAllQuery());
        
            if (result.Count == 0) return NotFound();

            return Ok(result);
        }
        
        // POST: api/v1/User
        /// <summary>
        /// Singup a new User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Account
        ///     {
        ///     "username": "Jorge Ruiz",
        ///     "dni": "28089824",
        ///     "ruc": "35355800618",
        ///     "emailAddress": "user@example.com",
        ///     "phone": "809324921", 
        ///     "role": "Farmer - Admin",
        ///     "password": "Securepassword123!",
        ///     "confirmPassword": "Securepassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="SinginCommad">The User to create</param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created User</response>
        /// <response code="400">If the User has invalid property</response>
        /// <response code="409">Error validating data</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] SingupCommand command)
        {
           var retun =  await _userCommandService.Handle(command);
            return CreatedAtAction("register", new { id = retun });
        }
        
        
        // POST: api/v1/User/5
        /// <summary>
        /// Login a new User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Account
        ///     {
        ///     "username": "Jorge Ruiz",
        ///     "password": "Securepassword123!",
        ///     }
        ///
        /// </remarks>
        /// <param name="SinginCommad">The User to create</param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created User</response>
        /// <response code="400">If the User has invalid property</response>
        /// <response code="409">Error validating data</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] SigninCommand command)
        {
            var retun =  await _userCommandService.Handle(command);
            return CreatedAtAction("login", new { jwt = retun });
        }
        
        
        // DELETE: api/v1/User/5
        ///<summary>Obtain all the active User</summary>
        /// <remarks>
        /// DELETE: api/User/5
        ///   </remarks>
        /// <response code="200">Returns delete the User</response>
        /// <response code="404">If there are no User</response>
        /// <response code="500">If there is an internal server error</response>
        [ProducesResponseType( typeof(UserResponse), 200)]
        [ProducesResponseType( typeof(void),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void),StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpDelete("{id}")]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult>DeleteAsync(int id)
        {
            DeleteUserCommand command = new DeleteUserCommand { Id = id };
            var result = await _userCommandService.Handle(command);
            return Ok();
        }
    }
}
