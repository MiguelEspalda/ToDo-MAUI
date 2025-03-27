using MAUIApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string databaseName)
        {
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, databaseName);
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<TaskModel>().Wait();
        }

        public Task<List<TaskModel>> GetTasksAsync()
        {
            return _database.Table<TaskModel>().ToListAsync();
        }

        public Task<int> SaveTaskAsync(TaskModel task)
        {
            return task.Id == 0 ? _database.InsertAsync(task) : _database.UpdateAsync(task);
        }

        public Task<int> DeleteTaskAsync(TaskModel task)
        {
            return _database.DeleteAsync(task);
        }
    }
}
