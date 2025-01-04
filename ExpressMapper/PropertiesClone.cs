/* ***************************************************************
 * @author: Peter Metz
 * @since: summer 2017
 * @change: 2022, 2025
 * @copyright: Peter Metz
 * ************************************************************** */

using System.Collections.Generic;
using System.Reflection;
using System;

namespace InspiredCodes.ExpressMapper;

public class PropertiesClone
{
    protected static Dictionary<string, PropertyInfo> GetPropertyNamesInfos(
        Type type, 
        out Dictionary<string, PropertyInfo> propertiesNames, 
        out PropertyInfo[] propertyInfos)
    {
        propertyInfos = type.GetProperties();
        
        int _len = propertyInfos.Length;
        propertiesNames = new Dictionary<string, PropertyInfo>(_len);

        for (int i = 0; i < _len; i++)
            propertiesNames[propertyInfos[i].Name] = propertyInfos[i];

        return propertiesNames;
    }

}