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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseLayer.Dal
{
    public class MessageDal
    {
        private readonly IDBContext _dbContext;

        public MessageDal(IDBContext dbContext)
        {
            _dbContext = dbContext;
            if (!BsonClassMap.IsClassMapRegistered(typeof(Message)))
            {
                BsonClassMap.RegisterClassMap<Message>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }

        public async Task<Dictionary<string, object>> GetAsync(string messageDetailCollectionName, string collectionName, string productId, string sellerId, string buyerId)
        {
            try
            {
                var _productCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Message>(collectionName);

                BsonDocument filter = new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("ProductId", new BsonDocument("$regex",productId).Add("$options", "i")))
                    .Add(new BsonDocument("BuyerId", new BsonDocument("$regex",buyerId).Add("$options", "i")))
                    .Add(new BsonDocument("SellerId", new BsonDocument("$regex", sellerId).Add("$options", "i")))) } };

                var pipeline = new BsonDocument[] {
                    new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", messageDetailCollectionName.ToString() },
                            { "localField", "Did" },
                            { "foreignField", "MessageId" },
                            { "as", "MessageDetail" }
                        }),
                    new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "_id", 0 },{ "Did", 1 },{ "ProductId", 1 },{ "SellerId", 1 },
                            { "BuyerId", 1 },{ "IsBlockBuyer",1},{ "InsertedDate", 1 },
                            { "MessageDetail", 1}
                        }),
                    filter

                };

                var m = await _productCollection.AggregateAsync<Dictionary<string, object>>(pipeline);

                return m.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> IsExist(string collectionName, string productId, string sellerId, string buyerId = "")
        {
            var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string, object>>(collectionName);
            bool isExist = false;

            var pipeline = new BsonDocument[] {
                    new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("ProductId",new BsonDocument("$eq", productId)))
                    .Add(new BsonDocument("SellerId",new BsonDocument("$eq", sellerId)))
                    .Add(new BsonDocument("BuyerId",new BsonDocument("$eq", buyerId)))

                    ) } }};

            var result = await _collection.AggregateAsync<Dictionary<string, object>>(pipeline);
            if (result.FirstOrDefault() != null)
            {
                isExist = true;
            }
            return isExist;
        }

        public async Task<string> SaveAsync(string collectionName, Message message)
        {
            try
            {
                var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Message>(collectionName);
                await _collection.InsertOneAsync(message);
                return message.Did;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(string collectionName,string did)
        {
            try
            {
                var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Message>(collectionName);

                var filter = Builders<Message>.Filter.Eq(s => s.Did, did);
                var update = Builders<Message>.Update
                  .Set(s => s.IsBlockBuyer, true);

                await _collection.UpdateOneAsync(filter,update);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
