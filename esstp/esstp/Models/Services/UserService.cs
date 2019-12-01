using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using esstp.Models;
using Microsoft.EntityFrameworkCore;

namespace esstp.Models
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<ICollection<User>> GetAllAsync();
        Task<User> GetAsync(params object[] key);
        Task<User> FindAsync(Expression<Func<User, bool>> match);
        Task<ICollection<User>> FindAllAsync(Expression<Func<User, bool>> match);
        Task<User> AddAsync(User entity, string password, int user_oid);
        Task<User> UpdatePasswordAsync(User entity, string password, int user_oid);
        Task<User> UpdateAsync(User entity, int user_oid);
        Task<int> DeleteAsync(User entity);
    }


    public class UserService : IUserService
    {
        //database
        private DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }


        //Authenticate
        public async Task<User> Authenticate(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await FindAsync(j => j.UserName == username);

            // check if username exists or is inactive
            if (user == null || user.IsInactive ==1)
                return null;
                

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }


        //Create
        public async Task<User> AddAsync(User entity, string password, int user_oid)
        {
          
            // validation
            var dbItem = await FindAllAsync(j => j.UserName == entity.UserName || j.Email == entity.Email || j.Nif == entity.Nif);       
            if (dbItem == null || string.IsNullOrWhiteSpace(password) || dbItem.Count > 0)
                return null;

            // logic
            entity.Id = 0; //reset oid
            entity.Role_id = 2; //Regular user
            CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordsalt);

            entity.PasswordHash = passwordhash;
            entity.PasswordSalt = passwordsalt;
            entity.CreatedBy = user_oid;
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedBy  = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.IsInactive = 0;
            // access db
            _context.Set<User>().Add(entity);

            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }



        //ReadById
        public async Task<User> GetAsync(params object[] key)
        {
            return await _context.Set<User>().FindAsync(key);
        }

        //ReadByUsername
        public async Task<User> FindAsync(Expression<Func<User, bool>> match)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(match);
        }

        public async Task<ICollection<User>> FindAllAsync(Expression<Func<User, bool>> match)
        {
            return await _context.Set<User>().Where(match).ToListAsync();
        }

        //ReadAll
        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }


        //Update
        public async Task<User> UpdatePasswordAsync(User entity, string password, int user_oid)
        {
            // validation
            var dbItem = await FindAllAsync(j => (j.UserName == entity.UserName || j.Email == entity.Email || j.Nif == entity.Nif) && j.Id != entity.Id);
            if (dbItem == null || string.IsNullOrWhiteSpace(password) || dbItem.Count > 0)
                return null;

            // logic
            CreatePasswordHash(password, out byte[] passwordhash, out byte[] passwordsalt);

            entity.PasswordHash = passwordhash;
            entity.PasswordSalt = passwordsalt;

            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;

            ChangeEntityState(entity, EntityState.Modified);
            await _context.SaveChangesAsync();
            return entity;
        }


        //Update
        public async Task<User> UpdateAsync(User entity, int user_oid)
        {
            // validation
            var dbItem = await FindAllAsync(j => (j.UserName == entity.UserName || j.Email == entity.Email || j.Nif == entity.Nif) && j.Id != entity.Id);
            if (dbItem == null || dbItem.Count > 0)
                return null;

            //logic
            entity.UpdatedBy = user_oid;
            entity.UpdatedDate = DateTime.UtcNow;

            ChangeEntityState(entity, EntityState.Modified);
            await _context.SaveChangesAsync();
            return entity;
        }




        protected void ChangeEntityState(User entity, EntityState state)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Set<User>().Attach(entity);
            entry.State = state;
        }


        //Delete
        public async Task<int> DeleteAsync(User entity)
        {
            _context.Set<User>().Remove(entity);
            return await _context.SaveChangesAsync();
        }


        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
               
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
        
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));

            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }


            return true;
        }
       


    }
}
