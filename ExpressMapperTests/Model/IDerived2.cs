//using Newtonsoft.Json;
namespace InspiredCodes.ExpressMapper.Tests.Model;

public interface IDerived2 : IDerived1, IBaseA
{
    string Derived_2 { get; set; }
}
