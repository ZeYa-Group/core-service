using AutoMapper;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.Schemas.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserContactEntity, UserModel>();
            CreateMap<RegisterRequestModel, UserModel>()
                .ForMember(mem => mem.Roles, opt => opt.Ignore());
        }
    }
}
