using AutoMapper;
using CloudOrder.Business.DTOs.Orders;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        //request->entity
        CreateMap<CreateOrderRequestDto, Order>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());


        CreateMap<CreateOrderItemRequestDto, OrderItem>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.OrderId, o => o.Ignore())
            .ForMember(d => d.Order, o => o.Ignore())
            .ForMember(d => d.Product, o => o.Ignore())
            .ForMember(d => d.UnitPrice, o => o.Ignore());

        //entity->response
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));



    }
}
