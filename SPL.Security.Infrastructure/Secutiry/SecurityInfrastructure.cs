namespace SPL.Security.Infrastructure.Security
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    using SPL.Domain;
    using SPL.Domain.SPL.Security;
    using SPL.Security.Infrastructure.Common;
    using SPL.Security.Infrastructure.Entities;

    public class SecurityInfrastructure : ISecurityInfrastructure
    {
        private readonly dbDevMigSPLContext _dbContext;
        private readonly IMapper _Mapper;

        public SecurityInfrastructure(IMapper Map, dbDevMigSPLContext dbContext)
        {
            this._Mapper = Map;
            this._dbContext = dbContext;
        }

        #region Methods

        public Task<List<UserProfiles>> GetProfiles(string pKey)
        {
            List<SplPerfile> getData = new();

            getData = pKey == "-1"
                ? this._dbContext.SplPerfiles.AsNoTracking().ToList()
                : this._dbContext.SplPerfiles.AsNoTracking().Where(x => x.Clave.ToUpper().Equals(pKey)).ToList();

            return Task.FromResult(this._Mapper.Map<List<UserProfiles>>(getData));

        }

        public Task<long> SaveProfiles(UserProfiles pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                SplPerfile data = this._dbContext.SplPerfiles.AsNoTracking().FirstOrDefault(x => x.Clave == pData.Clave);

                if (data is not null)
                {
                    _dbContext.SplPerfiles.Update(this._Mapper.Map<SplPerfile>(pData));
                    _dbContext.SaveChanges();

                }
                else {
                    _dbContext.SplPerfiles.Add(this._Mapper.Map<SplPerfile>(pData));
                    _dbContext.SaveChanges();
                }

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }



        public Task<long> deleteProfiles(UserProfiles pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                SplPerfile data = this._dbContext.SplPerfiles.AsNoTracking().FirstOrDefault(x => x.Clave == pData.Clave);

                if (data is not null)
                {
                    SplPermiso permiso = this._dbContext.SplPermisos.AsNoTracking().FirstOrDefault(x => x.ClavePerfil == data.Clave);
                    if (permiso is not null)
                    {
                        throw new ArgumentException("El perfil no puede ser eliminado debido a que tiene permisos definidos");
                    }

                    SplAsignacionUsuario user = this._dbContext.SplAsignacionUsuarios.AsNoTracking().FirstOrDefault(x => x.ClavePerfil == data.Clave);

                    if (user is not null)
                    {
                        throw new ArgumentException("El perfil no puede ser eliminado debido a que está asignado a usuarios");
                    }


                    _dbContext.SplPerfiles.Remove(data);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
                }
                else
                {
                    throw new ArgumentException("No se puede eliminar porque no se encuentra el registro en BD");
                }

               
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }



        public Task<List<UserOptions>> GetOptionsMenu()
        {
            List<SplOpcione> getData = new();

            getData =  this._dbContext.SplOpciones.AsNoTracking().ToList();

            return Task.FromResult(this._Mapper.Map<List<UserOptions>>(getData));

        }



        public Task<List<UserPermissions>> GetPermissionsProfile(string pProfile)
        {
            List<SplPermiso> getData = new();

            getData = pProfile == "-1"
                ? this._dbContext.SplPermisos.AsNoTracking().ToList()
                : this._dbContext.SplPermisos.AsNoTracking().Where(x => x.ClavePerfil.ToUpper().Equals(pProfile)).ToList();

            return Task.FromResult(this._Mapper.Map<List<UserPermissions>>(getData));

        }



        public Task<long> SavePermissions(List<UserPermissions> pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                List<SplPermiso> data = this._dbContext.SplPermisos.Where(x=>x.ClavePerfil == pData.FirstOrDefault().ClavePerfil).ToList();

                if (data.Count() > 0)
                {
                    _dbContext.SplPermisos.RemoveRange(data);
                    _dbContext.SaveChanges();


                }

                if (pData.FirstOrDefault().ClaveOpcion != null)
                {
                    _dbContext.SplPermisos.AddRange(this._Mapper.Map<List<SplPermiso>>(pData));
                    _dbContext.SaveChanges();
                }
             



                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }



        public Task<List<Users>> GetUsers(string pName)
        {
            List<SplUsuario> getData = new();

            getData = pName == "-1"
                ? this._dbContext.SplUsuarios.AsNoTracking().ToList()
                : this._dbContext.SplUsuarios.AsNoTracking().Where(x => x.Nombre.Contains(pName) || x.NombreIdentificador.Contains(pName)).ToList();

            return Task.FromResult(this._Mapper.Map<List<Users>>(getData));

        }

        public Task<long> SaveUsers(List<Users> pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                List<SplUsuario> pDataNew = new List<SplUsuario>();
                List<SplUsuario> pDataUpdate = new List<SplUsuario>();
               
                foreach (var item in pData)
                {
                    SplUsuario data = this._dbContext.SplUsuarios.AsNoTracking().FirstOrDefault(x => x.NombreIdentificador.ToUpper().Trim() == item.NombreIdentificador.ToUpper().Trim());

                    if (data is null)
                    {
                        pDataNew.Add(this._Mapper.Map<SplUsuario>(item));
                    }
                    else
                    {

                        pDataUpdate.Add(this._Mapper.Map<SplUsuario>(item));
                    }


                   
                }

               

                if (pDataNew.Count() > 0)
                {
                    _dbContext.SplUsuarios.AddRange(pDataNew);
                    _dbContext.SaveChanges();

                }

                if (pDataUpdate.Count() > 0)
                {
                    _dbContext.SplUsuarios.UpdateRange(pDataUpdate);
                    _dbContext.SaveChanges();

                }

                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }








        public Task<List<AssignmentUsers>> GetAssignmentProfiles(string pProfile)
        {
            List<AssignmentUsers> getData = new();

            getData = this._Mapper.Map<List<AssignmentUsers>>(pProfile == "-1"
                ? this._dbContext.SplAsignacionUsuarios.AsNoTracking().ToList()
                : this._dbContext.SplAsignacionUsuarios.AsNoTracking().Where(x => x.ClavePerfil.ToUpper().Equals(pProfile)).ToList());


            foreach (var item in getData)
            {
                item.Name = GetUsers(item.UserId).Result.FirstOrDefault().Nombre;
            }

            return Task.FromResult(getData);

        }

        public Task<long> SaveAssignmentProfilesUsers(AssignmentUsers pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                SplAsignacionUsuario data = this._dbContext.SplAsignacionUsuarios.AsNoTracking().FirstOrDefault(x => x.UserId == pData.UserId);

                if (data is not null)
                {
                    _dbContext.SplAsignacionUsuarios.Update(this._Mapper.Map<SplAsignacionUsuario>(pData));
                   
                    _dbContext.SaveChanges();

                }
                else
                {
                    _dbContext.SplAsignacionUsuarios.Add(this._Mapper.Map<SplAsignacionUsuario>(pData));
                    _dbContext.SaveChanges();
                }
                SplUsuario user = _dbContext.SplUsuarios.AsNoTracking().Where(x => x.NombreIdentificador.Equals(pData.UserId)).FirstOrDefault();
                user.Nombre = pData.Name;
                _dbContext.SplUsuarios.Update(user);
                _dbContext.SaveChanges();
                transaction.Commit();
                return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }



        public Task<long> deleteAssignmentProfilesUsers(AssignmentUsers pData)
        {
            using IDbContextTransaction transaction = this._dbContext.Database.BeginTransaction();
            try
            {
                SplAsignacionUsuario data = this._dbContext.SplAsignacionUsuarios.AsNoTracking().FirstOrDefault(x => x.UserId == pData.UserId);

                if (data is not null)
                {
                 
                    _dbContext.SplAsignacionUsuarios.Remove(data);
                    _dbContext.SaveChanges();
                    transaction.Commit();
                    return Task.FromResult(Convert.ToInt64(Enums.EnumsGen.Succes));
                }
                else
                {
                    throw new ArgumentException("No se puede eliminar porque no se encuentra el registro en BD");
                }


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ArgumentException(ex.Message);

            }
        }




        public Task<List<UserPermissions>> GetPermissionUsers(string pIdUser)
        {
            List<UserPermissions> listPermisosPerfil = new();
            try
            {
                AssignmentUsers getDataAsignacion = new();

                getDataAsignacion = this._Mapper.Map<AssignmentUsers>(this._dbContext.SplAsignacionUsuarios.AsNoTracking().FirstOrDefault(x => x.UserId.ToUpper().Equals(pIdUser)));
                if(getDataAsignacion != null)
                {
                    listPermisosPerfil = GetPermissionsProfile(getDataAsignacion.ClavePerfil).Result;
                }

                return Task.FromResult(listPermisosPerfil);
            }
            catch(Exception e)
            {
                return Task.FromResult(listPermisosPerfil);

            }
 

        }

        #endregion
    }
}
