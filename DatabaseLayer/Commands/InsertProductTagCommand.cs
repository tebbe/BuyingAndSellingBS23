using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Commands
{
    public class InsertProductTagCommand:IRequest<bool>
    {
        public List<ProductTag> ProductTagList { get; set; }=new List<ProductTag>();
        public InsertProductTagCommand(string userId,string productId, List<ProductTag> dataList) 
        {
            foreach (var model in dataList)
            {
                ProductTag ProductTag = new ProductTag
                {
                    Did = Guid.NewGuid().ToString("N"),
                    ProductId = productId,
                    TagName = model.TagName,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now
                };
                
                ProductTagList.Add(ProductTag);
            }
            
        }
    }
}
