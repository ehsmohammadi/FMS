namespace MITD.Fuel.Domain.Model.Factories
{
    public interface IOrderCodeGenerator:ICodeGenerator
    {
        string GenerateNewCode();
    }
}