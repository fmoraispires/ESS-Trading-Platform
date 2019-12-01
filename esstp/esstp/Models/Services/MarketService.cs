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
    public interface IMarketService
    {
        Task<ICollection<Market>> GetAllAsync();
        Task<Market> FindAsync(Expression<Func<Market, bool>> match);
        Task<Market> AddAsync(Market entity, int user_oid);
        Task<Market> UpdateAsync(Market entity, int user_oid);
        Task<Market> GetAsync(params object[] key);
        Task<ICollection<Market>> FindAllAsync(Expression<Func<Market, bool>> match);
    }


    public class MarketService : IMarketService
    { 
        //database
        private DataContext _context;
       
        
    

        public MarketService(DataContext context)
        {
            _context = context;
        }



        //ReadAll
        public async Task<ICollection<Market>> GetAllAsync()
        {
            return await _context.Markets.ToListAsync();
        }



        //Create
        public async Task<Market> AddAsync(Market entity, int user_oid)
        {
            // validation
            var dbItem = await FindAsync(j => j.Isin == entity.Isin);
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
            _context.Set<Market>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<Market>> FindAllAsync(Expression<Func<Market, bool>> match)
        {
            return await _context.Set<Market>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<Market> GetAsync(params object[] key)
        {
            return await _context.Set<Market>().FindAsync(key);
        }


        //ReadById
        public async Task<Market> FindAsync(Expression<Func<Market, bool>> match)
        {
            try
            {
                return await _context.Set<Market>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //Update
        public async Task<Market> UpdateAsync(Market entity, int user_oid)
        {
            // validation
            var dbItem = await GetAsync(entity.Isin);
            if (dbItem == null)
                return null;

            entity.Id = dbItem.Id;
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;

            ChangeEntityState(entity, EntityState.Modified);
            await _context.SaveChangesAsync();
    

            return entity;
        }



        protected void ChangeEntityState(Market entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Market>().Attach(entity);
            entry.State = state;
        }

    }

}
