using ecommerse_api.Common.Infastracture;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Shared;
using MediatR;

namespace ecommerse_api.Features.Orders.Command.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<Guid>>
    {

        private  readonly ApplicationDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;



        public DeleteOrderCommandHandler(ApplicationDbContext dbContext, IOrderRepository orderRepository, ICartItemRepository cartItemRepository)
        { 
            _dbContext = dbContext;
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            
        }

        public async Task<Result<Guid>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Delete order request cannot be null.");
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var cartItems = await _cartItemRepository.GetAllByOrderAsync(request.Id);

                    foreach (var item in cartItems)
                    {
                        await _cartItemRepository.DeleteAysnc(item.Id);
                    }

                    await _orderRepository.DeleteAsync(request.Id);

                    await transaction.CommitAsync(cancellationToken);

                    return Result.Success(request.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    Console.WriteLine(ex.ToString());
                    return Result.Failure<Guid>(new Error("DeleteOrder.Failed", "An error occurred while deleting the order."));
                }
            }
        }
    }
}
