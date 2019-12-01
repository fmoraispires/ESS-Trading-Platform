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
    public interface IAssetService
    {
        Task<ICollection<Asset>> GetAllAsync();
        Task<Asset> FindAsync(Expression<Func<Asset, bool>> match);
        Task<Asset> AddAsync(Asset entity, int user_oid);
        Task<Asset> UpdateAsync(Asset entity, int user_oid);
        Task<Asset> GetAsync(params object[] key);
        Task<ICollection<Asset>> FindAllAsync(Expression<Func<Asset, bool>> match);
    }

    //public delegate void EventHandler();

    public class AssetService : IAssetService
    { 
        //database
        private DataContext _context;
        private IMapper _mapper;
        


        public AssetService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }



        //ReadAll
        public async Task<ICollection<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }



        //Create
        public async Task<Asset> AddAsync(Asset entity, int user_oid)
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
            _context.Set<Asset>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }


        public async Task<ICollection<Asset>> FindAllAsync(Expression<Func<Asset, bool>> match)
        {
            return await _context.Set<Asset>().Where(match).ToListAsync();
        }


        //ReadById
        public async Task<Asset> GetAsync(params object[] key)
        {
            return await _context.Set<Asset>().FindAsync(key);
        }


        //ReadById
        public async Task<Asset> FindAsync(Expression<Func<Asset, bool>> match)
        {
            try
            {
                return await _context.Set<Asset>().FirstOrDefaultAsync(match);
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //Update
        public async Task<Asset> UpdateAsync(Asset entity, int user_oid)
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


        protected void ChangeEntityState(Asset entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<Asset>().Attach(entity);
            entry.State = state;
        }

    }

}
