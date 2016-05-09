using System;
using System.Linq;
using System.Reflection;

namespace FluentJsonNet
{
    public class JsonMap<T> : JsonMap
    {
        public override Type SerializedType { get; } = typeof(T);

        public class AndSubtypes : JsonMap<T>,
            IAndSubtypes<T>
        {
            public override Type SerializedType { get; } = typeof(T);

            public override bool AcceptsType(Type objectType)
            {
                return typeof(T).IsAssignableFrom(objectType);
            }

            public override bool AcceptsMember(MemberInfo member, MemberInfo baseMember)
            {
                if (base.AcceptsMember(member, baseMember))
                    return true;

                if (baseMember.DeclaringType?.IsInterface == true)
                {
                    var map = member.DeclaringType?.GetInterfaceMap(baseMember.DeclaringType);
                    if (map.HasValue)
                        return map.Value.TargetMethods.Contains(member);
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    var prop = (PropertyInfo)member;
                    var baseProp = baseMember as PropertyInfo;
                    if (baseProp != null)
                    {
                        var method = prop.GetMethod ?? prop.SetMethod;
                        var baseMethod = baseProp.GetMethod ?? baseProp.SetMethod;
                        while (method != null)
                        {
                            method = method.GetBaseDefinition();
                            if (base.AcceptsMember(method, baseMethod))
                                return true;
                        }
                    }
                }

                return false;
            }

            internal Func<Type, string> DiscriminatorFieldValueGetter { get; set; }

            Func<Type, string> IAndSubtypes.DiscriminatorFieldValueGetter => this.DiscriminatorFieldValueGetter;

            /// <summary>
            /// Gets the discriminator value given the type being serialized. 
            /// Note: you must implement `CreateObject` for the deserialization.
            /// </summary>
            /// <remarks>
            /// You must implement `CreateObject` method when using discriminators in this class.
            /// </remarks>
            /// <param name="discriminatorValueGetter"></param>
            protected void DiscriminatorValue(Func<Type, string> discriminatorValueGetter)
            {
                this.DiscriminatorFieldValueGetter = discriminatorValueGetter;
            }

            protected virtual T CreateObject(string discriminatorValue)
            {
                if (this.DiscriminatorFieldValueGetter != null)
                    throw new Exception($"You must implement `CreateObject` method when using custom discriminator values in the class `{this.GetType().Name}`.");
                var type = Type.GetType(discriminatorValue);
                if (type != null)
                    return (T)Activator.CreateInstance(type);
                throw new Exception($"Could not find the type to deserialize. `{discriminatorValue}`");
            }

            object IAndSubtypes.CreateObject(string discriminatorValue)
            {
                return this.CreateObject(discriminatorValue);
            }

            T IAndSubtypes<T>.CreateObject(string discriminatorValue)
            {
                return this.CreateObject(discriminatorValue);
            }
        }
    }

    public abstract class JsonMap : JsonMapBase
    {
        public override object CreateNew()
        {
            return Activator.CreateInstance(this.SerializedType);
        }

        public override bool CanCreateNew()
        {
            return !this.SerializedType.IsAbstract
                   && !this.SerializedType.IsInterface
                   && (this.SerializedType.IsValueType || this.SerializedType.GetConstructor(new Type[0]) != null);
        }
    }
}