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
    public class InsertMessageDetailHandler: IRequestHandler<InsertMessageDetailCommand, bool>
    {
        private MessageDetailDal _messageDetailDal;   
        private IOptions<DBCollections> _dbCollections;

        public InsertMessageDetailHandler(IOptions<DBCollections> dbCollections, MessageDetailDal messageDetailDal)
        {
            _dbCollections = dbCollections;
            _messageDetailDal = messageDetailDal; 
        }

        public async Task<bool> Handle(InsertMessageDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _messageDetailDal.SaveAsync(_dbCollections.Value.MessageDetail, request.MessageDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
