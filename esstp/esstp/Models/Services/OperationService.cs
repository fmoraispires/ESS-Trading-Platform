using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using esstp.Controllers;
using Microsoft.EntityFrameworkCore;

namespace esstp.Models.Services
{
    public interface IOperationService
    {
        Task<ICollection<Operation>> GetAllAsync();
        Task<Operation> FindAsync(Expression<Func<Operation, bool>> match);
        Task<Operation> AddAsync(Operation entity, int user_oid);
        Task<Operation> UpdateAsync(Operation entity, int user_oid);
        Task<Operation> GetAsync(params object[] key);
        Task<ICollection<Operation>> FindAllAsync(Expression<Func<Operation, bool>> match);
    }

    //public delegate void EventHandler();

    public class OperationService : IOperationService
    { 
        //database
        private DataContext _context;
        private IMapper _mapper;
        


        public OperationService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }



        //ReadAll
        public async Task<ICollection<Operation>> GetAllAsync()
        {
            return await _context.Operation.ToListAsync();
        }



        //Create
        public async Task<Operation> AddAsync(Operation entity, int user_oid)
        {
            // validation
            //var dbItem = await FindAsync(j => j.Id == entity.Id);
            //if (dbItem != null)
            //    return null;


            // logic
            entity.Id = 0; //reset oid
            entity.CreatedBy = user_oid;
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsInactive = 0;
            // access db
            _context.Set<Operation>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<Operation>> FindAllAsync(Expression<Func<Operation, bool>> match)
        {
            return await _context.Set<Operation>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<Operation> GetAsync(params object[] key)
        {
            return await _context.Set<Operation>().FindAsync(key);
        }


        //ReadById
        public async Task<Operation> FindAsync(Expression<Func<Operation, bool>> match)
        {
            try
            {
                return await _context.Set<Operation>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //Update
        public async Task<Operation> UpdateAsync(Operation entity, int user_oid)
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


        protected void ChangeEntityState(Operation entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Operation>().Attach(entity);
            entry.State = state;
        }

    }

}
