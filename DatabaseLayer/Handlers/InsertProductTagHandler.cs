using DatabaseLayer.Commands;
using DatabaseLayer.Dal;
using DatabaseLayer.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Model;
using Model.DBModel;
using PremiseGlobalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Handlers
{
    public class InsertProductTagHandler:IRequestHandler<InsertProductTagCommand, bool>
    {
        private ProductTagDal _productTagDal;   
        private IOptions<DBCollections> _dbCollections;

        public InsertProductTagHandler(IOptions<DBCollections> dbCollections, ProductTagDal productTagDal)
        {
            _dbCollections = dbCollections;
            _productTagDal = productTagDal;
        }

        public async Task<bool> Handle(InsertProductTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _productTagDal.SaveAsync(_dbCollections.Value.ProductTag, request.ProductTagList);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
