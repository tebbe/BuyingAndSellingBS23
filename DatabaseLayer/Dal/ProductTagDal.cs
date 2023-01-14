using DatabaseLayer.DBContext;
using MediatR;
using Model;
using Model.QueryString;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseLayer.Dal
{
    public class ProductTagDal
    {
        private readonly IDBContext _dbContext;

        public ProductTagDal(IDBContext dbContext)
        {
            _dbContext = dbContext;
            if (!BsonClassMap.IsClassMapRegistered(typeof(ProductTag)))
            {
                BsonClassMap.RegisterClassMap<ProductTag>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
       
        public async Task<bool> SaveAsync(string collectionName, List<ProductTag> productTagList)
        {
            try
            {
                var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<ProductTag>(collectionName);

                await _collection.InsertManyAsync(productTagList);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
