namespace SPL.WebApp.Domain.Services.ProfileSecurity
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProfileSecurityService
    {


        public  Task<ApiResponse<List<AssignmentUsersDTO>>> GetAssignmentProfiles(string Profile);

        public  Task<ApiResponse<long>> DeleteAssignmentProfilesUsers(AssignmentUsersDTO request);


        public  Task<ApiResponse<long>> DeleteProfiles(UserProfilesDTO request);


        public  Task<ApiResponse<List<UserOptionsDTO>>> GetOptionsMenu();

        public  Task<ApiResponse<List<UserPermissionsDTO>>> GetPermissionsProfile(string Profile);


        public  Task<ApiResponse<List<UserProfilesDTO>>> GetProfiles(string Profile);


        public  Task<ApiResponse<List<UsersDTO>>> GetUsers(string Name);

        public  Task<ApiResponse<long>> SaveAssignmentProfilesUsers(AssignmentUsersDTO request);

        public  Task<ApiResponse<long>> SaveProfiles(UserProfilesDTO request);
        public  Task<ApiResponse<long>> SavePermissions(List<UserPermissionsDTO> request);


        public  Task<ApiResponse<long>> SaveUsers(List<UsersDTO> request);

        public  Task<ApiResponse<List<UserPermissionsDTO>>> GetPermissionUsers(string IdUser);
        }
}
