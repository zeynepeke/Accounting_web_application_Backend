using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderDetailDto> orderDetailsDto)
        {
            var result = await _orderService.CreateOrderAsync(orderDetailsDto);
            if (result == "Sipariş başarıyla oluşturuldu.")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if (result == "Sipariş başarıyla silindi.")
            {
                return Ok(result);
            }

            return NotFound(result);
        }
    }
}
