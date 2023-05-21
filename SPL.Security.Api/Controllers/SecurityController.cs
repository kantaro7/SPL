namespace SPL.Security.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using global::AutoMapper;

    using MediatR;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using SPL.Domain;
    using SPL.Domain.SPL.Security;
    using SPL.Security.Api.DTOs.Security;


    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        #region Fields
        private readonly IMediator _meditor;
        private readonly ILogger<SecurityController> _logger;
        private readonly IMapper _mapper;
        #endregion

        public SecurityController(ILogger<SecurityController> logger, IMapper mapper, IMediator meditor)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._meditor = meditor;
        }


        [HttpPost("deleteAssignmentProfilesUsers")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> DeleteAssignmentProfilesUsers(AssignmentUsersDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new SPL.Security.Application.Commands.Security.DeleteAssignmentProfilesUsersCommand(this._mapper.Map<AssignmentUsers>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }

        [HttpPost("deleteProfiles")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> DeleteProfiles(UserProfilesDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.DeleteProfilesCommand(this._mapper.Map<UserProfiles>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpGet("getAssignmentProfiles/{Profile}")]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentUsersDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAssignmentProfiles(string Profile)
        {
            try
            {
                ApiResponse<List<AssignmentUsers>> result = await this._meditor.Send(new Application.Queries.Security.GetAssignmentProfilesQuery(Profile));

                return new JsonResult(new ApiResponse<List<AssignmentUsersDto>>(result.Code, result.Description, this._mapper.Map<List<AssignmentUsersDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpGet("getOptionsMenu")]
        [ProducesResponseType(typeof(ApiResponse<List<UserOptionsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOptionsMenu()
        {
            try
            {
                ApiResponse<List<UserOptions>> result = await this._meditor.Send(new Application.Queries.Security.GetOptionsMenuQuery());

                return new JsonResult(new ApiResponse<List<UserOptionsDto>>(result.Code, result.Description, this._mapper.Map<List<UserOptionsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



        [HttpGet("getPermissionsProfile/{Profile}")]
        [ProducesResponseType(typeof(ApiResponse<List<UserPermissionsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPermissionsProfile(string Profile)
        {
            try
            {
                ApiResponse<List<UserPermissions>> result = await this._meditor.Send(new Application.Queries.Security.GetPermissionsProfileQuery(Profile));

                return new JsonResult(new ApiResponse<List<UserPermissionsDto>>(result.Code, result.Description, this._mapper.Map<List<UserPermissionsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpGet("getProfiles/{Profile}")]
        [ProducesResponseType(typeof(ApiResponse<List<UserProfilesDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProfiles(string Profile)
        {
            try
            {
                ApiResponse<List<UserProfiles>> result = await this._meditor.Send(new Application.Queries.Security.GetProfilesQuery(Profile));

                return new JsonResult(new ApiResponse<List<UserProfilesDto>>(result.Code, result.Description, this._mapper.Map<List<UserProfilesDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



        [HttpGet("getUsers")]
        [ProducesResponseType(typeof(ApiResponse<List<UserProfilesDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetUsers([FromQuery] string Name)
        {
            try
            {
                ApiResponse<List<Users>> result = await this._meditor.Send(new Application.Queries.Security.GetUsersQuery(Name));

                return new JsonResult(new ApiResponse<List<UsersDto>>(result.Code, result.Description, this._mapper.Map<List<UsersDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpPost("saveAssignmentProfilesUsers")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> SaveAssignmentProfilesUsers(AssignmentUsersDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.SaveAssignmentProfilesUsersCommand(this._mapper.Map<AssignmentUsers>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpPost("savePermissions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> SavePermissions(List<UserPermissionsDto> viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.SavePermissionsCommand(this._mapper.Map<List<UserPermissions>>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }


        [HttpPost("saveProfiles")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> SaveProfiles(UserProfilesDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.SaveProfilesCommand(this._mapper.Map<UserProfiles>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



        [HttpPost("saveUsers")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> SaveUsers(List<UsersDto> viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.SaveUsersCommand(this._mapper.Map<List<Users>>(viewModel)));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



        [HttpPost("loginUser")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> LoginUser(AssignmentUsersDto viewModel)
        {
            try
            {

                ApiResponse<long> result = await this._meditor.Send(new Application.Commands.Security.LoginUserCommand(new List<string>(),"",null));
                return new JsonResult(new ApiResponse<long>(result.Code, result.Description, result.Structure));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



        [HttpGet("getPermissionUsers/{IdUser}")]
        [ProducesResponseType(typeof(ApiResponse<List<UserPermissionsDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetPermissionUsers(string IdUser)
        {
            try
            {
                ApiResponse<List<UserPermissions>> result = await this._meditor.Send(new Application.Queries.Security.GetPermissionUsersQuery(IdUser));

                return new JsonResult(new ApiResponse<List<UserPermissionsDto>>(result.Code, result.Description, this._mapper.Map<List<UserPermissionsDto>>(result.Structure)));
            }
            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<long>(Enums.EnumsGen.Error, ex.Message, Enums.EnumsGen.Error));
            }
        }



    }

}

