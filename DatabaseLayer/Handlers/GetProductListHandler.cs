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
    public class GetProductListHandler : IRequestHandler<GetProductList, List<Dictionary<string, object>>>
    {
        private ProductDal _productDal;
        private IOptions<DBCollections> _dbCollections;

        public GetProductListHandler(IOptions<DBCollections> dbCollections, ProductDal productDal)
        {
            _dbCollections = dbCollections;
            _productDal = productDal;
        }

        public async Task<List<Dictionary<string, object>>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            try
            {
                return await _productDal.GetAsync(_dbCollections.Value.ProductTag, _dbCollections.Value.Product, request.Name,request.TagName);
            }
            catch
            {
                throw;
            }
        }
    }
}
