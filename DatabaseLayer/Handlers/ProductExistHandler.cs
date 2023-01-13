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
    public class ProductExistHandler : IRequestHandler<IsProductExist,bool>
    {
        private ProductDal _productDal;
        private IOptions<DBCollections> _dbCollections;

        public ProductExistHandler(IOptions<DBCollections> dbCollections, ProductDal productDal)
        {
            _dbCollections = dbCollections;
            _productDal = productDal;
        }

        public async Task<bool> Handle(IsProductExist request, CancellationToken cancellationToken)
        {
            try
            {
                return await _productDal.IsExist(_dbCollections.Value.Product,request.Name,request.Model,request.Did);
            }
            catch
            {
                throw;
            }
        }
    }
}
