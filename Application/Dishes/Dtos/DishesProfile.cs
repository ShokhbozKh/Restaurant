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
        CreateMap<Dish, DishDto>();
    }
}
