﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class IndividualEntrepreneurUserOrganizationDataEntity : Entity
    {
        public Guid UserId { get; set; }
        public string LegalEntityFullName { get; set; }
        public string HeadFullName { get; set; }
        public string LegalEntityAbbreviatedName { get; set; }
        public string HeadPosition { get; set; }
        public string UNP { get; set; }
        public string BaseOrganization { get; set; }
        public string AccountantName { get; set; }
        public string CertificateNumber { get; set; }
        public string RegistrationAuthority { get; set; }
        public string CertificateDateIssue { get; set; }
        public string BankRegion { get; set; }
        public string BankLocality { get; set; }
        public string BankStreet { get; set; }
        public string BankHouseNumber { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string CheckingAccount { get; set; }
        public string SWIFT { get; set; }
        public string Region { get; set; }
        public string Locality { get; set; }
        public string Index { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Location { get; set; }
        public string RoomNumber { get; set; }
        public bool IsVerivied { get; set; }
    }
}
