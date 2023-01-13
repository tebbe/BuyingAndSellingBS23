using DatabaseLayer.Commands;
using DatabaseLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.QueryString;
using PremiseGlobalLibrary;
using PremiseGlobalLibrary.Model;
using System.Security.Cryptography;

namespace BuyingAndSelling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ProductQueryModel queryModel)
        {
            var returnData = new ApiResponseSuccess<List<Dictionary<string, object>>>();
            string userId = "1";
            var data = await _mediator.Send(new GetProductList { UserId = userId, Name = queryModel.ProductName, TagName = queryModel.Tag });
            returnData.statusCode = StatusCodes.Status200OK;
            returnData.status = "success";
            returnData.message = data.Count == 0 ? "No data found" : "";
            returnData.data = data;
            return StatusCode(StatusCodes.Status200OK, returnData);

        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Product product, [FromBody] List<ProductTag> productTags) 
        {
            var returnData = new ApiResponseSuccess<Dictionary<string, object>>();
            string userId = "1";

            if (!IsExist(product.Name, product.Model).Result)
            {
                var did = await _mediator.Send(new InsertProductCommand(userId, product));

                if(!string.IsNullOrEmpty(did) && productTags.Count>0)
                {
                    //save tag name
                }
                returnData.statusCode = StatusCodes.Status200OK;
                returnData.status = "success";
                returnData.message = "Product has been saved successfully";
                returnData.data = new Dictionary<string, object> { { "Did", did } };
                return StatusCode(StatusCodes.Status201Created, returnData);
            }
            else
            {
                returnData.statusCode = StatusCodes.Status200OK;
                returnData.status = "success";
                returnData.message = "Product already exist";
                returnData.data = new Dictionary<string, object> { { "Did", "" } };
                return StatusCode(StatusCodes.Status200OK, returnData);
            }

        }

        
        private async Task<bool> IsExist(string name, string model)
        {
            var data = await _mediator.Send(new IsProductExist {Name = name, Model = model });
            return data;
        }
    }
}
