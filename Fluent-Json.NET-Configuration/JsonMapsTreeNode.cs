using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentJsonNet
{
    internal class JsonMapsTreeNode
    {
        public JsonMapsTreeNode(JsonMapBase mapper, Type parent, Type[] children)
        {
            this.Mapper = mapper;
            this.Parent = parent;
            this.Children = new ReadOnlyCollection<Type>(children);
        }

        public JsonMapBase Mapper { get; }
        public Type Parent { get; }
        public IReadOnlyCollection<Type> Children { get; }
    }
}