using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MyPerformance.Models;
using SQLiteNetExtensions.Extensions;

namespace MyPerformance.Repositories
{
    public class PerformancesRepository
    {
        private SQLiteConnection _connection;
        private string _dbPath;

        public PerformancesRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        private void Init()
        {
            if (_connection != null)
                return;

            _connection = new SQLiteConnection(_dbPath);
            _connection.CreateTable<PerformanceModel>();
            _connection.CreateTable<PerformancePartModel>();
        }

        public void Add(PerformanceModel performance) {
            try
            {
                Init();

                _connection.Insert(performance);
                _connection.InsertAll(performance.PerformanceParts);

                _connection.UpdateWithChildren(performance);
            }
            catch (Exception ex) {
                throw;
            }
        }

        public PerformanceModel[] QueryAll()
        {
            try
            {
                Init();
                return _connection.Table<PerformanceModel>().ToArray();
            }
            catch (Exception ex)
            {
                return Array.Empty<PerformanceModel>();
            }
        }

        public PerformanceModel Query(int id)
        {
            Init();
            return _connection.GetWithChildren<PerformanceModel>(id);
        }
    }
}
