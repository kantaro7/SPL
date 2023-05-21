namespace SPL.Security.Api.AutoMapper
{
    using AutoMapper;

    using global::AutoMapper;


    //using SPL.Security.Infrastructure.Entities;
    using SPL.Domain.SPL.Security;
    using SPL.Security.Api.DTOs.Security;
    using SPL.Security.Infrastructure.Entities;

    public class SecurityProfile : Profile
    {
        public SecurityProfile()
        {
            _ = this.CreateMap<AssignmentUsersDto, AssignmentUsers>().ReverseMap();
            _ = this.CreateMap<UserOptionsDto, UserOptions>().ReverseMap();
            _ = this.CreateMap<UserPermissionsDto, UserPermissions>().ReverseMap();
            _ = this.CreateMap<UserProfilesDto, UserProfiles>().ReverseMap();
      
            _ = this.CreateMap<UsersDto, Users>().ReverseMap();


            _ = this.CreateMap<SplPerfile, UserProfiles>().ReverseMap();
            _ = this.CreateMap<SplOpcione, UserOptions>().ReverseMap();
            _ = this.CreateMap<SplPermiso, UserPermissions>().ReverseMap();
            _ = this.CreateMap<SplUsuario, Users>().ReverseMap();
            _ = this.CreateMap<SplAsignacionUsuario, AssignmentUsers>().ReverseMap();






        }
    }
}


