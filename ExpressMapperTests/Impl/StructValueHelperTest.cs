using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InspiredCodes.ExpressMapper.Tests.Impl;

[TestClass]
public class StructValueHelperTest
{

    [TestMethod]
    public void CopyClass2StructTest()
    {
        var sourceObj = new ClassObject1();
        sourceObj.MyEnum = MyEnum.hello;
        sourceObj.MyIntProperty = int.MaxValue;
        sourceObj.MyStringProperty = "HELLO";

        var cType = typeof(ClassObject1);
        { }
        var targetObj = new ClassObject2();
        var sType = targetObj.GetType();
        var g2 = targetObj.MyId;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < 10000; i++)
        {
            //int sourceValInt = (int)cType.GetProperty("MyIntProperty").GetValue(sourceObj);
            //string sourceValStrg = (string)cType.GetProperty("MyStringProperty").GetValue(sourceObj);

            InterfacePropertiesClone.CopyValues<ClassObject1, ClassObject2, IObjectInterface>(sourceObj, targetObj);

            //int targetValInt = (int)sType.GetProperty("MyIntProperty").GetValue(targetObj);
            //string targetValStrg = (string)sType.GetProperty("MyStringProperty").GetValue(targetObj);
            //Assert.AreEqual(sourceValInt, targetValInt);
            //Assert.AreEqual(sourceValStrg, targetValStrg);
            Assert.AreEqual((int)sourceObj.MyEnum, (int)targetObj.MyEnum);
            Assert.AreEqual(sourceObj.ChildStringProperty, targetObj.ChildStringProperty);
            sourceObj.MyIntProperty++;
            sourceObj.MyStringProperty = $@"hallo struct {i}";
            sourceObj.ChildStringProperty = $@"hello kitty struct {i}";
            sourceObj.MyEnum = (MyEnum)(i % 5);
        }
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;

        { }
        var targetObj2 = new StructObject2();
        sType = targetObj2.GetType();
        g2 = targetObj2.MyId;

        stopwatch.Restart();
        for (int i = 0; i < 10000; i++)
        {
            //int sourceValInt = (int)cType.GetProperty("MyIntProperty").GetValue(sourceObj);
            //string sourceValStrg = (string)cType.GetProperty("MyStringProperty").GetValue(sourceObj);

            InterfacePropertiesClone.CopyValues<ClassObject1, StructObject2, IObjectInterface>(sourceObj, ref targetObj2);

            //int targetValInt = (int)sType.GetProperty("MyIntProperty").GetValue(targetObj2);
            //string targetValStrg = (string)sType.GetProperty("MyStringProperty").GetValue(targetObj2);
            //Assert.AreEqual(sourceValInt, targetValInt);
            //Assert.AreEqual(sourceValStrg, targetValStrg);
            Assert.AreEqual((int)sourceObj.MyEnum, (int)targetObj2.MyEnum);
            Assert.AreEqual(sourceObj.ChildStringProperty, targetObj2.ChildStringProperty);
            sourceObj.MyIntProperty++;
            sourceObj.MyStringProperty = $@"hallo struct {i}";
            sourceObj.ChildStringProperty = $@"hello kitty struct {i}";
            sourceObj.MyEnum = (MyEnum)(i % 5);
        }
        stopwatch.Stop();
        var elapsed2 = stopwatch.ElapsedMilliseconds;

        var g3 = targetObj.MyId;

        return;
    }

}

/*  ------------------------------------------------------- */
public interface IObjectInterface : IChildInterface
{
    MyEnum MyEnum { get; set; }
    int MyIntProperty { get; set; }
    string MyStringProperty { get; set; }
}

public interface IChildInterface
{
    string ChildStringProperty { get; set; }
}

public class ClassObject1 : IObjectInterface
{
    public string ChildStringProperty { get; set; }

    public MyEnum MyEnum { get; set; }
    public int MyIntProperty { get; set; }

    public string MyStringProperty { get; set; }
}

public class ClassObject2 : IObjectInterface
{
    public string ChildStringProperty { get; set; }
    public MyEnum MyEnum { get; set; }
    public Guid MyId { get; private set; } = new Guid();
    public int MyIntProperty { get; set; }
    public string MyStringProperty { get; set; }
}

public struct StructObject2 : IObjectInterface
{
    public StructObject2(bool init = true)
    {
        MyId = Guid.NewGuid();
        MyEnum = MyEnum.none;
        MyIntProperty = 0;
        MyStringProperty = string.Empty;
        ChildStringProperty = string.Empty;
    }

    public string ChildStringProperty { get; set; }
    public MyEnum MyEnum { get; set; }

    public Guid MyId { get; private set; }
    public int MyIntProperty { get; set; }
    public string MyStringProperty { get; set; }
}

public enum MyEnum
{
    none = 0,
    hello = 1,
    du = 2,
    cooler = 3,
    vogel = 4
}
