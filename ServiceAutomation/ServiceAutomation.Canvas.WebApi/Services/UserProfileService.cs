using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Threading.Tasks;
using static ServiceAutomation.Canvas.WebApi.Constants.Requests;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public UserProfileService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<UserProfileResponseModel> GetUserInfo(Guid userId)
        {
            var user = await dbContext.UserContacts
                .Include(x => x.User)
                .ThenInclude(x=>x.ProfilePhoto)
                .FirstOrDefaultAsync(x=> x.UserId == userId);

            return mapper.Map<UserProfileResponseModel>(user);
        }

        public async Task<ResultModel> UploadProfilePhoto(Guid userId, byte[] data)
        {
            var response = new ResultModel();
            var photo = await dbContext.ProfilePhotos.FirstOrDefaultAsync(x => x.UserId == userId);

            if(photo == null)
            {
                photo = new ProfilePhotoEntity()
                {
                    Data = data,
                    UserId = userId
                };

                try
                {
                    await dbContext.ProfilePhotos.AddAsync(photo);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex.Message);
                    response.Success = false;
                }
            }

            try
            {
                photo.Data = data;
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Success = false;
            }

            response.Success = response.Errors != null ? false :  true;

            return response;
        }

        public async Task<ResultModel> UploadProfileInfo(UploadUserProfileRequestModel requestModel)
        {
            var response = new ResultModel();

            var userProfileData = await dbContext.UserContacts.FirstOrDefaultAsync(x => x.UserId == requestModel.UserId);
           
            if (userProfileData == null)
            {
                userProfileData = new UserContactEntity()
                {
                    FirstName = requestModel.FirstName,
                    LastName = requestModel.LastName,
                    Patronymic = requestModel.Patronymic,
                    DateOfBirth = requestModel.DateOfBirth,
                    PhoneNumber = requestModel.PhoneNumber,
                    Adress = requestModel.Adress,
                    PassportSeries = requestModel.PassportSeries,
                    PassportNumber = requestModel.PassportNumber,
                    IdentityCode = requestModel.IdentityCode,
                    UserId = requestModel.UserId
                };

                try
                {
                    await dbContext.UserContacts.AddAsync(userProfileData);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex.Message);
                    response.Success = false;
                }
            }
            else
            {
                try
                {
                    userProfileData.FirstName = requestModel.FirstName;
                    userProfileData.LastName = requestModel.LastName;
                    userProfileData.Patronymic = requestModel.Patronymic;
                    userProfileData.DateOfBirth = requestModel.DateOfBirth;
                    userProfileData.PhoneNumber = requestModel.PhoneNumber;
                    userProfileData.Adress = requestModel.Adress;
                    userProfileData.PassportSeries = requestModel.PassportSeries;
                    userProfileData.PassportNumber = requestModel.PassportNumber;
                    userProfileData.IdentityCode = requestModel.IdentityCode;

                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex.Message);
                    response.Success = false;
                }
            }
            
            response.Success = response.Errors != null ? false : true;

            return response; 
        }
    }
}
