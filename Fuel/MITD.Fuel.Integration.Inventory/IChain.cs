using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Integration.Inventory
{
    public interface IChain
    {
        void HandleRequest();
        string Name { get; set; }
        ChainType  ChainType { get; set; }
    }


    public enum ChainType
    {
        None,
        Activity,
        Condition,
        Exception
    }
}
