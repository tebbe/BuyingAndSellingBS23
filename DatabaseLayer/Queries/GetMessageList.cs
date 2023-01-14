using MediatR;
using Model.QueryString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Queries
{
    public class GetMessageList:IRequest<Dictionary<string,object>>
    {
        public string? ProductId { get; set; }
        public string? SellerId { get; set; } 
        public string? BuyerId { get; set; }      
    }
}
