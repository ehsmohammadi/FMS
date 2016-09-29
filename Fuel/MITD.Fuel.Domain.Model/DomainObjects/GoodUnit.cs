#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MITD.CurrencyAndMeasurement.Domain.Contracts;
using MITD.Domain.Model;

#endregion

namespace MITD.Fuel.Domain.Model.DomainObjects
{
    public class GoodUnit
    {
        public GoodUnit()
        {

        }

        public GoodUnit(long id, long goodId, long? parentId, string aliasName, decimal coefficient, string abbreviation)
        {
            Id = id;
            GoodId = goodId;
            Name = aliasName;
            ParentId = parentId;
            Coefficient = coefficient;
            Abbreviation = abbreviation;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public decimal Coefficient { get; private set; }
        public string Abbreviation { get; private set; }

        public long GoodId { get; private set; }
        public long? ParentId { get; private set; }


        public virtual Good Good { get; private set; }
        public virtual GoodUnit Parent { get; private set; }
        public virtual List<GoodUnit> ChildList { get; private set; }

        public GoodUnit MainGoodUnit {
            get
            {
                var goodUnitNode = this;
                while (goodUnitNode.Parent != null)
                {
                    goodUnitNode = goodUnitNode.Parent;
                }

                return goodUnitNode;
            }
        }

        //public string GoodUOM { get; set; }
        //public Quantity GoodQuantity { get; set; }

    }
}