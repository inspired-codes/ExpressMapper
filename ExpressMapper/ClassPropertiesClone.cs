/* ***************************************************************
 * @author: Peter Metz
 * @since: summer 2017
 * @change: 2022, 2024, 2025
 * @copyright: Peter Metz
 * ************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InspiredCodes.ExpressMapper;

/// <summary>
/// copy by value
/// support for value types like enum and struct
/// </summary>
public class ClassPropertiesClone : PropertiesClone
{
    static Dictionary<Type, HashSet<string>> ClassTypePropertyDict { get; } = [];

    /// <summary>
    /// gets and caches the properties of type, 
    /// at second run the properties are not fetechd by reflection but from cache
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <param name="allPropertyNames"></param>
    /// <param name="targetType"></param>
    /// <param name="sourceProperties"></param>
    /// <param name="targetProperties"></param>
    static void GetPropertyMetadata<S, T>(out HashSet<string> allPropertyNames,
                                       out Type targetType,
                                       out Dictionary<string, PropertyInfo> sourceProperties,
                                       out Dictionary<string, PropertyInfo> targetProperties) //where T : S
    {
        // source, get sorted list with property name and property info
        Type sourceType = typeof(S);
        PropertyInfo[] _ppts;

        if (!TypePropertyDict.TryGetValue(sourceType, out sourceProperties))
        {
            TypePropertyDict[sourceType] = GetPropertyNamesInfos(sourceType, out sourceProperties,  out _ppts);
        }

        // target, get sorted list with property name and property info
        targetType = typeof(T); // target.GetType();
        if (!TypePropertyDict.TryGetValue(targetType, out targetProperties))
        {
            TypePropertyDict[targetType] = GetPropertyNamesInfos(targetType, out targetProperties, out _ppts);
        }

        //Dictionary<string, PropertyInfo> interfaceProperties;// = new SortedList<string, PropertyInfo>();
        if (!TypePropertyDict.TryGetValue(sourceType, out Dictionary<string, PropertyInfo> interfaceProperties))
        {
            _ppts = sourceType.GetProperties();
            var _len = _ppts.Length;
            interfaceProperties = new Dictionary<string, PropertyInfo>(_len);

            for (int i = 0; i < _len; i++)
                interfaceProperties[_ppts[i].Name] = _ppts[i];

            TypePropertyDict[sourceType] = interfaceProperties;
        }

        lock (LockAddKV)
            ClassTypePropertyDict[sourceType] = new HashSet<string>(interfaceProperties.Select(p => p.Key).ToArray());

        allPropertyNames = ClassTypePropertyDict[sourceType];
    }





    /// <summary>
    /// copies values from source instance to target for instances 
    /// which do share same interface type I
    /// properties from inherited interfaces are included
    /// method used for TARGET type CLASS
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns>count of skipped properties</returns>
    public static int CopyValues<S, T>(S source, T target) where T : S
    {
        // check for null reference
        if (ReferenceEquals(null, source) || ReferenceEquals(null, target))
            throw new NullReferenceException("source or target must not be null");

        GetPropertyMetadata<S, T>(out HashSet<string> allPropertyNames,
                                     out Type targetType,
                                     out Dictionary<string, PropertyInfo> sourceProperties,
                                     out Dictionary<string, PropertyInfo> targetProperties);

        int count = 0;

        if (targetType.IsValueType)
        {
            var boxed = (object)target;
            //copy values property by property using boxing
            foreach (string pName in allPropertyNames)
            {
                var propTarget = targetProperties[pName];
                var propSource = sourceProperties[pName];

                if ((!targetProperties[pName].CanWrite) ||
                    (!sourceProperties[pName].CanRead) ||
                    (!propTarget.PropertyType.Equals(propSource.PropertyType))
                   )
                    continue; // skip if not accessible
                else
                    propTarget.SetValue(boxed, propSource.GetValue(source));
                count++;
            }
        }
        else
        {
            //copy values property by property
            foreach (string pName in allPropertyNames)
            {
                var propTarget = targetProperties[pName];
                var propSource = sourceProperties[pName];

                if ((!targetProperties[pName].CanWrite) ||
                    (!sourceProperties[pName].CanRead) ||
                    (!propTarget.PropertyType.Equals(propSource.PropertyType))
                   )
                    continue; // skip if not accessible
                else
                    propTarget.SetValue(target, propSource.GetValue(source));
                count++;
            }
        }

        // count of not copied properties
        return allPropertyNames.Count - count;
    }

    /// <summary>
    /// copies values from source instance to target for instances 
    /// which do share same interface type I
    /// properties from inherited interfaces are included
    /// method used for TARGET type STRUCT
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static int CopyValues<S, T>(S source, ref T target) where T : struct, S
    {
        // check for null reference, T is a struct, not nullable
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        GetPropertyMetadata<S, T>(out HashSet<string> allPropertyNames,
                                  out Type targetType,
                                  out Dictionary<string, PropertyInfo> sourceProperties,
                                  out Dictionary<string, PropertyInfo> targetProperties);

        int count = 0;
        if (targetType.IsValueType)
        {
            var boxed = (object)target;
            //copy values property by property using boxing
            foreach (string pName in allPropertyNames)
            {
                var propTarget = targetProperties[pName];
                var propSource = sourceProperties[pName];

                if ((!targetProperties[pName].CanWrite) ||
                    (!sourceProperties[pName].CanRead) ||
                    (!propTarget.PropertyType.Equals(propSource.PropertyType))
                   )
                    continue; // skip if not accessible
                else
                    propTarget.SetValue(boxed, propSource.GetValue(source));
                count++;

            }
            target = (T)boxed;
        }
        else
        {
            //copy values property by property
            foreach (string pName in allPropertyNames)
            {
                var propTarget = targetProperties[pName];
                var propSource = sourceProperties[pName];

                if ((!targetProperties[pName].CanWrite) ||
                    (!sourceProperties[pName].CanRead) ||
                    (!propTarget.PropertyType.Equals(propSource.PropertyType))
                   )
                    continue; // skip if not accessible
                else
                    propTarget.SetValue(target, propSource.GetValue(source));
                count++;
            }
        }

        // count of not copied properties
        return allPropertyNames.Count - count;
    }
}
