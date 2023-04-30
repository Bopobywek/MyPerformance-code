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

        public void AddOrUpdate(PerformanceModel performance)
        {
            try
            {
                Init();

                var insertions = performance.PerformanceParts.Where(x => x.Id == 0).ToArray();
                if (insertions.Length > 0)
                {
                    _connection.InsertAll(insertions, typeof(PerformancePartModel));
                }

                _connection.UpdateAll(performance.PerformanceParts);

                if (performance.Id == 0)
                {
                    _connection.Insert(performance);
                }

                _connection.UpdateWithChildren(performance);
            }
            catch (Exception ex)
            {
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


        public PerformancePartModel QueryPart(int id)
        {
            Init();
            return _connection.Table<PerformancePartModel>()
                .Where(x => x.Id == id)
                .Single();
        }


        public void DeletePart(PerformancePartModel model)
        {
            Init();
            _connection.Delete(model);
        }

        public void Delete(int id)
        {
            Init();
            const string sqlQuery = "DELETE FROM performance_parts WHERE PerformanceId = ?";
            _connection.Execute(sqlQuery, id);
            const string sqlQuery2 = " DELETE FROM performances WHERE Id = ?";
            _connection.Execute(sqlQuery2, id);
        }
    }
}
