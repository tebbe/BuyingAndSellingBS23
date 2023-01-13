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
    public class ProductDal
    {
        private readonly IDBContext _dbContext;

        public ProductDal(IDBContext dbContext) 
        {
            _dbContext = dbContext;
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }

        public async Task<List<Dictionary<string, object>>> GetAsync(string tagCollectionName,string collectionName, string name="",string tagName="")
        {
            try
            {
                var _productCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Product>(collectionName);
                //for serch operation document mention about "and" query but i impemented "or" query because 
                //can be search using product name or tagname
                BsonDocument filter = new BsonDocument { { "$match", new BsonDocument("$and", new BsonArray().Add(new BsonDocument("IsActive", true))) } };

               if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(tagName))
                {
                    //filter = new BsonDocument{ { "$match",
                    //new BsonDocument("$or", new BsonArray().Add(new BsonDocument("Name", new BsonDocument("$regex",name ).Add("$options", "i")))
                    //.Add(new BsonDocument("TagName", new BsonDocument("$regex", tagName ).Add("$options", "i")))) } };

                    new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray().Add(new BsonDocument("IsActive",true))
                    .Add(new BsonDocument("$or", new BsonArray().Add(new BsonDocument("Name", new BsonDocument("$regex",name ).Add("$options", "i")))
                    .Add(new BsonDocument("TagName", new BsonDocument("$regex", tagName ).Add("$options", "i")))))) } };
                };

                var pipeline = new BsonDocument[] {
                    new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", tagCollectionName.ToString() },
                            { "localField", "Did" },
                            { "foreignField", "ProductId" },
                            { "as", "results" }
                        }),

                    new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "Name", 1 },
                            { "BrandName", 2 },
                            { "Model", 3 },
                            { "Addition", 4 },
                            { "Detail", 5 },
                            { "Contact", 6 },
                            { "Did", 7},
                            { "TagName", "$results.TagName" }
                        }),
                    filter

                };


                var m = await _productCollection.AggregateAsync<Dictionary<string, object>>(pipeline);
                var result = await m.ToListAsync();
                return result.ToList();
            }
            catch
            {
                throw;
            }

        }

        public async Task<long> GetTotalCountAsync(string tagCollectionName, string collectionName,string name="",string tagName="")
        {
            try
            {
                var _productCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string,object>>(collectionName);
                //for serch operation document mention about "and" query but i impemented "or" query because 
                //can be search using product name or tagname
                BsonDocument filter = new BsonDocument { };

                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(tagName))
                {

                    new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray().Add(new BsonDocument("IsActive",true))
                    .Add(new BsonDocument("$or", new BsonArray().Add(new BsonDocument("Name", new BsonDocument("$regex",name ).Add("$options", "i")))
                    .Add(new BsonDocument("TagName", new BsonDocument("$regex", tagName ).Add("$options", "i")))))) } };
                };

                var pipeline = new BsonDocument[] {
                    new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", tagCollectionName.ToString() },
                            { "localField", "Did" },
                            { "foreignField", "ProductId" },
                            { "as", "results" }
                        }),

                    new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "Name", 1 },
                            { "BrandName", 2 },
                            { "Model", 3 },
                            { "Addition", 4 },
                            { "Detail", 5 },
                            { "Did", 6}
                            //{ "TagName", "$results.TagName" }
                        }),
                    filter

                };


                var m = await _productCollection.AggregateAsync<Dictionary<string,object>>(pipeline);
                return m.ToList().Count;

            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> IsExist(string collectionName, string name,string model, string did = "")
        {
            var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string, object>>(collectionName);
            bool isExist = false;
            BsonDocument filter = new BsonDocument();
            if (!String.IsNullOrEmpty(did))
            {
                filter = new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("Did",new BsonDocument("$ne", did)))
                    .Add(new BsonDocument("Model",new BsonDocument("$regex", "^"+model.Trim()+"$").Add("$options", "i")))
                    .Add(new BsonDocument("Name",new BsonDocument("$regex", "^"+name.Trim()+"$").Add("$options", "i")))
                    ) } };
            }
            else
            {
                filter = new BsonDocument { { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("Model",new BsonDocument("$regex", "^"+model.Trim()+"$").Add("$options", "i")))
                    .Add(new BsonDocument("Name",new BsonDocument("$regex", "^"+name.Trim()+"$").Add("$options", "i")))
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

        public async Task<string> SaveAsync(string collectionName, Product product)
        {
            try
            {
                var _collection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Product>(collectionName);
                await _collection.InsertOneAsync(product);
                return product.Did;
            }
            catch
            {
                throw;
            }
        }

        //public async Task<string> UpdateAsync(string collectionName, Product product)
        //{
        //    try
        //    {
        //        var _rolesCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Product>(collectionName);
        //        var filter = Builders<Product>.Filter.Eq(s => s.Id, product.Id);

        //        var update = Builders<Product>.Update
        //            .Set(m => m.Name, product.Name)
        //            .Set(m => m.BrandName, product.BrandName)
        //            .Set(m => m.Addition, product.Addition)
        //            .Set(m => m.Model, product.Model)
        //            .Set(m => m.Detail, product.Detail)
        //            .Set(m => m.IsUsed, product.IsUsed)
        //            .Set(m => m.Contact, product.Contact)
        //            .Set(m => m.UpdateBy, product.UpdateBy)
        //            .Set(m => m.UpdateDate, product.UpdateDate);
                  

        //        await _rolesCollection.UpdateOneAsync(filter, update);
        //        return product.Id;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

    }
}
