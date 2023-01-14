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

        public async Task<List<Dictionary<string, object>>> GetAsync(string tagCollectionName, string collectionName, string name = "", string tagName = "")
        {
            try
            {
                var _productCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Product>(collectionName);
                
                BsonDocument filter = new BsonDocument();

                if (!string.IsNullOrEmpty(name.Trim()) && !string.IsNullOrEmpty(tagName.Trim()))
                {
                    filter = new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("Name", new BsonDocument("$regex",name.Trim() ).Add("$options", "i")))
                    .Add(new BsonDocument("ProductTag.TagName", new BsonDocument("$regex", tagName.Trim() ).Add("$options", "i")))) } };
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("Name", new BsonDocument("$regex", name.Trim()).Add("$options", "i")) } };
                }
                else if (!string.IsNullOrEmpty(tagName))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("ProductTag.TagName", new BsonDocument("$regex", tagName.Trim()).Add("$options", "i")) } };
                }
                else
                {
                    filter = new BsonDocument { {"$match", new BsonDocument("Name", new BsonDocument("$ne", ""))}};
                }
            
                //
                var pipeline = new BsonDocument[] {
                    new BsonDocument("$match", new BsonDocument("IsActive", true)),
                    new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", tagCollectionName.Trim().ToString() },
                            { "localField", "Did" },
                            { "foreignField", "ProductId" },
                            { "as", "ProductTag" }
                        }),
                    new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "_id", 0 },{ "IsActive", 1 },{ "Name", 1 },{ "BrandName", 1 },
                            { "Model", 1 },{ "Addition",1},{ "Detail", 1 },{ "Contact", 1},
                            { "Did", 1},{ "ProductTag", 1}

                        }),
                    filter

                };

                var m = await _productCollection.AggregateAsync<Dictionary<string, object>>(pipeline);
                var result = await m.ToListAsync();
                return result.ToList();
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        public async Task<long> GetTotalCountAsync(string tagCollectionName, string collectionName, string name = "", string tagName = "")
        {
            try
            {
                var _productCollection = _dbContext.GetDataBase<IMongoDatabase>().GetCollection<Dictionary<string, object>>(collectionName);

                BsonDocument filter = new BsonDocument();

                if (!string.IsNullOrEmpty(name.Trim()) && !string.IsNullOrEmpty(tagName.Trim()))
                {
                    filter = new BsonDocument{ { "$match", new BsonDocument("$and", new BsonArray()
                    .Add(new BsonDocument("Name", new BsonDocument("$regex",name.Trim() ).Add("$options", "i")))
                    .Add(new BsonDocument("ProductTag.TagName", new BsonDocument("$regex", tagName.Trim() ).Add("$options", "i")))) } };
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("Name", new BsonDocument("$regex", name.Trim()).Add("$options", "i")) } };
                }
                else if (!string.IsNullOrEmpty(tagName))
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("ProductTag.TagName", new BsonDocument("$regex", tagName.Trim()).Add("$options", "i")) } };
                }
                else
                {
                    filter = new BsonDocument { { "$match", new BsonDocument("Name", new BsonDocument("$ne", "")) } };
                }

                //
                var pipeline = new BsonDocument[] {
                    new BsonDocument("$match", new BsonDocument("IsActive", true)),
                    new BsonDocument("$lookup",
                    new BsonDocument
                        {
                            { "from", tagCollectionName.Trim().ToString() },
                            { "localField", "Did" },
                            { "foreignField", "ProductId" },
                            { "as", "ProductTag" }
                        }),
                    new BsonDocument("$project",
                    new BsonDocument
                        {
                            { "_id", 0 },{ "IsActive", 1 },{ "Name", 1 },{ "BrandName", 1 },
                            { "Model", 1 },{ "Addition",1},{ "Detail", 1 },{ "Contact", 1},
                            { "Did", 1},{ "ProductTag", 1}

                        }),
                    filter

                };



                var m = await _productCollection.AggregateAsync<Dictionary<string, object>>(pipeline);
                return m.ToList().Count;

            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> IsExist(string collectionName, string name, string model, string did = "")
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
    }
}
