using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(38)]
    public class Migration_V38_RevertScenario : Migration
    {
        public override void Down()
        {
            Delete.Column("IsReform").FromTable("Vouchers").InSchema("Fuel");
        }

        public override void Up()
        {
            Alter.Table("Vouchers").InSchema("Fuel").AddColumn("IsReform").AsBoolean().NotNullable().WithDefaultValue(false);

            Rename.Column("ReceivedInMainUnit").OnTable("OrderItems").InSchema(Migration_Initial.FUEL_SCHEMA).To("OperatedQuantityInMainUnit");

            Create.Table("OrderItemOperatedQuantity").InSchema(Migration_Initial.FUEL_SCHEMA)
                .WithColumn("Id").AsInt64().PrimaryKey().Identity().Indexed()
                .WithColumn("OrderItemId").AsInt64().NotNullable()
                    .ForeignKey("FK_OrderItemOperatedQuantity_OrderItemId_OrderItems_Id", Migration_Initial.FUEL_SCHEMA, "OrderItems", "Id")
                .WithColumn("FuelReportDetailId").AsInt64().NotNullable()
                    .ForeignKey("FK_OrderItemOperatedQuantity_FuelReportDetailId_FuelReportDetail_Id", Migration_Initial.FUEL_SCHEMA, "FuelReportDetail", "Id")
                .WithColumn("UnitCode").AsString().NotNullable()
                .WithColumn("QuantityAmountInMainUnit").AsDecimal(18, 3).NotNullable();


            this.Execute.WithConnection((conn, tran) =>
            {
                var selectCommand = conn.CreateCommand();
                selectCommand.CommandText =
                    @"SELECT Fuel.FuelReportDetail.Receive AS OperatedQuantityInMainUnit, Fuel.FuelReportDetail.Id AS FuelReportDetailId, Fuel.OrderItems.Id AS OrderItemId, 'TON' AS UnitCode
                        FROM Fuel.FuelReportDetail INNER JOIN
                            Fuel.[Order] ON Fuel.FuelReportDetail.ReceiveReference_ReferenceId = Fuel.[Order].Id INNER JOIN
                            Fuel.OrderItems ON Fuel.[Order].Id = Fuel.OrderItems.OrderId AND Fuel.FuelReportDetail.GoodId = Fuel.OrderItems.GoodId
                        WHERE (NOT (Fuel.FuelReportDetail.Receive IS NULL)) AND (Fuel.FuelReportDetail.ReceiveReference_ReferenceType = 1)
                    UNION
                    SELECT   Fuel.FuelReportDetail.Transfer AS OperatedQuantityInMainUnit, Fuel.FuelReportDetail.Id AS FuelReportDetailId, Fuel.OrderItems.Id AS OrderItemId, 'TON' AS UnitCode
                        FROM      Fuel.FuelReportDetail INNER JOIN
                                        Fuel.[Order] ON Fuel.FuelReportDetail.TransferReference_ReferenceId = Fuel.[Order].Id INNER JOIN
                                        Fuel.OrderItems ON Fuel.[Order].Id = Fuel.OrderItems.OrderId AND Fuel.FuelReportDetail.GoodId = Fuel.OrderItems.GoodId
                        WHERE   (NOT (Fuel.FuelReportDetail.Transfer IS NULL)) AND (Fuel.FuelReportDetail.TransferReference_ReferenceType = 1)
";

                selectCommand.Transaction = tran;

                var insertQueryList = new List<string>();

                using (var selectReader = selectCommand.ExecuteReader())
                {
                    while (selectReader.Read())
                    {
                        var orderItemId = (long)selectReader["OrderItemId"];
                        var fuelReportDetailId = (long)selectReader["FuelReportDetailId"];
                        var operatedQuantityInMainUnit = (decimal)selectReader["OperatedQuantityInMainUnit"];
                        var unitCode = (string)selectReader["UnitCode"];

                        insertQueryList.Add(string.Format("INSERT INTO Fuel.OrderItemOperatedQuantity (OrderItemId, FuelReportDetailId, QuantityAmountInMainUnit, UnitCode) VALUES ({0}, {1}, {2}, '{3}');",
                            orderItemId,
                            fuelReportDetailId,
                            operatedQuantityInMainUnit,
                            unitCode
                        ));
                    }

                    selectReader.Close();
                }

                var insertCommand = conn.CreateCommand();

                foreach (var insertCommandText in insertQueryList)
                {
                    insertCommand.CommandText = insertCommandText;
                    insertCommand.Transaction = tran;

                    insertCommand.ExecuteNonQuery();
                }

                MessageBox.Show(insertQueryList.Count.ToString());
            });

            using (DataContainer context = new DataContainer())
            {
                DataConfigurationProvider.ModifyInvoiceWorkflowConfigForRejectSubmittedInvoice_950326(context);
            }
        }
    }
}
