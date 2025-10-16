using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    { //// keladigan ma'lumotni qaysi modelga o'tkazish kerakligini aytib beramiz
        CreateMap<Restaurant, RestaurantListDto >()
            .ForMember(l => l.RestaurantId, opt => opt.MapFrom(x => x.Id))
            .ForMember(l => l.RestaurantName, opt => opt.MapFrom(x => x.Name));
 
        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(d => d.Address, opt => opt.MapFrom(
                src => new Address
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode
                }));

        CreateMap<UpdateRestaurantDto, Restaurant>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom((src, dest) =>
            {
                if (dest.Address == null)
                    dest.Address = new Address();

                return new Address
                {
                    City = string.IsNullOrEmpty(src.City) ? dest.Address.City : src.City,
                    Street = string.IsNullOrEmpty(src.Street) ? dest.Address.Street : src.Street,
                    PostalCode = string.IsNullOrEmpty(src.PostalCode) ? dest.Address.PostalCode : src.PostalCode
                };
            }))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(d=>d.City, opt=>
                opt.MapFrom(scr=>scr.Address ==null? null:scr.Address.City))
            .ForMember(d => d.Street, opt =>
                opt.MapFrom(scr => scr.Address == null ? null : scr.Address.Street))
            .ForMember(d => d.PostalCode, opt =>
                opt.MapFrom(scr => scr.Address == null ? null : scr.Address.PostalCode))
            .ForMember(d => d.Dishes, opt =>
                opt.MapFrom(scr => scr.Dishes));
    }
}
