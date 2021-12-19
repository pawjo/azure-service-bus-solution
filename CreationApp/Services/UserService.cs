using CreationApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMessagingService _messagingService;

        public UserService(DataContext dataContext, IMessagingService messagingService)
        {
            _dataContext = dataContext;
            _messagingService = messagingService;
        }

        public async Task<bool> AddAsync(User newUser)
        {
            _dataContext.Users.Add(newUser);
            var added = await _dataContext.SaveChangesAsync();

            if (added == 1)
            {
                await _messagingService.SendMessageAsync("User created " + DateTime.Now);
                return true;
            }

            return false;
        }

        public async Task<List<User>> GetActiveListAsync()
        {
            return await _dataContext.Users.Where(x => x.Active).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetListAsync()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<bool> UpdateAsync(User modifiedUser)
        {
            var storedUser = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == modifiedUser.Id);

            if (storedUser == null)
            {
                return false;
            }

            storedUser.Email = modifiedUser.Email;
            storedUser.Name = modifiedUser.Name;
            storedUser.Name = modifiedUser.Name;
            storedUser.Surname = modifiedUser.Surname;
            storedUser.Active = modifiedUser.Active;

            var updated = await _dataContext.SaveChangesAsync();

            if (updated == 1)
            {
                await _messagingService.SendMessageAsync("User edited " + DateTime.Now);
                return true;
            }

            return false;
        }
    }
}
