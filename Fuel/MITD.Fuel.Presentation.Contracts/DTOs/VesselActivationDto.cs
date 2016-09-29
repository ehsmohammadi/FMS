using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.Fuel.Presentation.Contracts.Infrastructure;
using MITD.Presentation;

namespace MITD.Fuel.Presentation.Contracts.DTOs
{
    public partial class VesselActivationDto
    {
        public DateTime ActivationDate { get; set; }

        public List<VesselActivationItemDto> VesselActivationItemDtos { get; set; } 

    }
}
