using MediatR;
using Microsoft.VisualBasic;
using Model;
using Model.QueryString;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Commands
{
    public class UpdateBlockUserCommand:IRequest<bool>
    {
        public string Did { get; set; }
    }
}
