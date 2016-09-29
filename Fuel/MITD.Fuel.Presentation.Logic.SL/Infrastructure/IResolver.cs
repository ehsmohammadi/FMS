using Castle.Windsor;
using MITD.Core;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public interface IResolver<T> where T : class
    {
        T Resolve();
        T Resolve(params object[] parameters);
    }

    public class Resolver<T> : IResolver<T> where T:class 
    {
        public T Resolve()
        {
            var result = ServiceLocator.Current.GetInstance<T>();
            return result;
        }
        public T Resolve(params object[] parameters)
        {
            var container = ServiceLocator.Current.GetInstance<IWindsorContainer>();
            var result =container.Resolve<T>(parameters);
            return result;
        }
    }
}