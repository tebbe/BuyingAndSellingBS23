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
    public class MessageExistHandler : IRequestHandler<IsMessageEntryExist,bool>
    {
        private MessageDal _messageDal;
        private IOptions<DBCollections> _dbCollections;

        public MessageExistHandler(IOptions<DBCollections> dbCollections, MessageDal messageDal)
        {
            _dbCollections = dbCollections;
            _messageDal = messageDal;
        }

        public async Task<bool> Handle(IsMessageEntryExist request, CancellationToken cancellationToken)
        {
            try
            {
                return await _messageDal.IsExist(_dbCollections.Value.Message,request.ProductId,request.SellerId,request.BuyerId);
            }
            catch
            {
                throw;
            }
        }
    }
}
