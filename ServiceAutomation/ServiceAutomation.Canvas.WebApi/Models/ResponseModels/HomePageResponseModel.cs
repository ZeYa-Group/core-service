namespace ServiceAutomation.Canvas.WebApi.Models.ResponseModels
{
    public class HomePageResponseModel
    {
        public decimal AvailableForWithdrawal { get; set; }
        public decimal AwaitingAccrual { get; set; }
        public decimal AllTimeIncome { get; set; }
        public int BaseLevel { get; set; }
        public int MounthlyLevel { get; set; }
        public decimal GroupTurnover { get; set; }
        public decimal MonthlyTurnover { get; set; }
        public decimal ReuqiredGroupTurnover { get; set; }
        public string ReuqiredAction { get; set; }
        public long ReceivedPayoutPercentage {get; set; }
        public string PackageName { get; set; }
    }
}
