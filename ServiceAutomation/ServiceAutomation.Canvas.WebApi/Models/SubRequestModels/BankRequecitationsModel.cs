namespace ServiceAutomation.Canvas.WebApi.Models.SubRequestModels
{
    public class BankRequecitationsModel : DocumentModel
    {
        public string Region { get; set; }
        public string Locality { get; set; }
        public string BankStreet { get; set; }
        public string BankHouseNumber { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string CheckingAccount { get; set; }
        public string SWIFT { get; set; }
    }
}
