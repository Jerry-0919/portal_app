namespace diga.bl.Constants
{
    public static class PlatformContractChangeCases
    {
        #region Payment parts changes
        public const string PaymentPartInvoiceUploaded = "PaymentPartInvoiceUploaded";
        public const string PaymentPartProofUploaded = "PaymentPartProofUploaded";
        #endregion

        #region Measurement report changes
        public const string MeasurementInvoiceUploaded = "MeasurementInvoiceUploaded";
        public const string MeasurementProofUploaded = "MeasurementProofUploaded";
        public const string MeasurementIsPaid = "MeasurementIsPaid";
        public const string MeasurementStatusChanged = "MeasurementStatusChanged";
        public const string MeasurementPriceChanged = "MeasurementPriceChanged";
        #endregion

        #region EstimateChanges

        public const string ApproveEstimate = "ApproveEstimate";
        public const string AnotherPercent = "AnotherPercent";
        public const string EstimateName = "EstimateName";
        public const string PlatformVATId = "PlatformVATId";

        public const string AddSection = "AddSection";
        public const string EditSection = "EditSection";
        public const string RemoveSection = "RemoveSection";

        public const string AddPosition = "AddPosition";
        public const string EditPosition = "EditPosition";
        public const string RemovePosition = "RemovePosition";

        #endregion

        #region ContractChanges

        public const string ApproveContract = "ApproveContract";

        public const string BuildStart = "BuildStart";
        public const string PlannedBuildEnd = "PlannedBuildEnd";
        public const string GeneralTerm = "GeneralTerm";
        public const string Price = "Price";
        public const string Description = "Description";

        public const string PaymentType = "PaymentType";
        public const string ComissionType = "ComissionType";
        public const string CooperationType = "CooperationType";
        public const string BudgetOfWorks = "BudgetOfWorks";
        public const string HoursOfWorks = "HoursOfWorks";
        public const string CostOfHour = "CostOfHour";
        public const string PaymentFrequency = "PaymentFrequency";

        public const string Prepayment = "Prepayment";
        public const string PrepaymentValue = "PrepaymentValue";
        public const string PrepaymentPercent = "PrepaymentPercent";

        public const string AddStep = "AddStep";
        public const string StepName = "StepName";
        public const string StepValue = "StepValue";
        public const string StepDate = "StepDate";
        public const string RemoveStep = "RemoveStep";

        public const string Refuse = "Refuse";
        public const string Close = "Close";

        public const string EditSigning = "EditSigning";
        public const string ApproveSigning = "ApproveSigning";

        public static string Finish = "Finish";

        #endregion
    }
}
