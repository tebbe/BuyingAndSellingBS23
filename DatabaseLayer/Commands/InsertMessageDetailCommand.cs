using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Commands
{
    public class InsertMessageDetailCommand : IRequest<bool>
    {
        public MessageDetail MessageDetail { get; set; }
        public InsertMessageDetailCommand(string messageId, string message)
        {

            MessageDetail = new MessageDetail
            {
                Did = Guid.NewGuid().ToString("N"),
                MessageId = messageId,
                Message = message,
                InsertedDate = DateTime.Now
            };
        }
    }
}
