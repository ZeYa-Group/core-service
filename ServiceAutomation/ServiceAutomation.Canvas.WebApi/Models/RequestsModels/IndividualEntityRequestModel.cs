using ServiceAutomation.DataAccess.Models.Enums;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;

namespace ServiceAutomation.Canvas.WebApi.Models.RequestsModels
{
    public class IndividualEntityRequestModel
    {
        public Guid UserId { get; set; }
        public Country Country { get; set; }
        public TypeOfEmployment EmploymentType { get; set; }
    }
}
