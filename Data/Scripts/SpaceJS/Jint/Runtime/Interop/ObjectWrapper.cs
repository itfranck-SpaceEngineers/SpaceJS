using System;
using System.Collections.Generic;
using System.Reflection;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using Jint.Runtime.Descriptors.Specialized;

namespace Jint.Runtime.Interop
{
	/// <summary>
	/// Wraps a CLR instance
	/// </summary>
	public sealed class ObjectWrapper : ObjectInstance, IObjectWrapper
    {
        public ObjectWrapper(Engine engine, object obj)
            : base(engine)
        {
            Target = obj;
        }

        public object Target { get; }

        public override void Put(string propertyName, JsValue value, bool throwOnError)
        {
            if (!CanPut(propertyName))
            {
                if (throwOnError)
                {
                    ExceptionHelper.ThrowTypeError(Engine);
                }

                return;
            }

            var ownDesc = GetOwnProperty(propertyName);

            if (ownDesc == null)
            {
                if (throwOnError)
                {
                    ExceptionHelper.ThrowTypeError(_engine, "Unknown member: " + propertyName);
                }
            }
            else
            {
                ownDesc.Value = value;
            }
        }

        public override PropertyDescriptor GetOwnProperty(string propertyName)
        {
            PropertyDescriptor x;
            if (TryGetProperty(propertyName, out x))
            {
                return x;
            }

            var type = Target.GetType();
            var key = new Engine.ClrPropertyDescriptorFactoriesKey(type, propertyName);

            System.Func<Jint.Engine, object, Jint.Runtime.Descriptors.PropertyDescriptor> factory;
            if (!_engine.ClrPropertyDescriptorFactories.TryGetValue(key, out factory))
            {
                factory = ResolveProperty(type, propertyName);
                _engine.ClrPropertyDescriptorFactories[key] = factory;
            }

            var descriptor = factory(_engine, Target);
            AddProperty(propertyName, descriptor);
            return descriptor;
        }

        private static Func<Engine, object, PropertyDescriptor> ResolveProperty(Type type, string propertyName)
        {
            // TODO: SpaceEngineers doesn't allow much of this stuff.
/*
            // look for a property
            PropertyInfo property = null;
            foreach (var p in type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public))
            {
                if (EqualsIgnoreCasing(p.Name, propertyName))
                {
                    property = p;
                    break;
                }
            }

            if (property != null)
            {
                return (engine, target) => new PropertyInfoDescriptor(engine, property, target);
            }

            // look for a field
            FieldInfo field = null;
            foreach (var f in type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public))
            {
                if (EqualsIgnoreCasing(f.Name, propertyName))
                {
                    field = f;
                    break;
                }
            }

            if (field != null)
            {
                return (engine, target) => new FieldInfoDescriptor(engine, field, target);
            }

            // if no properties were found then look for a method
            List<MethodInfo> methods = null;
            foreach (var m in type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public))
            {
                if (EqualsIgnoreCasing(m.Name, propertyName))
                {
                    methods = methods ?? new List<MethodInfo>();
                    methods.Add(m);
                }
            }

            if (methods?.Count > 0)
            {
                return (engine, target) => new PropertyDescriptor(new MethodInfoFunctionInstance(engine, methods.ToArray()), PropertyFlag.OnlyEnumerable);
            }

            // if no methods are found check if target implemented indexing
            PropertyInfo first = null;
            foreach (var p in type.GetProperties())
            {
                if (p.GetIndexParameters().Length != 0)
                {
                    first = p;
                    break;
                }
            }

            if (first != null)
            {
                return (engine, target) => new IndexDescriptor(engine, propertyName, target);
            }

            // try to find a single explicit property implementation
            List<PropertyInfo> list = null;
            foreach (Type iface in type.GetInterfaces())
            {
                foreach (var iprop in iface.GetProperties())
                {
                    if (EqualsIgnoreCasing(iprop.Name, propertyName))
                    {
                        list = list ?? new List<PropertyInfo>();
                        list.Add(iprop);
                    }
                }
            }

            if (list?.Count == 1)
            {
                return (engine, target) => new PropertyInfoDescriptor(engine, list[0], target);
            }

            // try to find explicit method implementations
            List<MethodInfo> explicitMethods = null;
            foreach (Type iface in type.GetInterfaces())
            {
                foreach (var imethod in iface.GetMethods())
                {
                    if (EqualsIgnoreCasing(imethod.Name, propertyName))
                    {
                        explicitMethods = explicitMethods ?? new List<MethodInfo>();
                        explicitMethods.Add(imethod);
                    }
                }
            }

            if (explicitMethods?.Count > 0)
            {
                return (engine, target) => new PropertyDescriptor(new MethodInfoFunctionInstance(engine, explicitMethods.ToArray()), PropertyFlag.OnlyEnumerable);
            }

            // try to find explicit indexer implementations
            List<PropertyInfo> explicitIndexers = null;
            foreach (Type iface in type.GetInterfaces())
            {
                foreach (var iprop in iface.GetProperties())
                {
                    if (iprop.GetIndexParameters().Length != 0)
                    {
                        explicitIndexers = explicitIndexers ?? new List<PropertyInfo>();
                        explicitIndexers.Add(iprop);
                    }
                }
            }

            if (explicitIndexers?.Count == 1)
            {
                return (engine, target) => new IndexDescriptor(engine, explicitIndexers[0].DeclaringType, propertyName, target);
            }
*/
            return (engine, target) => PropertyDescriptor.Undefined;
        }

        private static bool EqualsIgnoreCasing(string s1, string s2)
        {
            bool equals = false;
            if (s1.Length == s2.Length)
            {
                if (s1.Length > 0)
                {
                    equals = char.ToLowerInvariant(s1[0]) == char.ToLowerInvariant(s2[0]);
                }
                if (equals && s1.Length > 1)
                {
                    equals = s1.Substring(1) == s2.Substring(1);
                }
            }
            return equals;
        }
    }
}
