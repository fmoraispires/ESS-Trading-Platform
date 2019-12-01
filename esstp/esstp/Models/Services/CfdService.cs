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
    public interface ICfdService
    {
        Task<ICollection<Cfd>> GetAllAsync();
        Task<Cfd> FindAsync(Expression<Func<Cfd, bool>> match);
        Task<Cfd> AddAsync(Cfd entity, int user_oid);
        Task<Cfd> UpdateAsync(Cfd entity, int user_oid);
        Task<Cfd> GetAsync(params object[] key);
        Task<ICollection<Cfd>> FindAllAsync(Expression<Func<Cfd, bool>> match);
    }

    //public delegate void EventHandler();

    public class CfdService : ICfdService
    { 
        //database
        private DataContext _context;
        private IMapper _mapper;
        


        public CfdService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }



        //ReadAll
        public async Task<ICollection<Cfd>> GetAllAsync()
        {
            return await _context.Cfds.ToListAsync();
        }



        //Create
        public async Task<Cfd> AddAsync(Cfd entity, int user_oid)
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
            _context.Set<Cfd>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<Cfd>> FindAllAsync(Expression<Func<Cfd, bool>> match)
        {
            return await _context.Set<Cfd>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<Cfd> GetAsync(params object[] key)
        {
            return await _context.Set<Cfd>().FindAsync(key);
        }


        //ReadById
        public async Task<Cfd> FindAsync(Expression<Func<Cfd, bool>> match)
        {
            try
            {
                return await _context.Set<Cfd>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //Update
        public async Task<Cfd> UpdateAsync(Cfd entity, int user_oid)
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


        protected void ChangeEntityState(Cfd entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Cfd>().Attach(entity);
            entry.State = state;
        }

    }

}
