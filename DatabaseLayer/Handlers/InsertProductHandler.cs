using DatabaseLayer.Commands;
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
    public class InsertProductHandler : IRequestHandler<InsertProductCommand, string>
    {
        private ProductDal _productDal;
        private IOptions<DBCollections> _dbCollections;

        public InsertProductHandler(IOptions<DBCollections> dbCollections, ProductDal productDal)
        {
            _dbCollections = dbCollections;
            _productDal = productDal;
        }

        public async Task<string> Handle(InsertProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _productDal.SaveAsync(_dbCollections.Value.Product, request.ProductModel);
            }
            catch
            {
                throw;
            }
        }
    }
}
