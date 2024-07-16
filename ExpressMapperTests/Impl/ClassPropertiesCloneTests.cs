using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InspiredCodes.ExpressMapper.Tests.Model;

namespace InspiredCodes.ExpressMapper.Tests.Impl;

[TestClass]
public class ClassPropertiesCloneTests
{
    [TestMethod]
    public void CopyPropertiesTest()
    {
        var stopw = new Stopwatch();

        var src = new Source
        {
            B1 = 1,
            B2 = 2,
            DateTime = new DateTime(2024, 12, 14, 12, 30, 1),
            String1 = "a",
            YesOrNo = true
        };
        var trgt = new Target();

        stopw.Start();
        for (var i = 0; i < 1000; i++)
        {
            ClassPropertiesClone.CopyValues<Source, Target>(src, trgt);
            Assert.AreEqual(src.ThisToJson(), trgt.ThisAsSourceToJson());
        }
        stopw.Stop();

        Debug.WriteLine(stopw.ElapsedMilliseconds);

        var empty = new Source();
        ClassPropertiesClone.CopyValues(empty, trgt);
        Assert.AreEqual(empty.ThisToJson(), trgt.ThisAsSourceToJson());

        ClassPropertiesClone.CopyValues(src, trgt);
        Assert.AreEqual(src.ThisToJson(), trgt.ThisAsSourceToJson());

    }
}
