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
    public class MessageDetailDal
    {
        private readonly IDBContext _dbContext;

        public MessageDetailDal(IDBContext dbContext)
        {
            _dbContext = dbContext;
            if (!BsonClassMap.IsClassMapRegistered(typeof(MessageDetail)))
            {
                BsonClassMap.RegisterClassMap<MessageDetail>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
       
        public async Task<bool> SaveAsync(string collectionName, MessageDetail messageDetail)
        {
            try
            {
                var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<MessageDetail>(collectionName);

                await _collection.InsertOneAsync(messageDetail);

                return true;
            }
            catch
            {
                throw;
            }
        }
     
    }
}
