using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentJsonNet
{
    public abstract class JsonMapBase
    {
        public List<Action<MemberInfo, JsonProperty, MemberSerialization>> Actions { get; } =
            new List<Action<MemberInfo, JsonProperty, MemberSerialization>>();

        public abstract Type SerializedType { get; }

        /// <summary>
        /// Gets the a value indicating whether the type can be processed by this class.
        /// </summary>
        public virtual bool AcceptsType(Type objectType)
        {
            return objectType == this.SerializedType;
        }

        public virtual bool AcceptsMember(MemberInfo member, MemberInfo memberInfo)
        {
            return member.MetadataToken == memberInfo.MetadataToken
                   && member.DeclaringType == memberInfo.DeclaringType;
        }
    }
}