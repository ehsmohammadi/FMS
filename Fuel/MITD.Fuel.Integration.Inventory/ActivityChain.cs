using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MITD.Fuel.Integration.Inventory
{
    public class ActivityChain<T> : IChain
    {
        private readonly Func<bool> _func;
        private readonly Func<Tuple<bool, List<T>>> _tuplFunc;
        private readonly Func<Tuple<bool, List<T>, List<T>>> _tuplFunc3p;
        private IChain _chain;
        public List<T> OutPutsList { get; set; }
        public List<T> OutPutsList1 { get; set; }
        public ActivityChain(string name, Func<bool> func)
        {
            _func = func;
            Name = name;
            ChainType = ChainType.Activity;
            OutPutsList = new List<T>();
            OutPutsList1 = new List<T>();
        }
        public ActivityChain(string name, Func<System.Tuple<bool, List<T>>> tuplFunc)
        {
            _tuplFunc = tuplFunc;
            OutPutsList = new List<T>();
            OutPutsList1 = new List<T>();
            Name = name;
            ChainType = ChainType.Condition;
        }
        public ActivityChain(string name, Func<System.Tuple<bool, List<T>, List<T>>> tuplFunc)
        {
            _tuplFunc3p = tuplFunc;
            OutPutsList = new List<T>();
            OutPutsList1 = new List<T>();

            Name = name;
            ChainType = ChainType.Condition;
        }
        public void HandleRequest()
        {
        
            OutPutsList.Clear();
            OutPutsList1.Clear();
            if (_tuplFunc3p != null)
            {
                var res = _tuplFunc3p.Invoke();

                if (res.Item2 != null)
                    OutPutsList.AddRange(res.Item2);
                if (res.Item3 != null)
                    OutPutsList1.AddRange(res.Item3);

                _chain.HandleRequest();
            }
            else if (_tuplFunc != null)
            {
                var res = _tuplFunc.Invoke();

                if (res.Item2 != null)
                    OutPutsList.AddRange(res.Item2);

                _chain.HandleRequest();
            }
            else
            {
                var res = _func.Invoke();
                if (_chain != null)
                    _chain.HandleRequest(); 
            }



        }
        public void SetChain(IChain nextChain)
        {
            _chain = nextChain;
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
