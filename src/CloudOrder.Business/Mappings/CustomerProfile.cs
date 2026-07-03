using AutoMapper;
using CloudOrder.Business.DTOs.Customers;
using CloudOrder.Entities.Entities;

namespace CloudOrder.Business.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        //request->entity
        CreateMap<CreateCustomerRequestDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        CreateMap<UpdateCustomerRequestDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

        //response->entity
        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}
