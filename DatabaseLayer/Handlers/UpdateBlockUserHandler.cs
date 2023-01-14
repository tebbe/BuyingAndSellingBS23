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
    public class UpdateBlockUserHandler : IRequestHandler<UpdateBlockUserCommand, bool>
    {
        private MessageDal _messageDal;
        private IOptions<DBCollections> _dbCollections;

        public UpdateBlockUserHandler(IOptions<DBCollections> dbCollections, MessageDal messageDal)
        {
            _dbCollections = dbCollections;
            _messageDal = messageDal;   
        }

        public async Task<bool> Handle(UpdateBlockUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _messageDal.UpdateAsync(_dbCollections.Value.Message, request.Did,request.IsBuyerBlock);
            }
            catch
            {
                throw;
            }
        }
    }
}
