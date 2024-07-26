﻿using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Repositories
{
    public class UserRepository: IUserRepository
    {
        public readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void AddUser(User User)
        {
            _context.Users.Add(User);
            _context.SaveChanges();

        }

        public void DeleteUser(int id)
        {
            var User = _context.Users.Find(id);
            _context.Users.Remove(User);
            _context.SaveChanges();

        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.First(l => l.Id == id);
        }

        public void UpdateUser(User User)
        {
            _context.Users.Update(User);
            _context.SaveChanges();

        }








    }
}