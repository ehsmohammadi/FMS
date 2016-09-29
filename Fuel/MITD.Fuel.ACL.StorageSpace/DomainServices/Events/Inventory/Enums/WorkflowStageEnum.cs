namespace MITD.Fuel.ACL.StorageSpace.DomainServices.Events.Inventory.Enums
{
    public enum WorkflowStageEnum
    {
        //None = -1,
        //[Description("ثبت اولیه")]
        Initial = 0,
        //[Description("تایید میانی")]
        Approved = 1,
        //[Description("تایید نهایی")]
        FinalApproved = 2,
        //[Description("تایید نهایی")]
        Submited = 3,
        //[Description("بسته شده")]
        Closed = 4,
        //[Description("کنسل")]
        Canceled = 5,
        //[Description("برگشت")]
        SubmitRejected = 6,
    }
}
