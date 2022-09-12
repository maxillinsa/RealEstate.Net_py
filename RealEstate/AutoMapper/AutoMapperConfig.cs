using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RealEstate.Models;
using RealEstate.Models.ViewModels;

namespace RealEstate.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AllowNullCollections = true;
                x.AddProfile< MappingProfile>();
            });
        }
    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EstateViewModel, Estate>().ReverseMap();
            
        }
    }
}