using OneOf;
using ServiceAutomation.Canvas.WebApi.Models.RequestsModels;
using ServiceAutomation.Canvas.WebApi.Models.ResponseModels;
using ServiceAutomation.DataAccess.Models.EntityModels;
using System;
using System.Threading.Tasks;

namespace ServiceAutomation.Canvas.WebApi.Interfaces.CountryService
{
    public interface IDocumentVerificationService 
    {
        Task<ResultModel> UploadLegalEntityData(LegalEntityRequestModel requestModel);
    }
}
