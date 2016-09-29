using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(16)]
    public class Migration_V16 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Execute.Sql("ALTER VIEW [BasicInfo].[UserView] AS " +
                        "SELECT   Id, Id AS IdentityId, LastName + ', ' + FirstName AS Name, CompanyId, " +
                        "CAST( CASE (SELECT COUNT(*) FROM dbo.Parties_CustomActions pca WHERE pca.PartyId = u.Id AND pca.ActionTypeId = 26 AND pca.IsGranted = 1) WHEN 1 THEN 1 ELSE 0 END AS BIT)  AS IsFRApprover " +
                        "FROM dbo.Users AS u");
        }

    }
}
