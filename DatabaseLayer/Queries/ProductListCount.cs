using MediatR;
using Model.QueryString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Queries
{
    public class ProductListCount:IRequest<long> 
    {
        public string? Name { get; set; } 
        public string? TagName { get; set; }   
    }
}
