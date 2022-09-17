using AutoMapper;
using ServiceAutomation.Canvas.WebApi.Extensions;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceAutomation.Canvas.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserModel>()
                .ForMember(x => x.InviteCode, opt => opt.MapFrom(x => x.InviteReferral));
            CreateMap<RegisterRequestModel, UserModel>()
                .ForMember(x => x.InviteCode, opt => opt.MapFrom(x => x.ReferralCode));
            CreateMap<RefreshTokenEntity, RefreshToken>();
            CreateMap<ThumbnailTemplateEntity, ThumbnailResponseModel>();
            CreateMap<WithdrawTransactionEntity, WithdrawResponseModel>();
            CreateMap<VideoLessonTemplateEntity, VideoLessonResponseModel>();

            CreateMap<UserContactEntity, UserProfileResponseModel>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.User.Email))
                .ForMember(x => x.ProfilePhoto, opt => opt.MapFrom(x => x.User.ProfilePhoto.Data))
                .ForMember(x => x.PersonalReferral, opt => opt.MapFrom(x => x.User.PersonalReferral));

            CreateMap<PackageBonusAssociationEntity, BonusModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Bonus.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.GetPackageBonusTitle()));

            CreateMap<PackageEntity, PackageModel>()
                .ForMember(x => x.Bonuses, opt => opt.MapFrom(x => x.PackageBonuses.OrderBy(pb => pb.Bonus.DisplayOrder)));
        }
    }
}
