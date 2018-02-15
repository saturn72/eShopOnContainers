using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Commands;
using Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;
using Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure.Services;
using Ordering.API.Application.Commands;
using Product.API.ApiModels;
using Product.API.Services;
using Product.API.ViewModels;

namespace Product.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;
        private readonly IIdentityService _identityService;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, IMediator mediator, IOrderQueries orderQueries, IIdentityService identityService)
        {
            _productService = productService;

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _orderQueries = orderQueries ?? throw new ArgumentNullException(nameof(orderQueries));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(PageViewModel<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            if (pageSize < 0)
                pageSize = 0;
            if (pageIndex < 0)
                pageIndex = 0;

            var allProducts = await _productService.GetProducts(pageSize * pageIndex, pageSize);
            throw new NotImplementedException();
            //var totalItems = await _catalogContext.CatalogItems
            //    .LongCountAsync();

            //var itemsOnPage = await _catalogContext.CatalogItems
            //    .OrderBy(c => c.Name)
            //    .Skip(pageSize * pageIndex)
            //    .Take(pageSize)
            //    .ToListAsync();

            //itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            //var model = new PageViewModel<CatalogItem>(
            //    pageIndex, pageSize, totalItems, itemsOnPage);

            //return Ok(model);
        }

        [Route("cancel")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestCancelOrder);
            }
           
            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();

        }

        [Route("ship")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ShipOrder([FromBody]ShipOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestShipOrder = new IdentifiedCommand<ShipOrderCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestShipOrder);
            }

            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();

        }

        [Route("{orderId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Order),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var order = await _orderQueries
                    .GetOrderAsync(orderId);

                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            var orders = await _orderQueries.GetOrdersAsync();

            return Ok(orders);
        }

        [Route("cardtypes")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCardTypes()
        {
            var cardTypes = await _orderQueries
                .GetCardTypesAsync();

            return Ok(cardTypes);
        }        
    }
}


