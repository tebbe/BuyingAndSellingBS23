using Microsoft.Extensions.Options;
using Model.DBModel.MongoDBModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.DBContext
{
    public class MongoDBContext : IDBContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        public MongoDBContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(
               mongoDBSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(
                mongoDBSettings.Value.DatabaseName);
        }

        public T GetDataBase<T>()
        {
            return (T) _mongoDatabase;
        }
    }
}
