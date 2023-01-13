using MediatR;
using Model.QueryString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Queries
{
    public class GetProductList:IRequest<List<Dictionary<string,object>>>
    {
        public string? UserId { get; set; }
        public string? Name { get; set; } 
        public string? TagName { get; set; }   
    }
}
