using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static ServiceAutomation.Canvas.WebApi.Constants.Requests;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        private const string BasePath = "";

        public UserProfileService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<UserProfileResponseModel> GetUserInfo(Guid userId)
        {
            var user = await dbContext.Users
                //.Include(x => x.User)
                //.ThenInclude(x => x.ProfilePhoto)
                .Include(x => x.UserContact)
                .Include(x => x.UserPhoneNumber)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return mapper.Map<UserProfileResponseModel>(user);
        }

        public async Task<ResultModel> UploadProfilePhoto(Guid userId, byte[] data)
        {
            var response = new ResultModel();
            var photo = await dbContext.ProfilePhotos.FirstOrDefaultAsync(x => x.UserId == userId);

            if (photo == null)
            {
                photo = new ProfilePhotoEntity()
                {
                    //Data = data,
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
                //photo.Data = data;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Success = false;
            }

            response.Success = response.Errors != null ? false : true;

            return response;
        }

        public async Task<ResultModel> UploadProfileInfo(Guid userId, string firstName, string lastName, string patronymic, DateTime dateOfBirth)
        {
            var response = new ResultModel();

            var userProfileData = await dbContext.UserContacts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userProfileData == null)
            {
                userProfileData = new UserProfileInfoEntity()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Patronymic = patronymic,
                    DateOfBirth = dateOfBirth,
                    UserId = userId
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
                    userProfileData.FirstName = firstName;
                    userProfileData.LastName = lastName;
                    userProfileData.Patronymic = patronymic;
                    userProfileData.DateOfBirth = dateOfBirth;

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

        public async Task<ResultModel> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var response = new ResultModel();
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                response.Errors.Add("User is null");
                response.Success = false;

                return response;
            }

            var isPasswordCorrect = VerifyPasswordhash(oldPassword, user.PasswordHash, user.PasswordSalt);

            if (isPasswordCorrect)
            {
                var updatedPasswordModel = CreatePasswordHash(newPassword);

                user.PasswordSalt = updatedPasswordModel.PasswordSalt;
                user.PasswordHash = updatedPasswordModel.PasswordHash;

                await dbContext.SaveChangesAsync();

                response.Success = true;
            }
            else
            {
                response.Errors.Add("Password is incorrect");
                response.Success = false;
            }

            return response;
        }

        private bool VerifyPasswordhash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes((string)password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private PasswordHashModel CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                return new PasswordHashModel()
                {
                    PasswordSalt = hmac.Key,
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
                };
            }
        }

        public async Task<ResultModel> ChangeEmailAdress(Guid userId, string newEmail)
        {
            var result = new ResultModel();

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var isEmailExists = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == newEmail);

            if (user != null)
            {
                if (isEmailExists != null)
                {
                    user.Email = newEmail;
                    await dbContext.SaveChangesAsync();

                    result.Success = true;
                    return result;
                }
            }

            result.Success = false;
            return result;
        }

        public async Task<ResultModel> UploadPhoneNumber(Guid userId, string newPhoneNumber)
        {
            var result = new ResultModel();
            var user = await dbContext.Users
                .Include(x => x.UserPhoneNumber)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                if (user.UserPhoneNumber == null)
                {
                    user.UserPhoneNumber = new UserPhoneNumberEntity()
                    {
                        PhoneNumber = newPhoneNumber
                    };

                    await dbContext.UserPhones.AddAsync(user.UserPhoneNumber);
                    await dbContext.SaveChangesAsync();

                    result.Success = true;
                    return result;
                }

                user.UserPhoneNumber.PhoneNumber = newPhoneNumber;
                await dbContext.SaveChangesAsync();

                result.Success = true;
                return result;
            }

            result.Success = false;
            return result;
        }
    }
}
