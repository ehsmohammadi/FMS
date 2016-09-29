using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MITD.Fuel.Integration.Inventory
{
    public class ConditionChain<T> : IChain
    {
        private readonly Func<bool> _func;
        private readonly Func<Tuple<bool,List<T>>> _tuplFunc;
        public List<T> OutPutsList { get; set; }

        private  IChain _yesChain;
        private  IChain _noChain;
        public ConditionChain(string name, Func<bool> func)
        {
            _func = func;
            OutPutsList = new List<T>();
            Name = name;
            ChainType = ChainType.Condition;
        }

        public ConditionChain(string name, Func<System.Tuple<bool,List<T>>> tuplFunc)
        {
            _tuplFunc = tuplFunc;
            OutPutsList = new List<T>();
             Name = name;
            ChainType = ChainType.Condition;
        }

        public void SetChain( IChain yesChain,IChain noChain)
        {
            _yesChain = yesChain;
            _noChain = noChain;
        }

        public void HandleRequest()
        {
           
            OutPutsList.Clear();
           
            if (_tuplFunc!=null)
            {
                var res=_tuplFunc.Invoke();
                if (res.Item2!=null)
                    OutPutsList.AddRange(res.Item2);
                if (res.Item1)
                {
                    if (_yesChain != null)
                        _yesChain.HandleRequest();
                }
                else
                {
                    if (_noChain != null)
                        _noChain.HandleRequest();
                } 
            }
            else
            {

                if (_func.Invoke())
                {
                    if (_yesChain != null)
                        _yesChain.HandleRequest();
                }
                else
                {
                    if (_noChain != null)
                        _noChain.HandleRequest();
                } 
            }
           
        }

        public string Name
        {
            get
                ;
            set
                ;
        }

        public ChainType ChainType
        {
            get
                ;
            set
                ;
        }
    }
}
