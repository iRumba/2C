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

        public static string GetIdFieldName(Type modelType)
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

        public static string GetIdFieldName(this BaseModel model)
        {
            return GetIdFieldName(model.GetType());
        }

        public static string GetIdFieldName<TModel>() where TModel : BaseModel
        {
            return GetIdFieldName(typeof(TModel));
        }

        public static string GetModelTableName(Type modelType, bool flag)
        {
            var attr = modelType.GetCustomAttribute<TableAttribute>();
            string res;
            if (attr == null)
                res = modelType.Name;
            else
                res = attr.Name;

            if (flag)
                res = res.Replace("[", "").Replace("]", "");
            _tableNamesCache[modelType] = res;
            return res;
        }

        public static string GetModelTableName<TModel>(bool flag = false) where TModel : BaseModel
        {
            return GetModelTableName(typeof(TModel), flag);
        }

        public static string GetModelTableName(this BaseModel model, bool flag = false)
        {
            return GetModelTableName(model.GetType(), flag);
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

        internal static List<MappingInfo> GetMappingInfo(this BaseModel model)
        {
            return GetMappingInfo(model.GetType());
        }

        public static string GetColumnName<TModel>(string modelFieldName) where TModel : BaseModel
        {
            return GetColumnName(typeof(TModel), modelFieldName);
        }

        public static string GetColumnName(Type modelType, string modelFieldName)
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
            if (otherModelType.IsAssignableFrom(prop.PropertyType))
                res = GetFKFieldName(prop);
            else
                res = GetDirectFieldName(prop);

            if (!_columnNamesCache.ContainsKey(modelType))
                _columnNamesCache[modelType] = new Dictionary<string, string>();

            _columnNamesCache[modelType][modelFieldName] = res;
            return res;
        }

        public static string GetColumnName(this BaseModel model, string modelFieldName)
        {
            return GetColumnName(model.GetType(), modelFieldName);
        }

        public static string GetFKFieldName(PropertyInfo prop)
        {
            var fkAttribute = prop.GetCustomAttribute<ForeignKeyAttribute>();
            if (fkAttribute == null)
                return $"{prop.PropertyType.Name}Id";
            return fkAttribute.Name;
        }

        public static string GetFKFieldName<TModel>(string propertyName)
        {
            var type = typeof(TModel);
            var prop = type.GetProperty(propertyName);
            return GetFKFieldName(prop);
        }

        public static string GetDirectFieldName(PropertyInfo prop)
        {
            var fnAttribute = prop.GetCustomAttribute<ColumnAttribute>();
            if (fnAttribute == null)
                return prop.Name;
            return fnAttribute.Name;
        }
    }
}
