using DatabaseLayer.Dal;
using DatabaseLayer.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Model.DBModel;
using PremiseGlobalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Handlers
{
    public class ProductListCountHandler : IRequestHandler<ProductListCount,long>
    {
        private ProductDal _productDal;
        private IOptions<DBCollections> _dbCollections;

        public ProductListCountHandler(IOptions<DBCollections> dbCollections, ProductDal productDal)
        {
            _dbCollections = dbCollections;
            _productDal = productDal;
        }

        public async Task<long> Handle(ProductListCount request, CancellationToken cancellationToken)
        {
            try
            {
                return await _productDal.CountAsync(_dbCollections.Value.ProductTag, _dbCollections.Value.Product,request);
            }
            catch
            {
                throw;
            }
        }
        
    }
}
