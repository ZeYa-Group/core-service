﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using ServiceAutomation.Canvas.WebApi.Interfaces;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.Canvas.WebApi.Models.SubRequestModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static ServiceAutomation.Canvas.WebApi.Constants.Requests;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public DocumentVerificationService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ResultModel> SendUserVerificationData(DocumentVerificationRequestModel requestModel)
        {
            var userOrganizationType = requestModel.EmploymentType;

            switch (userOrganizationType)
            {
                case TypeOfEmployment.LegalEntity:

                    var legalEntityModel = new LegalEntityModel()
                    {
                        UserId = requestModel.UserId,
                        Region = requestModel.DocumentVerificationModels.BankRequecitationModel.BankRegion,
                        Locality = requestModel.DocumentVerificationModels.BankRequecitationModel.BankLocality,
                        BankStreet = requestModel.DocumentVerificationModels.BankRequecitationModel.BankStreet,
                        BankHouseNumber = requestModel.DocumentVerificationModels.BankRequecitationModel.BankHouseNumber,
                        BeneficiaryBankName = requestModel.DocumentVerificationModels.BankRequecitationModel.BeneficiaryBankName,
                        CheckingAccount = requestModel.DocumentVerificationModels.BankRequecitationModel.CheckingAccount,
                        SWIFT = requestModel.DocumentVerificationModels.BankRequecitationModel.SWIFT,
                        Disctrict = requestModel.DocumentVerificationModels.PersonalAdressModel.Disctrict,
                        City = requestModel.DocumentVerificationModels.PersonalAdressModel.City,
                        Index = requestModel.DocumentVerificationModels.PersonalAdressModel.Index,
                        Street = requestModel.DocumentVerificationModels.PersonalAdressModel.Street,
                        HouseNumber = requestModel.DocumentVerificationModels.PersonalAdressModel.HouseNumber,
                        Flat = requestModel.DocumentVerificationModels.PersonalAdressModel.Flat
                    };

                    return await SendLegalEntityData(legalEntityModel);
                case TypeOfEmployment.IndividualEntity:

                    var individualEntityModel = new IndividualEntityModel()
                    {
                        UserId = requestModel.UserId,
                        LegalEntityFullName = requestModel.DocumentVerificationModels.LegalDataModel.LegalEntityFullName,
                        HeadFullName = requestModel.DocumentVerificationModels.LegalDataModel.HeadFullName,
                        LegalEntityAbbreviatedName = requestModel.DocumentVerificationModels.LegalDataModel.LegalEntityAbbreviatedName,
                        HeadPosition = requestModel.DocumentVerificationModels.LegalDataModel.HeadPosition,
                        UNP = requestModel.DocumentVerificationModels.LegalDataModel.UNP,
                        BaseOrganization = requestModel.DocumentVerificationModels.LegalDataModel.BaseOrganization,
                        AccountantName = requestModel.DocumentVerificationModels.LegalDataModel.AccountantName,
                        CertificateNumber = requestModel.DocumentVerificationModels.WitnessDataModel.CertificateNumber,
                        RegistrationAuthority = requestModel.DocumentVerificationModels.WitnessDataModel.RegistrationAuthority,
                        CertificateDateIssue = requestModel.DocumentVerificationModels.WitnessDataModel.CertificateDateIssue,
                        BankRegion = requestModel.DocumentVerificationModels.BankRequecitationModel.BankRegion,
                        BankLocality = requestModel.DocumentVerificationModels.BankRequecitationModel.BankLocality,
                        BankStreet = requestModel.DocumentVerificationModels.BankRequecitationModel.BankStreet,
                        BankHouseNumber = requestModel.DocumentVerificationModels.BankRequecitationModel.BankHouseNumber,
                        BeneficiaryBankName = requestModel.DocumentVerificationModels.BankRequecitationModel.BeneficiaryBankName,
                        CheckingAccount = requestModel.DocumentVerificationModels.BankRequecitationModel.CheckingAccount,
                        SWIFT = requestModel.DocumentVerificationModels.BankRequecitationModel.SWIFT,
                        Region = requestModel.DocumentVerificationModels.LegallyAddressModel.Region,
                        Locality = requestModel.DocumentVerificationModels.LegallyAddressModel.Locality,
                        Index = requestModel.DocumentVerificationModels.LegallyAddressModel.Index,
                        Street = requestModel.DocumentVerificationModels.LegallyAddressModel.Street,
                        HouseNumber = requestModel.DocumentVerificationModels.LegallyAddressModel.HouseNumber,
                        Location = requestModel.DocumentVerificationModels.LegallyAddressModel.Location,
                        RoomNumber = requestModel.DocumentVerificationModels.LegallyAddressModel.RoomNumber
                    };

                    return await SendIndividualEntityData(individualEntityModel);
                case TypeOfEmployment.IndividualEntrepreneur:

                    var individualEntityModel2 = new IndividualEntityModel()
                    {
                        UserId = requestModel.UserId,
                        LegalEntityFullName = requestModel.DocumentVerificationModels.LegalDataModel.LegalEntityFullName,
                        HeadFullName = requestModel.DocumentVerificationModels.LegalDataModel.HeadFullName,
                        LegalEntityAbbreviatedName = requestModel.DocumentVerificationModels.LegalDataModel.LegalEntityAbbreviatedName,
                        HeadPosition = requestModel.DocumentVerificationModels.LegalDataModel.HeadPosition,
                        UNP = requestModel.DocumentVerificationModels.LegalDataModel.UNP,
                        BaseOrganization = requestModel.DocumentVerificationModels.LegalDataModel.BaseOrganization,
                        AccountantName = requestModel.DocumentVerificationModels.LegalDataModel.AccountantName,
                        CertificateNumber = requestModel.DocumentVerificationModels.WitnessDataModel.CertificateNumber,
                        RegistrationAuthority = requestModel.DocumentVerificationModels.WitnessDataModel.RegistrationAuthority,
                        CertificateDateIssue = requestModel.DocumentVerificationModels.WitnessDataModel.CertificateDateIssue,
                        BankRegion = requestModel.DocumentVerificationModels.BankRequecitationModel.BankRegion,
                        BankLocality = requestModel.DocumentVerificationModels.BankRequecitationModel.BankLocality,
                        BankStreet = requestModel.DocumentVerificationModels.BankRequecitationModel.BankStreet,
                        BankHouseNumber = requestModel.DocumentVerificationModels.BankRequecitationModel.BankHouseNumber,
                        BeneficiaryBankName = requestModel.DocumentVerificationModels.BankRequecitationModel.BeneficiaryBankName,
                        CheckingAccount = requestModel.DocumentVerificationModels.BankRequecitationModel.CheckingAccount,
                        SWIFT = requestModel.DocumentVerificationModels.BankRequecitationModel.SWIFT,
                        Region = requestModel.DocumentVerificationModels.LegallyAddressModel.Region,
                        Locality = requestModel.DocumentVerificationModels.LegallyAddressModel.Locality,
                        Index = requestModel.DocumentVerificationModels.LegallyAddressModel.Index,
                        Street = requestModel.DocumentVerificationModels.LegallyAddressModel.Street,
                        HouseNumber = requestModel.DocumentVerificationModels.LegallyAddressModel.HouseNumber,
                        Location = requestModel.DocumentVerificationModels.LegallyAddressModel.Location,
                        RoomNumber = requestModel.DocumentVerificationModels.LegallyAddressModel.RoomNumber
                    };

                    return await SendIndividualEntityData(individualEntityModel2);
                default:
                    return new ResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid argument"
                        }
                    };
            }
        }

        private async Task<ResultModel> SendLegalEntityData(LegalEntityModel dataModel)
        {
            var userorganization = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == dataModel.UserId && x.TypeOfEmployment == dataModel.TypeOfEmployment);

            if (userorganization == null)
            {
                userorganization = new UserAccountOrganizationEntity()
                {
                    UserId = dataModel.UserId,
                    TypeOfEmployment = dataModel.TypeOfEmployment
                };

                var legalUserModel = new LegalUserOrganizationDataEntity()
                {
                    UserId = dataModel.UserId,
                    Region = dataModel.Region,
                    Locality = dataModel.Locality,
                    BankStreet = dataModel.BankStreet,
                    BankHouseNumber = dataModel.BankHouseNumber,
                    BeneficiaryBankName = dataModel.BeneficiaryBankName,
                    CheckingAccount = dataModel.CheckingAccount,
                    SWIFT = dataModel.SWIFT,
                    Disctrict = dataModel.Disctrict,
                    City = dataModel.City,
                    Index = dataModel.Index,
                    Street = dataModel.Street,
                    HouseNumber = dataModel.HouseNumber,
                    Flat = dataModel.Flat
                };

                try
                {
                    await dbContext.UserAccountOrganizations.AddAsync(userorganization);
                    await dbContext.LegalUserOrganizationsData.AddAsync(legalUserModel);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            ex.Message
                        }
                    };
                }

                return new ResultModel()
                {
                    Success = true
                };
            }
            else
            {
                var legalUserDataModel = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == dataModel.UserId);
                legalUserDataModel.UserId = dataModel.UserId;
                legalUserDataModel.Region = dataModel.Region;
                legalUserDataModel.Locality = dataModel.Locality;
                legalUserDataModel.BankStreet = dataModel.BankStreet;
                legalUserDataModel.BankHouseNumber = dataModel.BankHouseNumber;
                legalUserDataModel.BeneficiaryBankName = dataModel.BeneficiaryBankName;
                legalUserDataModel.CheckingAccount = dataModel.CheckingAccount;
                legalUserDataModel.SWIFT = dataModel.SWIFT;
                legalUserDataModel.Disctrict = dataModel.Disctrict;
                legalUserDataModel.City = dataModel.City;
                legalUserDataModel.Index = dataModel.Index;
                legalUserDataModel.Street = dataModel.Street;
                legalUserDataModel.HouseNumber = dataModel.HouseNumber;
                legalUserDataModel.Flat = dataModel.Flat;

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            ex.Message
                        }
                    };
                }

                return new ResultModel()
                {
                    Success = true
                };
            }
        }

        private async Task<ResultModel> SendIndividualEntityData(IndividualEntityModel dataModel)
        {
            var userorganization = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == dataModel.UserId && x.TypeOfEmployment == dataModel.TypeOfEmployment);

            if (userorganization == null)
            {
                userorganization = new UserAccountOrganizationEntity()
                {
                    UserId = dataModel.UserId,
                    TypeOfEmployment = dataModel.TypeOfEmployment
                };

                var individualUserModel = new IndividualUserOrganizationDataEntity()
                {
                    UserId = dataModel.UserId,
                    LegalEntityFullName = dataModel.LegalEntityFullName,
                    HeadFullName = dataModel.HeadFullName,
                    LegalEntityAbbreviatedName = dataModel.LegalEntityAbbreviatedName,
                    HeadPosition = dataModel.HeadPosition,
                    UNP = dataModel.UNP,
                    BaseOrganization = dataModel.BaseOrganization,
                    AccountantName = dataModel.AccountantName,
                    CertificateNumber = dataModel.CertificateNumber,
                    RegistrationAuthority = dataModel.RegistrationAuthority,
                    CertificateDateIssue = dataModel.CertificateDateIssue,
                    BankRegion = dataModel.BankRegion,
                    BankLocality = dataModel.BankLocality,
                    BankStreet = dataModel.BankStreet,
                    BankHouseNumber = dataModel.BankHouseNumber,
                    BeneficiaryBankName = dataModel.BeneficiaryBankName,
                    CheckingAccount = dataModel.CheckingAccount,
                    SWIFT = dataModel.SWIFT,
                    Region = dataModel.Region,
                    Locality = dataModel.Locality,
                    Index = dataModel.Index,
                    Street = dataModel.Street,
                    HouseNumber = dataModel.HouseNumber,
                    Location = dataModel.Location,
                    RoomNumber = dataModel.RoomNumber
                };

                try
                {
                    await dbContext.UserAccountOrganizations.AddAsync(userorganization);
                    await dbContext.IndividualUserOrganizationsData.AddAsync(individualUserModel);
                    await dbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return new ResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            ex.Message
                        }
                    };
                }

                return new ResultModel()
                {
                    Success = true
                };
            }
            else
            {
                var individualUserModel = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == dataModel.UserId);

                individualUserModel.UserId = dataModel.UserId;
                individualUserModel.LegalEntityFullName = dataModel.LegalEntityFullName;
                individualUserModel.HeadFullName = dataModel.HeadFullName;
                individualUserModel.LegalEntityAbbreviatedName = dataModel.LegalEntityAbbreviatedName;
                individualUserModel.HeadPosition = dataModel.HeadPosition;
                individualUserModel.UNP = dataModel.UNP;
                individualUserModel.BaseOrganization = dataModel.BaseOrganization;
                individualUserModel.AccountantName = dataModel.AccountantName;
                individualUserModel.CertificateNumber = dataModel.CertificateNumber;
                individualUserModel.RegistrationAuthority = dataModel.RegistrationAuthority;
                individualUserModel.CertificateDateIssue = dataModel.CertificateDateIssue;
                individualUserModel.BankRegion = dataModel.BankRegion;
                individualUserModel.BankLocality = dataModel.BankLocality;
                individualUserModel.BankStreet = dataModel.BankStreet;
                individualUserModel.BankHouseNumber = dataModel.BankHouseNumber;
                individualUserModel.BeneficiaryBankName = dataModel.BeneficiaryBankName;
                individualUserModel.CheckingAccount = dataModel.CheckingAccount;
                individualUserModel.SWIFT = dataModel.SWIFT;
                individualUserModel.Region = dataModel.Region;
                individualUserModel.Locality = dataModel.Locality;
                individualUserModel.Index = dataModel.Index;
                individualUserModel.Street = dataModel.Street;
                individualUserModel.HouseNumber = dataModel.HouseNumber;
                individualUserModel.Location = dataModel.Location;
                individualUserModel.RoomNumber = dataModel.RoomNumber;

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return new ResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            ex.Message
                        }
                    };
                }

                return new ResultModel()
                {
                    Success = true
                };
            }
        }

        public async Task<OneOf<IndividualEntityDataResponseModel, LegalEntityDataResponseModel, ResultModel>> GetUserVerifiedData(Guid userId)
        {
            var userOrganizationData = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.UserId == userId);

            if (userOrganizationData == null)
            {
                return new ResultModel()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "There is no data for this user."
                    }
                };
            }

            var userOrganizationType = userOrganizationData.TypeOfEmployment;

            switch (userOrganizationType)
            {
                case TypeOfEmployment.LegalEntity:
                    var data1 = await dbContext.LegalUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == userId);
                    return mapper.Map<LegalEntityDataResponseModel>(data1);
                case TypeOfEmployment.IndividualEntity:
                    var data2 = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == userId);
                    return mapper.Map<IndividualEntityDataResponseModel>(data2);
                case TypeOfEmployment.IndividualEntrepreneur:
                    var data3 = await dbContext.IndividualUserOrganizationsData.FirstOrDefaultAsync(x => x.UserId == userId);
                    return mapper.Map<IndividualEntityDataResponseModel>(data3);
                default:
                    throw new ArgumentException("Invalid argument");
            }
        }

        //private async Task DeleteIfExist(Guid userId)
        //{
        //    var usersOrganixationsType = await dbContext.UserAccountOrganizations.FirstOrDefaultAsync(x => x.Id == userId);

        //    if(usersOrganixationsType != null)
        //    {
                 
        //    }
        //}
    }
}
