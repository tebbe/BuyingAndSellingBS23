using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Commands
{
    public class InsertMessageCommand:IRequest<string>
    {
        public Message MessageModel { get; set; } = new Message();
        public InsertMessageCommand(MessageMappingModel mappingModel)
        {
            MessageModel.Did = Guid.NewGuid().ToString("N");
            MessageModel.SellerId=mappingModel.SellerId;//product post CreatedBy
            MessageModel.BuyerId=mappingModel.BuyerId;
            MessageModel.ProductId=mappingModel.ProductId;
            MessageModel.InsertedDate = DateTime.Now;
        }
    }
}
