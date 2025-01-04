/* ***************************************************************
 * @author: Peter Metz
 * @since: summer 2017
 * @change: 2022, 2024
 * @copyright: Peter Metz
 * ************************************************************** */

using System.Collections.Generic;
using System.Reflection;
using System;

namespace InspiredCodes.ExpressMapper;

public class PropertiesClone
{
    protected static Dictionary<string, PropertyInfo> GetPropertyNamesInfos(
        out Dictionary<string, PropertyInfo> sourceProperties, 
        Type sourceType, 
        out PropertyInfo[] propertyInfos)
    {
        propertyInfos = sourceType.GetProperties();
        int _len = propertyInfos.Length;
        sourceProperties = new Dictionary<string, PropertyInfo>(_len);

        for (int i = 0; i < _len; i++)
            sourceProperties[propertyInfos[i].Name] = propertyInfos[i];

        return sourceProperties;
    }

}