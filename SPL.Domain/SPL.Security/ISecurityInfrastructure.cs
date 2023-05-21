namespace SPL.Domain.SPL.Security
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using System.Linq;
    using System.Text;


    public interface ISecurityInfrastructure
    {
        public Task<List<UserProfiles>> GetProfiles(string pKey);
        public Task<long> SaveProfiles(UserProfiles pData);
        public Task<long> deleteProfiles(UserProfiles pData);
        public Task<List<UserOptions>> GetOptionsMenu();
        public Task<List<UserPermissions>> GetPermissionsProfile(string pProfile);
        public Task<long> SavePermissions(List<UserPermissions> pData);
        public Task<List<Users>> GetUsers(string pName);
        public Task<long> SaveUsers(List<Users> pData);
        public Task<List<AssignmentUsers>> GetAssignmentProfiles(string pProfile);
        public Task<long> SaveAssignmentProfilesUsers(AssignmentUsers pData);
        public Task<long> deleteAssignmentProfilesUsers(AssignmentUsers pData);

        public Task<List<UserPermissions>> GetPermissionUsers(string pIdUser);
        }
}