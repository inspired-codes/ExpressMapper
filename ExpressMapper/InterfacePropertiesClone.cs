/* ***************************************************************
 * @author: Peter Metz
 * @since: summer 2017
 * @change: 2022, 2024
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
public class InterfacePropertiesClone : PropertiesClone
{
    private static object lock_iftpd = new object();

    static Dictionary<Type, Type[]> InterfaceChildInterfaceTypeDict { get; } = [];
    static Dictionary<Type, Dictionary<string, PropertyInfo>> TypePropertyDict { get; } = [];
    static Dictionary<Type, HashSet<string>> InterfaceTypePropertyDict { get; } = [];

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
    static void GetPropertyMetadata<S, T, I>(out HashSet<string> allPropertyNames,
                                       out Type targetType,
                                       out Dictionary<string, PropertyInfo> sourceProperties,
                                       out Dictionary<string, PropertyInfo> targetProperties)
    {
        // source, get sorted list with property name and property info
        Type sourceType = typeof(S);
        PropertyInfo[] _ppts;

        if (!TypePropertyDict.TryGetValue(sourceType, out sourceProperties))
        {
            TypePropertyDict[sourceType] = GetPropertyNamesInfos(out sourceProperties, sourceType, out _ppts);
        }

        // target, get sorted list with property name and property info
        targetType = typeof(T); // target.GetType();
        if (!TypePropertyDict.TryGetValue(targetType, out targetProperties))
        {
            NewMethod(targetType, out targetProperties, out _ppts);
        }

        // interface
        Type interfaceType = typeof(I);
        if (InterfaceTypePropertyDict.ContainsKey(interfaceType))
        {
            allPropertyNames = InterfaceTypePropertyDict[interfaceType];
            return;
        }

        //Dictionary<string, PropertyInfo> interfaceProperties;// = new SortedList<string, PropertyInfo>();
        if (!TypePropertyDict.TryGetValue(interfaceType, out Dictionary<string, PropertyInfo> interfaceProperties))
        {
            _ppts = interfaceType.GetProperties();
            var _len = _ppts.Length;
            interfaceProperties = new Dictionary<string, PropertyInfo>(_len);

            for (int i = 0; i < _len; i++)
                interfaceProperties[_ppts[i].Name] = _ppts[i];

            TypePropertyDict[interfaceType] = interfaceProperties;
        }

        lock (lock_iftpd)
            InterfaceTypePropertyDict[interfaceType] = new HashSet<string>(interfaceProperties.Select(p => p.Key).ToArray());

        /* child interfaces */
        // get all properties of child interface
        Type[] iChildInterfaces;
        if (!InterfaceChildInterfaceTypeDict.TryGetValue(interfaceType, out iChildInterfaces))
        {
            var tmpI = interfaceType.GetInterfaces();
            InterfaceChildInterfaceTypeDict[interfaceType] = tmpI;
            iChildInterfaces = tmpI;
        }

        // get all properties of child-interfaces
        int _ciLen = iChildInterfaces.Length;
        TypeInfo childInterfaceType;
        for (int i = 0; i < _ciLen; i++)
        {
            childInterfaceType = iChildInterfaces[i].GetTypeInfo();
            if (!TypePropertyDict.TryGetValue(childInterfaceType, out Dictionary<string, PropertyInfo> childInterfaceProperties))
            {
                _ppts = childInterfaceType.GetProperties();
                int _len = _ppts.Length;

                childInterfaceProperties = new Dictionary<string, PropertyInfo>(_len);
                for (int j = 0; j < _len; j++)
                    childInterfaceProperties[_ppts[j].Name] = _ppts[j];

                TypePropertyDict[childInterfaceType] = childInterfaceProperties;
            }

            //allPropertyNames.UnionWith(childInterfaceProperties.Select(p => p.Key));

            InterfaceTypePropertyDict[interfaceType].UnionWith(childInterfaceProperties.Select(p => p.Key).ToArray());
        }

        allPropertyNames = InterfaceTypePropertyDict[interfaceType];
    }

    private static Dictionary<string, PropertyInfo> NewMethod1(out Dictionary<string, PropertyInfo> sourceProperties, Type sourceType, out PropertyInfo[] _ppts)
    {
        _ppts = sourceType.GetProperties();
        var _len = _ppts.Length;
        sourceProperties = new Dictionary<string, PropertyInfo>(_len);

        for (int i = 0; i < _len; i++)
            sourceProperties[_ppts[i].Name] = _ppts[i];

        return sourceProperties;
    }

    private static void NewMethod(Type targetType, out Dictionary<string, PropertyInfo> targetProperties, out PropertyInfo[] _ppts)
    {
        _ppts = targetType.GetProperties();
        var _len = _ppts.Length;
        targetProperties = new Dictionary<string, PropertyInfo>(_len);

        for (int i = 0; i < _len; i++)
            targetProperties[_ppts[i].Name] = _ppts[i];

        TypePropertyDict[targetType] = targetProperties;
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
    public static int CopyValues<S, T, I>(S source, T target) where S : I where T : class, I
    {
        // check for null reference
        if (ReferenceEquals(null, source) || ReferenceEquals(null, target))
            throw new NullReferenceException("source or target must not be null");
        // check if I is interface
        if (!typeof(I).IsInterface)
            throw new Exception("I must be an interface type");

        GetPropertyMetadata<S, T, I>(out HashSet<string> allPropertyNames,
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
    public static int CopyValues<S, T, I>(S source, ref T target) where S : I where T : struct, I
    {
        // check for null reference, T is a struct, not nullable
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        // check if I is interface
        if (!typeof(I).IsInterface)
            throw new ArgumentException("type of argument I must be an interface type", nameof(I));

        var count = 0;

        GetPropertyMetadata<S, T, I>(out HashSet<string> allPropertyNames,
                                     out Type targetType,
                                     out Dictionary<string, PropertyInfo> sourceProperties,
                                     out Dictionary<string, PropertyInfo> targetProperties);

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
