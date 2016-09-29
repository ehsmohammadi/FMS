using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.DomainObjects.ApproveFlow;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(37)]
    public class Migration_V37 : Migration
    {
        public override void Up()
        {
            using (DataContainer context = new DataContainer())
            {
                DataConfigurationProvider.ModifyOrderWorkflowConfigForRejectSubmittedOrder_941128(context);
            }
        }

        public override void Down()
        {

        }
    }
}

