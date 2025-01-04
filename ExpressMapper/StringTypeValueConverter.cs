/* ***************************************************************
 * @author: Peter Metz
 * @since: 2017
 * @change: 2020, 2022, 2024
 * ************************************************************** */

using System;
using System.Globalization;

namespace InspiredCodes.ExpressMapper;

/// <summary>
/// string type value converter
/// </summary>
public class StringTypeValueConverter
{
    /// <summary>
    /// Value Type to String Value converter
    /// </summary>
    public StringTypeValueConverter() { }

    protected static (bool Success, object Result) GetValue(string valueAsString, TypeCode typeCode)
    {
        bool success = false;
        object result = new();
        switch (typeCode)
        {
            case (TypeCode.DBNull):
                result = DBNull.Value;
                success = true;
                break;
            case (TypeCode.Boolean):
                success = Boolean.TryParse(valueAsString, out Boolean _bool);
                result = _bool;
                break;
            case (TypeCode.Byte):
                success = Byte.TryParse(valueAsString, out Byte _byte);
                result = _byte;
                break;
            case (TypeCode.Char):
                success = Char.TryParse(valueAsString, out Char _char);
                result = _char;
                break;
            case (TypeCode.DateTime):
                success = DateTime.TryParse(valueAsString, out DateTime _dateTime);
                result = _dateTime;
                break;
            case (TypeCode.Decimal):
                success = Decimal.TryParse(valueAsString, out Decimal _decimal);
                result = _decimal;
                break;
            case (TypeCode.Double):
                success = Double.TryParse(valueAsString, out Double _doubble);
                result = _doubble;
                break;
            case (TypeCode.Empty):
                //o is an Empty object, but anyway, System.Empty isn't accessible
                success = String.IsNullOrEmpty(valueAsString);
                result = new();
                break;
            case (TypeCode.Int16):
                success = Int16.TryParse(valueAsString, out Int16 _i16);
                result = _i16;
                break;

            case (TypeCode.Int32):
                success = Int32.TryParse(valueAsString, out Int32 _i32);
                result = _i32;
                break;
            case (TypeCode.Int64):
                success = Int64.TryParse(valueAsString, out Int64 _i64);
                result = _i64;
                break;
            case (TypeCode.Object):
                success = true;
                result = valueAsString ?? new object();
                break;
            case (TypeCode.SByte):
                success = SByte.TryParse(valueAsString, out SByte _sbyte);
                result = _sbyte;
                break;
            case (TypeCode.Single):
                success = Single.TryParse(valueAsString, out Single _single);
                result = _single;
                break;
            case (TypeCode.String):
                success = true;
                result = valueAsString ?? string.Empty;
                break;
            case (TypeCode.UInt16):
                success = UInt16.TryParse(valueAsString, out UInt16 _ui16);
                result = _ui16;
                break;
            case (TypeCode.UInt32):
                success = UInt32.TryParse(valueAsString, out UInt32 _ui32);
                result = _ui32;
                break;
            case (TypeCode.UInt64):
                success = UInt64.TryParse(valueAsString, out UInt64 _ui64);
                result = _ui64;
                break;
            default:
                success = false;
                result = new object();
                break;
        }

        return (success, result);
    }

    protected static (bool Success, object Result) ParseDateTimeOffset(string valueAsString)
    {
        bool success = DateTimeOffset.TryParse(valueAsString, out DateTimeOffset dto);
        return (success, dto);
    }

    /// <summary>
    /// CreateInstanceOf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T CreateInstanceOf<T>()
    {
        //DBNull
        if (typeof(DBNull) == typeof(T))
            return (T)(object)DBNull.Value;
        //String 
        else if (typeof(String) == typeof(T))
            return (T)(object)String.Empty;
        //all other TypeCodes
        else
            return Activator.CreateInstance<T>();
    }

    /// <summary>
    /// CreateInstanceOf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T CreateInstanceOf<T>(string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return CreateInstanceOf<T>();

        //DBNull
        if (typeof(DBNull) == typeof(T))
            return (T)(object)DBNull.Value;
        
        //String 
        else if (typeof(String) == typeof(T))
            return String.IsNullOrWhiteSpace(value) ?
                (T)(object)String.Empty : (T)(object)value;

        //all other TypeCodes
        else
        {
            try
            {
                T v = (T)Convert.ChangeType(value, typeof(T));
                return v;
            }
            catch
            {
                //not possible to convert the string to variable value
                return CreateInstanceOf<T>();
            }
        }
    }

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/system.typecode(v=vs.110).aspx
    /// </summary>
    /// <param name="typeName">Name of Type: MyVar.GetType().Name</param>
    /// <param name="valueAsString">Value in String representation with standard format specifier "r"</param>
    /// <param name="lcid">Microsoft Locale ID value</param>
    /// <param name="o">Object assigned to new native variable instance</param>
    /// <returns></returns>
    public bool StringToVariable(string typeName, string valueAsString, out object result)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            throw new ArgumentNullException(nameof(typeName));

        bool success = false;

        bool match = Enum.TryParse<TypeCode>(typeName, out TypeCode typeCode);
        if (!match)
            (typeName, typeCode) = TypeNameSanitizer.SanitizeTypeName(typeName);

        if (typeName == nameof(DateTimeOffset))
            (success, result) = ParseDateTimeOffset(valueAsString);


        (success, result) = GetValue(valueAsString, typeCode);
        return success;
    }
}
