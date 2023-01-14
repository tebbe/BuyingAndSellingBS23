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
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] MessageQueryModel queryModel)
        {
            var returnData = new ApiResponseSuccess<Dictionary<string, object>>();

            var data = await _mediator.Send(new GetMessageList { ProductId = queryModel.ProductId, SellerId = queryModel.SellerId, BuyerId = queryModel.BuyerId });
            returnData.statusCode = StatusCodes.Status200OK;
            returnData.status = "success";
            returnData.message = data == null ? "No message found" : "";
            returnData.data = data;
            return StatusCode(StatusCodes.Status200OK, returnData);

        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] MessageMappingModel messageMapping)
        {
            var returnData = new ApiResponseSuccess<Dictionary<string, object>>();
            var did = "";
            if (!IsFirstMessageExist(messageMapping.ProductId, messageMapping.BuyerId, messageMapping.SellerId).Result)
            {
                did = await _mediator.Send(new InsertMessageCommand(messageMapping));
            }
            else
            {
                did = messageMapping.MessageId;
            }

            if (!string.IsNullOrEmpty(did))
            {
                await _mediator.Send(new InsertMessageDetailCommand(did, messageMapping.Message));
            }
            returnData.statusCode = StatusCodes.Status200OK;
            returnData.status = "success";
            returnData.message = "Message has been saved successfully";
            returnData.data = new Dictionary<string, object> { { "Did", did } };
            return StatusCode(StatusCodes.Status201Created, returnData);

        }

        [HttpPost]
        [Route("blockbuyer/{did}/{isblock}")]
        public async Task<IActionResult> PutAsync(string did,bool isblock)
        {
            var returnData = new ApiResponseSuccess<bool>();

            bool data =await _mediator.Send(new UpdateBlockUserCommand { Did = did,IsBuyerBlock=isblock });

            returnData.statusCode = StatusCodes.Status200OK;
            returnData.status = "success";
            returnData.message = data? "Buyer has been blocked successfully":"";
            returnData.data = data;
            return StatusCode(StatusCodes.Status201Created, returnData);

        }

        private async Task<bool> IsFirstMessageExist(string productId, string buyerId, string sellerId)
        {
            var data = await _mediator.Send(new IsMessageEntryExist { ProductId = productId, BuyerId = buyerId, SellerId = sellerId });
            return data;
        }
    }
}
