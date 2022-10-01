using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.AdministratorResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        public AdministratorService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Task AcceptContactVerificationRequest(Guid requestId, Guid userId)
        {
            throw new NotImplementedException();
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

            await dbContext.SaveChangesAsync();
        }

        public Task<ICollection<UserContactsVerificationResponseModel>> GetContactVerificationRequest()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<UserVerificationResponseModel>> GetVerificationRequest()
        {
            var legalUsersRequests = await dbContext.LegalUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result1 = legalUsersRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToList();

            var individualUsersRequests = await dbContext.IndividualUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result2 = individualUsersRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToList();

            var individualEntrepreneursRequests = await dbContext.IndividualEntrepreneurUserOrganizationsData.Where(x => x.IsVerivied == false).ToListAsync();
            var result3 = individualEntrepreneursRequests.Select(x => mapper.Map<UserVerificationResponseModel>(x)).ToList();

            foreach(var item in result1)
            {
                var itemExtraData = await dbContext.UserContacts
                    .Include(x => x.User)
                    .ThenInclude(x => x.UserPhoneNumber)
                    .FirstOrDefaultAsync(x => x.UserId == item.UserId);

                if(itemExtraData != null)
                {
                    item.Name = itemExtraData.FirstName + " " + itemExtraData.LastName;
                    item.Email = itemExtraData?.User?.Email;
                    item.PhoneNumber = itemExtraData?.User?.UserPhoneNumber?.PhoneNumber;
                    item.TypeOfEmployment = itemExtraData?.User?.UserAccountOrganization.ToString();
                }
            }



            var response = new List<UserVerificationResponseModel>();
            response.AddRange(result1);
            response.AddRange(result2);
            response.AddRange(result3);

            return response;
        }

        public Task RejectContactVerificationRequest(Guid requestId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RejectVerificationRequest(Guid requestId, Guid userId)
        {
            var userOrganizationType = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == userId);

            switch (userOrganizationType.TypeOfEmployment)
            {
                case TypeOfEmployment.LegalEntity:
                    var legalEntityRequest = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.LegalUserOrganizationsData.Remove(legalEntityRequest);
                    break;
                case TypeOfEmployment.IndividualEntity:
                    var individualEntityRequest = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.IndividualUserOrganizationsData.Remove(individualEntityRequest);
                    break;
                case TypeOfEmployment.IndividualEntrepreneur:
                    var individualEntrepreneurEntityRequest = await dbContext.IndividualEntrepreneurUserOrganizationsData.FirstOrDefaultAsync(x => x.Id == requestId && x.IsVerivied == false);
                    dbContext.IndividualEntrepreneurUserOrganizationsData.Remove(individualEntrepreneurEntityRequest);
                    break;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
