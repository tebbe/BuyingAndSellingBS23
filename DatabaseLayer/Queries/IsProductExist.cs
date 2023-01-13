using MediatR;
using Model.QueryString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Queries
{
    public class IsProductExist : IRequest<bool>
    {
        public string Did { get; set; }   
        public string Name { get; set; }   
        public string Model { get; set; }   
    }
}
