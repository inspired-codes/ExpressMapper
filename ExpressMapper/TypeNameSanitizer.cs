using System;
using System.Collections.Generic;

namespace InspiredCodes.ExpressMapper;

public class TypeNameSanitizer
{

    public static readonly Dictionary<string, (string Nameof, Type Type)> MappedDateTimeOffset = new Dictionary<string, (string, Type)>()
    {
        { "DateTimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "dateTimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "DatetimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "System.DateTimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "System.DatetimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "system.DateTimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))},
        { "system.DatetimeOffset", ("DateTimeOffset", typeof(DateTimeOffset))}
    };

    /// <summary>
    /// int value same as TypeCode Enum
    /// </summary>
    public static readonly Dictionary<string, TypeCode> MappedTypeNames = new Dictionary<string, TypeCode>()
    {
//
        {"Empty", TypeCode.Empty},
        {"empty", TypeCode.Empty},
        {"System.Empty", TypeCode.Empty},
        {"system.Empty", TypeCode.Empty},
        {"system.empty", TypeCode.Empty},
//
        {"Object", TypeCode.Object},
        {"object", TypeCode.Object},
        {"System.Object", TypeCode.Object},
        {"system.Object", TypeCode.Object},
        {"system.object", TypeCode.Object},
//
        {"DBNull", TypeCode.DBNull},
        {"dBNull", TypeCode.DBNull},
        {"System.DBNull", TypeCode.DBNull},
        {"system.DBNull", TypeCode.DBNull},
        {"system.dbnull", TypeCode.DBNull},
//
        {"Boolean", TypeCode.Boolean},
        {"boolean", TypeCode.Boolean},
        {"System.Boolean", TypeCode.Boolean},
        {"system.Boolean", TypeCode.Boolean},
        {"system.boolean", TypeCode.Boolean},
//
        {"Char", TypeCode.Char},
        {"char", TypeCode.Char},
        {"System.Char", TypeCode.Char},
        {"system.Char", TypeCode.Char},
        {"system.char", TypeCode.Char},
//
        {"SByte", TypeCode.SByte},
        {"sByte", TypeCode.SByte},
        {"System.SByte", TypeCode.SByte},
        {"system.SByte", TypeCode.SByte},
        {"system.sbyte", TypeCode.SByte},
//
        {"Byte", TypeCode.Byte},
        {"byte", TypeCode.Byte},
        {"System.Byte", TypeCode.Byte},
        {"system.Byte", TypeCode.Byte},
        {"system.byte", TypeCode.Byte},
//
        {"Int16", TypeCode.Int16},
        {"int16", TypeCode.Int16},
        {"System.Int16", TypeCode.Int16},
        {"system.Int16", TypeCode.Int16},
        {"system.int16", TypeCode.Int16},
//
        {"UInt16", TypeCode.UInt16},
        {"uInt16", TypeCode.UInt16},
        {"System.UInt16", TypeCode.UInt16},
        {"system.UInt16", TypeCode.UInt16},
        {"system.uint16", TypeCode.UInt16},
//
        {"Int32", TypeCode.Int32},
        {"int32", TypeCode.Int32},
        {"System.Int32", TypeCode.Int32},
        {"system.Int32", TypeCode.Int32},
        {"system.int32", TypeCode.Int32},
//
        {"UInt32", TypeCode.UInt32},
        {"uInt32", TypeCode.UInt32},
        {"System.UInt32", TypeCode.UInt32},
        {"system.UInt32", TypeCode.UInt32},
        {"system.uint32", TypeCode.UInt32},
//
        {"Int64", TypeCode.Int64},
        {"int64", TypeCode.Int64},
        {"System.Int64", TypeCode.Int64},
        {"system.Int64", TypeCode.Int64},
        {"system.int64", TypeCode.Int64},
//
        {"UInt64", TypeCode.UInt64},
        {"uInt64", TypeCode.UInt64},
        {"System.UInt64", TypeCode.UInt64},
        {"system.UInt64", TypeCode.UInt64},
        {"system.uint64", TypeCode.UInt64},
//
        {"Single", TypeCode.Single},
        {"single", TypeCode.Single},
        {"System.Single", TypeCode.Single},
        {"system.Single", TypeCode.Single},
        {"system.single", TypeCode.Single},
//
        {"Double", TypeCode.Double},
        {"double", TypeCode.Double},
        {"System.Double", TypeCode.Double},
        {"system.Double", TypeCode.Double},
        {"system.double", TypeCode.Double},
//
        {"Decimal", TypeCode.Decimal},
        {"decimal", TypeCode.Decimal},
        {"System.Decimal", TypeCode.Decimal},
        {"system.Decimal", TypeCode.Decimal},
        {"system.decimal", TypeCode.Decimal},
//
        {"DateTime", TypeCode.DateTime},
        {"dateTime", TypeCode.DateTime},
        {"System.DateTime", TypeCode.DateTime},
        {"system.DateTime", TypeCode.DateTime},
        {"system.datetime", TypeCode.DateTime},
//
        {"String", TypeCode.String},
        {"string", TypeCode.String},
        {"System.String", TypeCode.String},
        {"system.String", TypeCode.String},
        {"system.string", TypeCode.String}
    };

    public static (string, TypeCode) SanitizeTypeName(string twistedName)
    {
        if (string.IsNullOrWhiteSpace(twistedName))
            if (null == twistedName)
                throw new ArgumentNullException(nameof(twistedName));
            else
                throw new ArgumentException(nameof(twistedName));

        string key = twistedName.Trim();
        if (MappedTypeNames.ContainsKey(key))
            return (MappedTypeNames[key].ToString(), MappedTypeNames[key]);

        if (MappedDateTimeOffset.ContainsKey(key))
            return (MappedDateTimeOffset[key].Nameof, Type.GetTypeCode(MappedDateTimeOffset[key].Type));

        throw new ArgumentException(nameof(twistedName), $@"can not match type for {twistedName}");
    }
}
