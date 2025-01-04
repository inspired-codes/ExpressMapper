using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InspiredCodes.ExpressMapper.Tests.Impl;

using Converter = InspiredCodes.ExpressMapper.StringTypeValueConverter;

[TestClass()]
public class StringTypeValueConverterTests
{
    Collection<KeyValuePair<string, object>> sTypeObjectCollection;
    Collection<KeyValuePair<string, string>> sTypeSValueCollection;

    [TestMethod()]
    public void CreateInstanceOfTest()
    {
        StringTypeValueConverter stvc = new StringTypeValueConverter();
        for (int i = 0; i < 1000; i++)
        {
            object obj = Converter.CreateInstanceOf<object>(null);
            Assert.IsNotNull(obj);

            short i16 = Converter.CreateInstanceOf<short>(short.MaxValue.ToString());
            Assert.IsNotNull(i16);
            Assert.IsTrue(i16 == short.MaxValue);

            long i64 = Converter.CreateInstanceOf<long>(long.MaxValue.ToString());
            Assert.IsNotNull(i64);
            Assert.IsTrue(i64 == long.MaxValue);

            DateTime dateTime = Converter.CreateInstanceOf<DateTime>(DateTime.MinValue.ToString("o"));
            Assert.IsNotNull(dateTime);
            Assert.IsTrue(dateTime.Equals(DateTime.MinValue));

            string s = Converter.CreateInstanceOf<string>("908709756^°ß@\\#'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(s));
            Assert.IsTrue(s == "908709756^°ß@\\#'");

            DBNull dbNull = DBNull.Value;
            DBNull sNull = Converter.CreateInstanceOf<DBNull>(dbNull.ToString());
            Assert.IsTrue(sNull is DBNull);
            Assert.IsTrue(sNull == DBNull.Value);

            decimal m = Converter.CreateInstanceOf<decimal>(983.39M.ToString());
            Assert.IsNotNull(m);
            Assert.IsTrue(983 < m);

            decimal m2 = Converter.CreateInstanceOf<decimal>(string.Empty);
            Assert.IsNotNull(m2);
            Assert.IsTrue(m2 == 0);
        }
    }

    [TestMethod()]
    public void StringToVariableTest()
    {
        StringTypeValueConverter cvtr = new StringTypeValueConverter();
        Type t;
        object obj;

        bool _boolean = true;
        t = _boolean.GetType();
        cvtr.StringToVariable(t.FullName, _boolean.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_boolean));

        byte _byte = byte.MinValue;
        t = _byte.GetType();
        cvtr.StringToVariable(t.Name, _byte.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_byte));

        byte _byteMax = byte.MaxValue;
        t = _byteMax.GetType();
        cvtr.StringToVariable(t.FullName, _byteMax.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_byteMax));

        sbyte _sbyte = sbyte.MaxValue;
        t = _sbyte.GetType();
        cvtr.StringToVariable(t.Name, _sbyte.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_sbyte));

        char _char = 'c';
        t = _char.GetType();
        cvtr.StringToVariable(t.FullName, _char.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_char));

        decimal _decimal = 99089890.901123M;
        t = _decimal.GetType();
        cvtr.StringToVariable(t.Name, _decimal.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_decimal));

        double _double = 7892345.98979087;
        t = _double.GetType();
        cvtr.StringToVariable(t.FullName, _double.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_double));

        float _float = 8.098f;
        t = _float.GetType();
        cvtr.StringToVariable(t.Name, _float.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_float));

        int _int = int.MaxValue;
        t = _int.GetType();
        cvtr.StringToVariable(t.FullName, _int.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_int));

        int _intMin = int.MinValue;
        t = _int.GetType();
        cvtr.StringToVariable(t.FullName, _intMin.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_intMin));

        uint _uint = uint.MaxValue;
        t = _uint.GetType();
        cvtr.StringToVariable(t.Name, _uint.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_uint));

        long _long = long.MaxValue;
        t = _long.GetType();
        cvtr.StringToVariable(t.FullName, _long.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_long));

        long _longMin = long.MinValue;
        t = _longMin.GetType();
        cvtr.StringToVariable(t.Name, _longMin.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_longMin));

        ulong _ulong = ulong.MaxValue;
        t = _ulong.GetType();
        cvtr.StringToVariable(t.FullName, _ulong.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_ulong));

        object _object = new object();
        t = _object.GetType();
        cvtr.StringToVariable(t.Name, _object.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_object.ToString()));

        short _short = short.MinValue;
        t = _short.GetType();
        cvtr.StringToVariable(t.FullName, _short.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_short));

        ushort _ushort = ushort.MaxValue;
        t = _ushort.GetType();
        cvtr.StringToVariable(t.Name, _ushort.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_ushort));

        string _string = string.Empty;
        t = _string.GetType();
        cvtr.StringToVariable(t.FullName, _string.ToString(), out obj);
        Assert.IsTrue(obj.Equals(_string));
    }

    [TestInitialize()]
    public void InitializeTest()
    {
        sTypeObjectCollection = new Collection<KeyValuePair<string, object>>();
        sTypeSValueCollection = new Collection<KeyValuePair<string, string>>();
    }

    [TestMethod()]
    public void MiscTest()
    {
        StringTypeValueConverter sTVConverter = new StringTypeValueConverter();
        Stopwatch stopwatch = new Stopwatch();
        StringBuilder sb = new StringBuilder();
        /*1 TryParse*/
        Debug.WriteLine("1 TryParse YesOrNo");
        stopwatch.Start();
        for (int i = 0; i < 1000; i++)
        {
            decimal m = i % 3 + 88.98M;
            decimal out_;
            decimal.TryParse(m.ToString(), out out_);

            sb.Append(out_.GetType().Name).Append(":").AppendLine(out_.ToString());
        }
        stopwatch.Stop();
        Debug.WriteLine($" elapsed ms: {stopwatch.ElapsedMilliseconds}");

        /*2 CreateInstanceOf*/
        Debug.WriteLine("2 CreateInstanceOf YesOrNo");
        sb.Clear();
        CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
        stopwatch.Restart();
        for (int i = 0; i < 333; i++)
        {
            decimal m = i % 3 + 88.98M;
            object out_ = Converter.CreateInstanceOf<decimal>(m.ToString());
            sb.Append(" : ").Append(out_.ToString());
        }
        stopwatch.Stop();
        Debug.WriteLine($" elapsed ms: {stopwatch.ElapsedMilliseconds}");

        /*3 Dictionary*/
        Debug.WriteLine("3 Dictionary YesOrNo");
        sb.Clear();
        stopwatch.Restart();
        Dictionary<int, int> LICD_CP = new Dictionary<int, int>();
        {
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
            {
                int cp = ci.TextInfo.ANSICodePage;
                int licd = ci.LCID;
                LICD_CP.Add(licd, cp);
            }
        }
        stopwatch.Stop();
        Debug.WriteLine($" elapsed ms: {stopwatch.ElapsedMilliseconds}");

        /*4 GetDecimalFromString*/
        Debug.WriteLine("4 GetDecimalFromString YesOrNo");
        sb.Clear();
        stopwatch.Restart();
        for (int i = 0; i < 1000; i++)
        {
            decimal m = i + 88.98M;
            object out_;
            //int LCID = 1031;
            bool success = sTVConverter.StringToVariable(m.GetType().Name, m.ToString(), out out_);
            sb.Append(" : ").Append(out_.ToString());
            if (i == 1)
                Debug.WriteLine($"{out_.GetType().Name}");
        }
        stopwatch.Stop();
        Debug.WriteLine($" elapsed ms: {stopwatch.ElapsedMilliseconds}");
    }
}