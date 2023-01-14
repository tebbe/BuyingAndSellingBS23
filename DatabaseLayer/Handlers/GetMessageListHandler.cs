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
    public class GetMessageListHandler : IRequestHandler<GetMessageList, Dictionary<string, object>>
    {
        private MessageDal _messageDal;
        private IOptions<DBCollections> _dbCollections;

        public GetMessageListHandler(IOptions<DBCollections> dbCollections, MessageDal messageDal)
        {
            _dbCollections = dbCollections;
            _messageDal = messageDal;
        }

        public async Task<Dictionary<string, object>> Handle(GetMessageList request, CancellationToken cancellationToken)
        {
            try
            {
                return await _messageDal.GetAsync(_dbCollections.Value.MessageDetail, _dbCollections.Value.Message,request.ProductId,request.SellerId,request.BuyerId);
            }
            catch
            {
                throw;
            }
        }
    }
}
