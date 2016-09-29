using System;
using MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.FuelReportStates;
using MITD.Fuel.Domain.Model.Enums;
using MITD.Fuel.Domain.Model.IDomainServices;

namespace MITD.Fuel.Domain.Model.DomainObjects.FuelReportAggregate.Factories
{
    public class FuelReportStateFactory : IFuelReportStateFactory
    {
        public FuelReportState CreateSubmitState()
        {
            return new SubmittedState(this);
        }

        public FuelReportState CreateOpenState()
        {
            return new OpenState(this);
        }

        public FuelReportState CreateClosedState()
        {
            return new ClosedState(this);
        }

        public FuelReportState CreateInvalidState()
        {
            return new InvalidState(this);
        }

        public FuelReportState CreateSubmitRejectedState()
        {
            return new SubmitRejectedState(this);
        }

        public FuelReportState CreatState(States state)
        {
            switch (state)
            {
                case States.Open:
                    return CreateOpenState();
                case States.Submitted:
                    return CreateSubmitState();
                case States.Closed:
                    return CreateClosedState();
                case States.Cancelled:
                    return CreateInvalidState();
                case States.SubmitRejected:
                    return this.CreateSubmitRejectedState();
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
        }
    }
}