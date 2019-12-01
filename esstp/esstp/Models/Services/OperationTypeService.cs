using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace esstp.Models.Services
{
    public interface IOperationTypeService
    {
        Task<ICollection<OperationType>> GetAllAsync();
        Task<OperationType> FindAsync(Expression<Func<OperationType, bool>> match);
        Task<OperationType> AddAsync(OperationType entity, int user_oid);
        Task<OperationType> UpdateAsync(OperationType entity, int user_oid);
        Task<OperationType> GetAsync(params object[] key);
        Task<ICollection<OperationType>> FindAllAsync(Expression<Func<OperationType, bool>> match);
    }

    //public delegate void EventHandler();

    public class OperationTypeService : IOperationTypeService
    { 
        //database
        private DataContext _context;
        


        public OperationTypeService(DataContext context)
        {
            _context = context;
        }



        //ReadAll
        public async Task<ICollection<OperationType>> GetAllAsync()
        {
            return await _context.OperationTypes.ToListAsync();
        }



        //Create
        public async Task<OperationType> AddAsync(OperationType entity, int user_oid)
        {
            // validation
            var dbItem = await FindAsync(j => j.Id == entity.Id);
            if (dbItem != null)
                return null;


            // logic
            entity.Id = 0; //reset oid
            entity.CreatedBy = user_oid;
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsInactive = 0;
            // access db
            _context.Set<OperationType>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<OperationType>> FindAllAsync(Expression<Func<OperationType, bool>> match)
        {
            return await _context.Set<OperationType>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<OperationType> GetAsync(params object[] key)
        {
            return await _context.Set<OperationType>().FindAsync(key);
        }


        //ReadById
        public async Task<OperationType> FindAsync(Expression<Func<OperationType, bool>> match)
        {
            try
            {
                return await _context.Set<OperationType>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //Update
        public async Task<OperationType> UpdateAsync(OperationType entity, int user_oid)
        {
            // validation
            var dbItem = await GetAsync(entity.Id);
            if (dbItem == null)
                return null;

            entity.Id = dbItem.Id;
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;

            ChangeEntityState(entity, EntityState.Modified);
            await _context.SaveChangesAsync();
            
            return entity;
        }


        protected void ChangeEntityState(OperationType entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<OperationType>().Attach(entity);
            entry.State = state;
        }

    }

}
