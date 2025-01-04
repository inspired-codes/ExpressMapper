using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InspiredCodes.ExpressMapper.Tests.Impl;

[TestClass]
public class MiscTest
{
    [TestMethod]
    public void DateTimeOffsetTypeTest()
    {
        string @empty = TypeCode.Empty.ToString();
        string @object = TypeCode.Object.ToString();

        TypeCode typeCode = Type.GetTypeCode(typeof(DateTimeOffset));
        Assert.IsTrue(Enum.TryParse(typeCode.ToString(), out TypeCode result));
        Assert.IsTrue(result.Equals(typeCode));

        if ((new string[] { @empty, @object }).Contains(typeCode.ToString()))
            Assert.Inconclusive($"for type codes kind of \"{@empty}\", \"{@object}\", special converting/parsing is needed");
    }
}
