using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dishes.Dtos;

public class DishesProfile: Profile
{
    public DishesProfile()
    {
        // dish dan dishdto ga map qilamiz
        CreateMap<Dish, DishListDto>()
            .ForMember(l => l.DishId, opt => opt.MapFrom(x => x.Id))
            .ForMember(l => l.DishName, opt => opt.MapFrom(x => x.Name));
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishDto, Dish>();
        CreateMap<UpdateDishDto, Dish>()
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
    }
}
