using Microsoft.EntityFrameworkCore;
using OneOf;
using ServiceAutomation.Canvas.WebApi.Interfaces.CountryService;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.DbContexts;
using ServiceAutomation.DataAccess.Models.EntityModels;
using ServiceAutomation.DataAccess.Models.Enums;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly AppDbContext dbContext;
        public DocumentVerificationService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ResultModel> UploadLegalEntityData(LegalEntityRequestModel requestModel)
        {
            return new ResultModel()
            {
                Success = true
            };
        }
    }
}