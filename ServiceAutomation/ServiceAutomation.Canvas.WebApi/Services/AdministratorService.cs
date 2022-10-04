﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.AdministratorResponseModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceAutomation.Canvas.WebApi.Constants.Requests;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment hostEnvironment;
        public AdministratorService(AppDbContext dbContext, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task AcceptContactVerificationRequest(Guid requestId, Guid userId)
        {
            var verificationRequest = await dbContext.UserContactVerifications.FirstOrDefaultAsync(x => x.Id == requestId);
            var currentUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            switch (verificationRequest.VerificationType)
            {
                case ContactVerificationType.EmailAdress:
                    currentUser.Email = verificationRequest.NewData;
                    verificationRequest.IsVerified = true;
                    break;
                case ContactVerificationType.PhoneNumber:
                    currentUser.PhoneNumber = verificationRequest.NewData;
                    verificationRequest.IsVerified = true;
                    break;
                default:
                    break;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task AcceptVerificationRequest(Guid requestId, Guid userId)
        {
            var userOrganizationType = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == userId);

            switch (userOrganizationType.TypeOfEmployment)
            {
                case TypeOfEmployment.LegalEntity:
                    var legalEntityRequest = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    legalEntityRequest.IsVerivied = true;
                    break;
                case TypeOfEmployment.IndividualEntity:
                    var individualEntityRequest = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    individualEntityRequest.IsVerivied = true;
                    break;
                case TypeOfEmployment.IndividualEntrepreneur:
                    var individualEntrepreneurEntityRequest = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    individualEntrepreneurEntityRequest.IsVerivied = true;
                    break;     
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.IsVerifiedUser = true;

            await dbContext.SaveChangesAsync();
        }

        public async Task AccepWitdrawRequest(Guid requestId)
        {
            var verificationRequest = await dbContext.UserAccuralsVerifications.Include(x => x.Accurals).FirstOrDefaultAsync(x => x.Id == requestId);

            foreach (var item in verificationRequest.Accurals.Select(x => x.Id))
            {
                var accural = await dbContext.Accruals.FirstOrDefaultAsync(x => x.Id == item);
                accural.TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Accept;

                await dbContext.SaveChangesAsync();
            }

            var withdarwAmount = verificationRequest.Accurals.Sum(x => x.AccuralAmount);

            var withdrawRequest = new WithdrawTransactionEntity()
            {
                UserId = verificationRequest.UserId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Accept,
                Date = DateTime.UtcNow,
                Value = withdarwAmount
            };

            var userOrganization = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);

            switch (userOrganization.TypeOfEmployment)
            {
                case TypeOfEmployment.LegalEntity:
                    var legalData = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = legalData.CheckingAccount;
                    break;
                case TypeOfEmployment.IndividualEntity:
                    var individualData = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = individualData.CheckingAccount;
                    break;
                case TypeOfEmployment.IndividualEntrepreneur:
                    var individualEntrepreneurData = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = individualEntrepreneurData.CheckingAccount;
                    break;
            }

            
            await dbContext.WithdrawTransactions.AddAsync(withdrawRequest);

            verificationRequest.IsVerified = true;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<UserContactsVerificationResponseModel>> GetContactVerificationRequest()
        {
            var contactVerificationCollection = await dbContext.UserContactVerifications
                .Include(x => x.User)
                .Where(x => x.IsVerified == false)
                .ToListAsync();

            return contactVerificationCollection.Select(x => mapper.Map<UserContactsVerificationResponseModel>(x)).ToList();
        }

        public async Task<ICollection<UserVerificationResponseModel>> GetVerificationRequest()
        {
            var legalUsersRequests = await dbContext.LegalUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result1 = legalUsersRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToArray();

            var individualUsersRequests = await dbContext.IndividualUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result2 = individualUsersRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToArray();

            var individualEntrepreneursRequests = await dbContext.IndividualEntrepreneurUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result3 = individualEntrepreneursRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToArray();

            for (int i=0; i < result1.Length; i++)
            {
                var itemExtraData = await dbContext.Users
                    .Include(x => x.UserAccountOrganization)
                    .FirstOrDefaultAsync(x => x.Id == result1[i].UserId);

                if (itemExtraData != null)
                {
                    result1[i].Name = itemExtraData.FirstName + " " + itemExtraData.LastName;
                    result1[i].Email = itemExtraData?.Email;
                    result1[i].PhoneNumber = itemExtraData?.PhoneNumber;
                    result1[i].TypeOfEmployment = itemExtraData?.UserAccountOrganization.TypeOfEmployment.ToString();
                }
            }

            for (int i = 0; i < result2.Length; i++)
            {
                var itemExtraData = await dbContext.Users
                    .Include(x => x.UserAccountOrganization)
                    .FirstOrDefaultAsync(x => x.Id == result2[i].UserId);

                var individualUsersPhoto = await dbContext.UserVerificationPhotos.FirstOrDefaultAsync(x => x.UserId == result2[i].UserId);

                if (itemExtraData != null)
                {
                    result2[i].Name = itemExtraData.FirstName + " " + itemExtraData.LastName;
                    result2[i].Email = itemExtraData?.Email;
                    result2[i].PhoneNumber = itemExtraData?.PhoneNumber;
                    result2[i].TypeOfEmployment = itemExtraData?.UserAccountOrganization.TypeOfEmployment.ToString();
                }

                if(individualUsersPhoto != null)
                {
                    result2[i].VerivicationPhoto = individualUsersPhoto.FullPath;
                }
            }

            for (int i = 0; i < result3.Length; i++)
            {
                var itemExtraData = await dbContext.Users
                    .Include(x => x.UserAccountOrganization)
                    .FirstOrDefaultAsync(x => x.Id == result3[i].UserId);

                var individualUsersPhoto = await dbContext.UserVerificationPhotos.FirstOrDefaultAsync(x => x.UserId == result3[i].UserId);

                if (itemExtraData != null)
                {
                    result3[i].Name = itemExtraData.FirstName + " " + itemExtraData.LastName;
                    result3[i].Email = itemExtraData?.Email;
                    result3[i].PhoneNumber = itemExtraData?.PhoneNumber;
                    result3[i].TypeOfEmployment = itemExtraData?.UserAccountOrganization.TypeOfEmployment.ToString();
                }

                if (individualUsersPhoto != null)
                {
                    result3[i].VerivicationPhoto = individualUsersPhoto.FullPath;
                }
            }

            var response = new List<UserVerificationResponseModel>();
            response.AddRange(result1);
            response.AddRange(result2);
            response.AddRange(result3);

            return response;
        }

        public async Task<ICollection<WithdrawVerifictionResponseModel>> GetWitdrawRequests()
        {
            var requests = await dbContext.UserAccuralsVerifications
                .Include(x => x.Accurals)
                .ThenInclude( x => x.Bonus)
                .Include(x => x.User)
                .ThenInclude(x => x.UserAccountOrganization)
                .Where(x => x.IsVerified == false)
                .ToListAsync();

            var result  = new List<WithdrawVerifictionResponseModel>();

            for (int i = 0; i < requests.Count; i++)
            {
                var item = mapper.Map<WithdrawVerifictionResponseModel>(requests[i]);

                switch (requests[i].User.UserAccountOrganization.TypeOfEmployment)
                {
                    case TypeOfEmployment.LegalEntity:
                        var legalAccount = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == requests[i].User.Id);
                        item.CheckingAccount = legalAccount.CheckingAccount;
                        break;
                    case TypeOfEmployment.IndividualEntity:
                        var individualAccount = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == requests[i].User.Id);
                        item.CheckingAccount = individualAccount.CheckingAccount;
                        break;
                    case TypeOfEmployment.IndividualEntrepreneur:
                        var individualEntrepreneurAccount = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == requests[i].User.Id);
                        item.CheckingAccount = individualEntrepreneurAccount.CheckingAccount;
                        break;
                }
               
                foreach(var accural in item.Accurals)
                {
                    item.WithdrawSum += accural.AccuralAmount;
                }

                result.Add(item);
            }

            return result;
        }

        public async Task RejectContactVerificationRequest(Guid requestId, Guid userId)
        {
            var contactVerificationRequest = await dbContext.UserContactVerifications.FirstOrDefaultAsync(x => x.Id == requestId);

            dbContext.UserContactVerifications.Remove(contactVerificationRequest);
            await dbContext.SaveChangesAsync();
        }

        public async Task RejectVerificationRequest(Guid requestId, Guid userId)
        {
            var userOrganizationType = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == userId);
            var photoPath = await dbContext.UserVerificationPhotos.FirstOrDefaultAsync(x => x.UserId == userId);
            
            switch (userOrganizationType.TypeOfEmployment)
            {
                case TypeOfEmployment.LegalEntity:
                    var legalEntityRequest = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.LegalUserOrganizationsData.Remove(legalEntityRequest);
                    break;
                case TypeOfEmployment.IndividualEntity:
                    var individualEntityRequest = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.IndividualUserOrganizationsData.Remove(individualEntityRequest);
                    System.IO.File.Delete(hostEnvironment.WebRootPath + photoPath.FullPath);
                    break;
                case TypeOfEmployment.IndividualEntrepreneur:
                    var individualEntrepreneurEntityRequest = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.IndividualEntrepreneurUserOrganizationsData.Remove(individualEntrepreneurEntityRequest);
                    System.IO.File.Delete(hostEnvironment.WebRootPath + photoPath.FullPath);
                    break;
            }

            var accountOrganization = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == userId);


            dbContext.UserAccountOrganizations.Remove(accountOrganization);
            await dbContext.SaveChangesAsync();
        }

        public async Task RejectWitdrawRequest(Guid requestId)
        {
            var verificationRequest = await dbContext.UserAccuralsVerifications.Include(x => x.Accurals).FirstOrDefaultAsync(x => x.Id == requestId);
            
            foreach(var item in verificationRequest.Accurals.Select(x => x.Id))
            {
                var accural = await dbContext.Accruals.FirstOrDefaultAsync(x => x.Id == item);
                accural.TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Failed;

                await dbContext.SaveChangesAsync();
            }

            var withdarwAmount = verificationRequest.Accurals.Sum(x => x.AccuralAmount);

            var withdrawRequest = new WithdrawTransactionEntity()
            {
                UserId = verificationRequest.UserId,
                TransactionStatus = DataAccess.Schemas.Enums.TransactionStatus.Failed,
                Date = DateTime.UtcNow,
                Value = withdarwAmount
            };

            var userOrganization = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);

            switch (userOrganization.TypeOfEmployment)
            {
                case TypeOfEmployment.LegalEntity:
                    var legalData = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = legalData.CheckingAccount;
                    break;
                case TypeOfEmployment.IndividualEntity:
                    var individualData = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = individualData.CheckingAccount;
                    break;
                case TypeOfEmployment.IndividualEntrepreneur:
                    var individualEntrepreneurData = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == verificationRequest.UserId);
                    withdrawRequest.CheckingAccount = individualEntrepreneurData.CheckingAccount;
                    break;
            }

            await dbContext.WithdrawTransactions.AddAsync(withdrawRequest);

            dbContext.UserAccuralsVerifications.Remove(verificationRequest);

            await dbContext.SaveChangesAsync();
        }
    }
}
