using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using esstp.Models;
using Microsoft.EntityFrameworkCore;

namespace esstp.Models
{
    public interface IRoleService
    {
        Task<ICollection<Role>> GetAllAsync();
        Task<Role> GetAsync(params object[] key);
        Task<Role> FindAsync(Expression<Func<Role, bool>> match);
        Task<ICollection<Role>> FindAllAsync(Expression<Func<Role, bool>> match);
        Task<Role> AddAsync(Role entity, int user_oid);
        Task<Role> UpdateAsync(Role entity, int user_oid);
        Task<int> DeleteAsync(Role entity);
    }


    public class RoleService : IRoleService
    {
        //database
        private DataContext _context;

        public RoleService(DataContext context)
        {
            _context = context;
        }




        //Create
        public async Task<Role> AddAsync(Role entity, int user_oid)
        {          
            // validation
            var dbItem = await FindAllAsync(j => j.RoleValue == entity.RoleValue || j.RoleName == entity.RoleName );       
            if (dbItem == null || dbItem.Count >0)
                return null;


            // logic
            entity.Id = 0; //reset oid
            entity.CreatedBy = user_oid;
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedBy  = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsInactive = 0;
            // access db
            _context.Set<Role>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }



        //ReadById
        public async Task<Role> GetAsync(params object[] key)
        {
            try 
            { 
                return await _context.Set<Role>().FindAsync(key);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //ReadByRoleId
        public async Task<Role> FindAsync(Expression<Func<Role, bool>> match)
        {
            try
            {
                return await _context.Set<Role>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task<ICollection<Role>> FindAllAsync(Expression<Func<Role, bool>> match)
        {
            try
            {
                return await _context.Set<Role>().Where(match).ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //ReadAll
        public async Task<ICollection<Role>> GetAllAsync()
        {
            try
            {
                return await  _context.Roles.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
         
        }



        //Update
        public async Task<Role> UpdateAsync(Role entity, int user_oid)
        {
            // validation
            var dbItem = await FindAllAsync(j => (j.RoleValue == entity.RoleValue || j.RoleName == entity.RoleName ) && j.Id!=entity.Id);
            if (dbItem == null || dbItem.Count > 0)
                return null;


            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;

            ChangeEntityState(entity, EntityState.Modified);
            await _context.SaveChangesAsync();
            return entity;
        }


        protected void ChangeEntityState(Role entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Role>().Attach(entity);
            entry.State = state;
        }


        //Delete
        public async Task<int> DeleteAsync(Role entity)
        {
            _context.Set<Role>().Remove(entity);
            return await _context.SaveChangesAsync();
        }



    }
}
