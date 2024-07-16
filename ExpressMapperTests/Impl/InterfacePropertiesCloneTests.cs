using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InspiredCodes.ExpressMapper.Tests.Model;

namespace InspiredCodes.ExpressMapper.Tests.Impl;


[TestClass]
public class InterfacePropertiesCloneTests
{
    /// <summary>
    /// testing with interface including an inherited interface "grandchild"
    /// </summary>
    [TestMethod]
    public void CopyPropertiesTest()
    {
        var stopw = new Stopwatch();

        var propertyNames = new string[] { "Derived_2", "Derived_1", "Base", "BaseB" };

        var src = new Derived2 { Derived_1 = "Derived_1", Base = "Base", BaseB = "BaseB", Derived_2 = "Derived_2" };
        var trgt = new Derived2();

        stopw.Start();
        for (var i = 0; i < 1000; i++)
        {
            InterfacePropertiesClone.CopyValues<Derived2, Derived2, IDerived2>(src, trgt);
            Assert.AreEqual(src.ToString(), trgt.ToString());
        }
        stopw.Stop();

        Debug.WriteLine(stopw.ElapsedMilliseconds);

        var empty = new Derived2();
        InterfacePropertiesClone.CopyValues<Derived2, Derived2, IDerived2>(empty, trgt);
        Assert.AreEqual(empty.ToString(), trgt.ToString());

        InterfacePropertiesClone.CopyValues<Derived2, Derived2, IBaseB>(src, trgt);
        Assert.AreEqual(new Derived2 { BaseB = "BaseB" }.ToString(), trgt.ToString());

        return;
    }

    /// <summary>
    /// testing with interface including an inherited interface "grandchild"
    /// </summary>
    [TestMethod]
    public void DeepCopyPropertiesTest()
    {
        var stopw = new Stopwatch();

        var propertyNames = new string[] { "Derived_2", "Derived_1", "Base", "BaseB" };

        var src = new Derived2
        {
            Derived_1 = "Derived_1",
            Base = "Base",
            BaseB = "BaseB",
            Derived_2 = "Derived_2",
            EnumA = AnotherEnum.one,
            IntArr1 = new int[] { 1, 2, 3, 4, 5, 6, 0 },
            B1 = 0x2a,
            DateTime = DateTime.Now,
            YesOrNo = true
        };

        var trgt = new Derived2();

        stopw.Start();
        for (var i = 0; i < 1000; i++)
        {
            try
            {
                InterfacePropertiesClone.CopyValues<Derived2, Derived2, IDerived2>(src, trgt);
                Assert.IsFalse(!src.ToString().Equals(trgt.ToString()) ||
                    !trgt.BaseB.Equals("BaseB"));

            }
            catch
            {
                throw;
            }

        }
        stopw.Stop();

        Debug.WriteLine(stopw.ElapsedMilliseconds);

        return;
    }
}
