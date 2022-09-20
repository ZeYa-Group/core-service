using ServiceAutomation.Canvas.WebApi.Models.SubRequestModels;
using ServiceAutomation.DataAccess.Models.Enums;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;

namespace ServiceAutomation.Canvas.WebApi.Models.RequestsModels
{
    public class IndividualEntrepreneurRequestModel
    {
        public Guid UserId { get; set; }
        public Country Country { get; set; }
        public TypeOfEmployment EmploymentType { get; set; }
        public LegalDataModel LegalDataModel { get; set; }
        public WitnessDataModel WitnessDataModel { get; set; }
        public BankRequecitationsModel BankRequecitationModel { get; set; }
        public LegallyAddressModel LegallyAddressModel { get; set; }
    }
}
