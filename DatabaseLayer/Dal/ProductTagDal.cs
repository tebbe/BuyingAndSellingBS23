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
        
        //test
        public async Task<List<Dictionary<string, object>>> GetAsync(string collectionName, string name = "", string tagName = "")
        {
            try
            {
                var _productTagCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Product>(collectionName);

                BsonDocument filter = new BsonDocument();

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(tagName))
                {
                    filter = new BsonDocument{ { "$match",new BsonDocument("$or", new BsonArray().Add(new BsonDocument("Name", new BsonDocument("$regex",name.Trim() ).Add("$options", "i")))
                    .Add(new BsonDocument("TagName", new BsonDocument("$regex", tagName.Trim() ).Add("$options", "i"))))} };
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("Name", new BsonDocument("$regex", name.Trim()).Add("$options", "i")) } };
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("TagName", new BsonDocument("$regex", tagName.Trim()).Add("$options", "i")) } };
                }
                var pipeline = new BsonDocument[] {
                   filter,
                    new BsonDocument{ { "$project", new BsonDocument {
                        {"_id",0}
                    }}}
                };
                var m = await _productTagCollection.AggregateAsync<Dictionary<string, object>>(pipeline);
                var result = await m.ToListAsync();
                return result.ToList();
            }
            catch
            {
                throw;
            }

        }
        public async Task<bool> UpdateAsync(string collectionName, List<ProductTag> productTagList)
        {
            try
            {
                string productId = productTagList.Count > 0 ? productTagList.FirstOrDefault().ProductId : "";
                await DeleteAsync(collectionName, productId);
                if (productTagList.Count > 0)
                {
                    var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<ProductTag>(collectionName);

                    await _collection.InsertManyAsync(productTagList);

                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        //for existing checking
        public async Task<bool> IsExist(string collectionName, string tagName, string productId, string did = "")
        {
            var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string, object>>(collectionName);
            bool isExist = false;
            BsonDocument filter = new BsonDocument();
            if (!String.IsNullOrEmpty(did))
            {//for put operation
                filter = new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("Did",new BsonDocument("$ne", did)))
                    .Add(new BsonDocument("TagName",new BsonDocument("$regex", "^"+tagName.Trim()+"$").Add("$options", "i")))
                    .Add(new BsonDocument("ProductId",new BsonDocument("$regex", "^"+productId.Trim()+"$").Add("$options", "i")))
                    ) } };
            }
            else
            {//for post operation
                filter = new BsonDocument { { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("TagName",new BsonDocument("$regex", "^"+tagName.Trim()+"$").Add("$options", "i")))
                    .Add(new BsonDocument("ProductId",new BsonDocument("$regex", "^"+productId.Trim()+"$").Add("$options", "i")))
                    )}};
            }


            var pipeline = new BsonDocument[] {
                    filter
                };
            var result = await _collection.AggregateAsync<Dictionary<string, object>>(pipeline);
            if (result.FirstOrDefault() != null)
            {
                isExist = true;
            }
            return isExist;
        }
        private async Task DeleteAsync(string collectionName, string productId)
        {
            var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string, object>>(collectionName);
            if (string.IsNullOrEmpty(productId))
            {
                var filter = Builders<Dictionary<string, object>>.Filter.Eq("ProductId", productId);
                await _collection.DeleteManyAsync(filter);
            }
            else
            {
                await _collection.DeleteManyAsync("{ }");
            }


        }

    }
}
