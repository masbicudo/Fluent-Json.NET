using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentJsonNet
{
    public static class JsonMapExtensions
    {
        private static MemberInfo GetMemberInfoFromExpression(LambdaExpression expr)
        {
            var memberExpression = expr.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null)
                    if (unary.NodeType == ExpressionType.Convert || unary.NodeType == ExpressionType.ConvertChecked)
                        memberExpression = unary.Operand as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Expression must be a property or field expression of the main type.", nameof(expr));

            var memberInfo = memberExpression.Member;
            if (!(memberInfo is PropertyInfo || memberInfo is FieldInfo))
                throw new ArgumentException("Expression must be a property or field expression of the main type.", nameof(expr));

            return memberInfo;
        }

        public static void Ignore<T, TProp>(this JsonMap<T> jsonMap, Expression<Func<T, TProp>> expr)
        {
            jsonMap.Ignore((LambdaExpression)expr);
        }

        public static void Ignore<T, TProp>(this JsonSubclassMap<T> jsonMap, Expression<Func<T, TProp>> expr)
        {
            jsonMap.Ignore((LambdaExpression)expr);
        }

        public static void Ignore(this JsonMapBase jsonMap, LambdaExpression expr)
        {
            var memberInfo = GetMemberInfoFromExpression(expr);

            jsonMap.Actions.Add(
                (member, property, mode) =>
                {
                    if (jsonMap.AcceptsMember(member, memberInfo))
                    {
                        property.ShouldSerialize = instance => false;
                    }
                });
        }

        public static void IgnoreAllByDefault(this JsonMap jsonMap)
        {
            jsonMap.Actions.Insert(
                0,
                (member, property, mode) =>
                {
                    property.ShouldSerialize = instance => false;
                });
        }

        public static void Map<T, TProp>(this JsonMap<T> jsonMap, Expression<Func<T, TProp>> expr, string name)
        {
            jsonMap.Map((LambdaExpression)expr, name);
        }

        public static void SubclassMap<T>(this IAndSubtypes jsonMap, Expression<Func<T, object>> expr, string name)
        {
            ((JsonMapBase)jsonMap).Map((LambdaExpression)expr, name);
        }

        public static void Map<T, TProp>(this JsonSubclassMap<T> jsonMap, Expression<Func<T, TProp>> expr, string name)
        {
            jsonMap.Map((LambdaExpression)expr, name);
        }

        public static void Map(this JsonMapBase jsonMap, LambdaExpression expr, string name)
        {
            var memberInfo = GetMemberInfoFromExpression(expr);

            jsonMap.Actions.Add(
                (member, property, mode) =>
                {
                    if (member.MetadataToken == memberInfo.MetadataToken
                        && member.DeclaringType == memberInfo.DeclaringType)
                    {
                        property.PropertyName = name;
                        property.ShouldSerialize = instance => true;
                    }
                });
        }
    }
}