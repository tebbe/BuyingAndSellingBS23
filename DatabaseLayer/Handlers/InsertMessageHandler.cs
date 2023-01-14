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
    public class InsertMessageHandler : IRequestHandler<InsertMessageCommand, string>
    {
        private MessageDal _messageDal;
        private IOptions<DBCollections> _dbCollections;

        public InsertMessageHandler(IOptions<DBCollections> dbCollections, MessageDal messageDal)
        {
            _dbCollections = dbCollections;
            _messageDal = messageDal;   
        }

        public async Task<string> Handle(InsertMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _messageDal.SaveAsync(_dbCollections.Value.Message, request.MessageModel);
            }
            catch
            {
                throw;
            }
        }
    }
}
