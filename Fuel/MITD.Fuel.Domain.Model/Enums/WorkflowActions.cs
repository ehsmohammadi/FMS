namespace MITD.Fuel.Domain.Model.Enums
{
    public enum WorkflowActions
    {
        None=6,
        Init=0,
        Approve = 1,
        //FinalApprove = 2,    //Deprecated and not used, but the id of other ActionTypes should remain unchanged.
        Reject = 3,
        Cancel = 4,
        Close = 5,
    }
}