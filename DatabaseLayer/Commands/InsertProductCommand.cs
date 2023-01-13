using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Commands
{
    public class InsertProductCommand:IRequest<string>
    {
        public Product ProductModel { get; set; }
        public InsertProductCommand(string userId, Product model)
        {
            ProductModel = model;
            ProductModel.Did = Guid.NewGuid().ToString("N");
            ProductModel.CreatedBy = userId;
            ProductModel.CreatedDate = DateTime.Now;

        }
    }
}
