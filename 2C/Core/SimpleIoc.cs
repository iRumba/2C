using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core
{
    public class SimpleIoc
    {
        static SimpleIoc _instance;
        Dictionary<Type, Func<Type, object>> _objects = new Dictionary<Type, Func<Type, object>>();
        Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        private SimpleIoc()
        {
            
        }

        public static SimpleIoc Instance
        {
            get
            {
                return _instance ?? (_instance = new SimpleIoc());
            }
        }

        object GetFromCtor(ConstructorInfo ctor)
        {
            if (ctor == null)
                return null;

            object res = null;
            var parameters = ctor.GetParameters();
            if (parameters.Length == 0)
                res = Activator.CreateInstance(ctor.ReflectedType);
            else
            {
                var paramValues = new List<object>();
                foreach (var parameter in parameters)
                {
                    var value = Get(parameter.ParameterType);
                    if (value != null)
                        paramValues.Add(value);
                }
                if (paramValues.Count == parameters.Length)
                    res = Activator.CreateInstance(ctor.ReflectedType, paramValues.ToArray());
            }

            return res;
        }

        object Get(Type type)
        {
            if (!_objects.ContainsKey(type) || !_mappings.ContainsKey(type))
                throw new InvalidOperationException($"Тип {type.Name} не найден в контейнере");
            var res = _objects[type]?.Invoke(_mappings[type]);
            if (res == null)
                throw new InvalidOperationException($"Тип {type.Name} не может быть создан");
            return res;
        }

        object GetNew(Type type)
        {
            var res = GetFromCtor(type.GetConstructor(Type.EmptyTypes));
            if (res != null)
                return res;
            var ctors = type.GetConstructors();
            foreach (var ctor in ctors)
            {
                res = GetFromCtor(ctor);
                if (res != null)
                    break;
            }

            return res;
        }

        object GetSingleton(Type type)
        {
            if (!_singletons.ContainsKey(type))
                _singletons[type] = GetNew(type);
            return _singletons[type];
        }

        void Add<TDefinition, TImplementation>(Func<Type, object> func) where TImplementation : TDefinition
        {
            var typeDef = typeof(TDefinition);
            var typeImpl = typeof(TImplementation);

            if (typeImpl.IsInterface || typeImpl.IsAbstract)
                throw new InvalidOperationException("TImplementation должен не быть интерфейсом или абстрактным классом");

            _objects[typeDef] = func;
            _mappings[typeDef] = typeImpl;
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public void AddTransient<T>()
        {
            AddTransient<T, T>();
        }

        public void AddTransient<TDefinition, TImplementation>() where TImplementation : TDefinition
        {
            Add<TDefinition, TImplementation>(GetNew);
        }

        public void AddSingleton<T>()
        {
            AddSingleton<T, T>();
        }

        public void AddSingleton<TDefinition, TImplementation>() where TImplementation : TDefinition
        {
            Add<TDefinition, TImplementation>(GetSingleton);
        }

        public void AddSingleton<T>(T instance)
        {
            AddSingleton<T, T>(instance);
        }

        public void AddSingleton<TDefinition, TImplementation>(TImplementation instance) where TImplementation : TDefinition
        {
            
            if (!_singletons.ContainsKey(typeof(TDefinition)))
            {
                Add<TDefinition, TImplementation>(GetSingleton);
                _singletons[typeof(TDefinition)] = instance;
            }  
        }

        public bool Remove<T>()
        {
            var type = typeof(T);
            return _objects.Remove(type) | _mappings.Remove(type) | _singletons.Remove(type);
        }
    }
}
