using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using esstp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace esstp.Models.Services
{
    public interface IPortfolioService
    {
        Task<ICollection<Portfolio>> GetAllAsync();
        Task<Portfolio> FindAsync(Expression<Func<Portfolio, bool>> match);
        Task<Portfolio> AddAsync(Portfolio entity, int user_oid);
        Task<Portfolio> UpdateAsync(Portfolio entity, int user_oid);
        Task<Portfolio> GetAsync(params object[] key);
        Task<ICollection<Portfolio>> FindAllAsync(Expression<Func<Portfolio, bool>> match);
        
    }



    public class PortfolioService : IPortfolioService
    {

        //database
        private DataContext _context;


        public PortfolioService(DataContext context)
        {
            _context = context;
        }



        //ReadAll
        public async Task<ICollection<Portfolio>> GetAllAsync()
        {
            return await _context.Portfolios.ToListAsync();

        }



        //Create
        public async Task<Portfolio> AddAsync(Portfolio entity, int user_oid)
        {
            // validation
            var dbItem = await FindAsync(j => j.Id == entity.Id);
            if (dbItem != null)
                return null;

            //var brokerMG = Convert.ToDouble(AppConfig.Config["AppSettings:brokerMarginInPercent"]);


            // logic
            entity.Id = 0; //reset oid
            entity.State = 0;
            entity.ValueOpen = Math.Round(entity.ValueOpen, 2, MidpointRounding.AwayFromZero);
            entity.ValueClosed = 0;
            entity.Invested = Math.Round(entity.Invested, 2, MidpointRounding.AwayFromZero);
            entity.StopLoss = Math.Round(entity.StopLoss, 2, MidpointRounding.AwayFromZero);
            entity.TakeProfit = Math.Round(entity.TakeProfit, 2, MidpointRounding.AwayFromZero);
            entity.Units = Math.Round(entity.Invested / entity.ValueOpen, 2, MidpointRounding.AwayFromZero);
            //entity.BrokerMargin = entity.Cfd_id != 2 ? brokerMG : 0;
            entity.Profit = 0;
            entity.CreatedBy = user_oid;
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsInactive = 0;
            entity.Cfd_id = entity.Cfd_id;
            entity.Market_id = entity.Market_id;
            entity.User_id = user_oid;
            entity.Action = "Open";
            // access db
            _context.Set<Portfolio>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<Portfolio>> FindAllAsync(Expression<Func<Portfolio, bool>> match)
        {
            return await _context.Set<Portfolio>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<Portfolio> GetAsync(params object[] key)
        {
            return await _context.Set<Portfolio>().FindAsync(key);
        }


        //ReadById
        public async Task<Portfolio> FindAsync(Expression<Func<Portfolio, bool>> match)
        {
            try
            {
                return await _context.Set<Portfolio>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        //Update
        public async Task<Portfolio> UpdateAsync(Portfolio entity, int user_oid)
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


        //update
        protected void ChangeEntityState(Portfolio entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Portfolio>().Attach(entity);
            entry.State = state;
        }

       



    }
}
