namespace MITD.Fuel.Domain.Model.Enums
{
    public enum WorkflowStages
    {
        None = -1,
        Initial = 0,
        Approved = 1,
        FinalApproved = 2,
        Submited = 3,
        Closed = 4,
        Canceled = 5,
        SubmitRejected = 6,
        FinancialSubmitted = 7,
    }
}