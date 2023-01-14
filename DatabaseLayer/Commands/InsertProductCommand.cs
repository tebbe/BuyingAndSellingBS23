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
        public Product ProductModel { get; set; } = new Product();
        public InsertProductCommand(string userId, ProductMappingModel mappingModel)
        {
            ProductModel.Did = Guid.NewGuid().ToString("N");
            ProductModel.Name=mappingModel.Name;
            ProductModel.BrandName= mappingModel.BrandName;
            ProductModel.Model= mappingModel.Model;
            ProductModel.Addition = mappingModel.Addition;
            ProductModel.Contact = mappingModel.Contact;
            ProductModel.Detail = mappingModel.Detail;
            ProductModel.CreatedBy = userId;
            ProductModel.CreatedDate = DateTime.Now;

        }
    }
}
