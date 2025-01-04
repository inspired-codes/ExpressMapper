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
        Type targetType, 
        out Dictionary<string, PropertyInfo> targetProperties, 
        out PropertyInfo[] _ppts)
    {
        _ppts = targetType.GetProperties();
        var _len = _ppts.Length;
        targetProperties = new Dictionary<string, PropertyInfo>(_len);

        for (int i = 0; i < _len; i++)
            targetProperties[_ppts[i].Name] = _ppts[i];

        return targetProperties;
    }

}