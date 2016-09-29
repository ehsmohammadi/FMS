﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Presentation;
namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class ActionTypeDto
    {
        long id;
        public long Id
        {
            get { return id; }
            set { this.SetField(p => p.Id, ref id, value); }
        }

        string actionName;

        public string ActionName
        {
            get { return actionName; }
            set { this.SetField(p => p.ActionName, ref actionName, value); }
        }

        string description;

        public string Description
        {
            get { return description; }
            set { this.SetField(p => p.Description, ref description, value); }
        }
    }
}