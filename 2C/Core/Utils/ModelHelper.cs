using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Core.Attributes;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Utils
{
    public static class ModelHelper
    {
        static Dictionary<Type, List<MappingInfo>> _mappingCache = new Dictionary<Type, List<MappingInfo>>();
        static Dictionary<Type, string> _tableNamesCache = new Dictionary<Type, string>();
        static Dictionary<Type, string> _idFieldsCache = new Dictionary<Type, string>();
        static Dictionary<Type, Dictionary<string, string>> _columnNamesCache = new Dictionary<Type, Dictionary<string, string>>();

        internal static string GetIdFieldName(Type modelType)
        {
            if (_idFieldsCache.ContainsKey(modelType))
                return _idFieldsCache[modelType];
            var attr = modelType.GetCustomAttribute<IdFieldNameAttribute>();
            string res;
            if (attr == null)
                res = "Id";
            else
                res = attr.Name;
            _idFieldsCache[modelType] = res;
            return res;
        }

        internal static string GetIdFieldName<TModel>() where TModel : BaseModel
        {
            return GetIdFieldName(typeof(TModel));
        }

        internal static string GetModelTableName(Type modelType)
        {
            if (_tableNamesCache.ContainsKey(modelType))
                return _tableNamesCache[modelType];
            var attr = modelType.GetCustomAttribute<TableAttribute>();
            string res;
            if (attr == null)
                res = modelType.Name;
            else
                res = attr.Name;
            _tableNamesCache[modelType] = res;
            return res;
        }

        internal static string GetModelTableName<TModel>() where TModel : BaseModel
        {
            return GetModelTableName(typeof(TModel));
        }

        internal static List<MappingInfo> GetMappingInfo<TModel>() where TModel : BaseModel
        {
            return GetMappingInfo(typeof(TModel));
        }

        internal static List<MappingInfo> GetMappingInfo(Type baseModelType)
        {
            if (_mappingCache.ContainsKey(baseModelType))
                return _mappingCache[baseModelType];

            var res = new List<MappingInfo>();
            var props = baseModelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var listType = typeof(IEnumerable<BaseModel>);
            var modelType = typeof(BaseModel);
            foreach (var prop in props)
            {
                if (listType.IsAssignableFrom(prop.PropertyType))
                    continue;
                if (modelType.IsAssignableFrom(prop.PropertyType))
                    res.Add(new MappingInfo(prop, GetFKFieldName(prop), MappingType.ForeignKey));
                else
                    res.Add(new MappingInfo(prop, GetDirectFieldName(prop), MappingType.Direct));
            }

            _mappingCache[baseModelType] = res;
            return res;
        }

        internal static string GetColumnName<TModel>(string modelFieldName) where TModel : BaseModel
        {
            return GetColumnName(typeof(TModel), modelFieldName);
        }

        internal static string GetColumnName(Type modelType, string modelFieldName)
        {
            if (_columnNamesCache.ContainsKey(modelType) && _columnNamesCache[modelType].ContainsKey(modelFieldName))
                return _columnNamesCache[modelType][modelFieldName];

            string res = null;

            var prop = modelType.GetProperty(modelFieldName);
            if (prop == null)
                return res;

            var listType = typeof(IEnumerable<BaseModel>);
            var otherModelType = typeof(BaseModel);

            if (listType.IsAssignableFrom(prop.PropertyType))
                return res;
            if (modelType.IsAssignableFrom(prop.PropertyType))
                res = GetFKFieldName(prop);
            else
                res = GetDirectFieldName(prop);

            if (!_columnNamesCache.ContainsKey(modelType))
                _columnNamesCache[modelType] = new Dictionary<string, string>();

            _columnNamesCache[modelType][modelFieldName] = res;
            return res;
        }

        internal static string GetFKFieldName(PropertyInfo prop)
        {
            var fkAttribute = prop.GetCustomAttribute<ForeignKeyAttribute>();
            if (fkAttribute == null)
                return $"{prop.PropertyType.Name}Id";
            return fkAttribute.Name;
        }

        internal static string GetDirectFieldName(PropertyInfo prop)
        {
            var fnAttribute = prop.GetCustomAttribute<ColumnAttribute>();
            if (fnAttribute == null)
                return prop.Name;
            return fnAttribute.Name;
        }
    }
}
