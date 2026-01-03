using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.OrderResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.OrderService;

public class OrderQueryService(
    INotificationHandler notification,
    IValidate<Order> validate,
    ILoggerHandler logger,
    IOrderRepository orderRepository,
    IOrderMapper orderMapper)
    : ServiceBase<Order>(notification, validate, logger), IOrderQueryService
{
    public async Task<OrderResponse?> GetOrderByIdAsync(long orderId)
    {
        var order = await orderRepository.GetOrderWithItemsAsync(orderId);
        return order != null ? orderMapper.DomainToResponse(order) : null;
    }

    public async Task<OrderResponse?> GetOrderByNumberAsync(string orderNumber)
    {
        var order = await orderRepository.FindByPredicateAsync(
            x => x.OrderNumber == orderNumber,
            include: q => q.Include(o => o.Items).ThenInclude(i => i.Product));
        
        return order != null ? orderMapper.DomainToResponse(order) : null;
    }

    public async Task<PageList<OrderResponse>> GetUserOrdersAsync(Guid userId, PageParams pageParams)
    {
        Expression<Func<Order, bool>> predicate = x => x.UserId == userId;
        
        var orders = await orderRepository.FindAllWithPaginationAsync(
            pageParams,
            predicate,
            include: q => q.Include(o => o.Items).ThenInclude(i => i.Product));

        return orderMapper.DomainToPaginationResponse(orders);
    }
}