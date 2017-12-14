using System;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentJsonNet.Tests
{
    public static class MyAssert
    {
        public static void Throws<TEx>([NotNull] Action action)
            where TEx : Exception
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            TEx exception = null;
            try
            {
                action();
            }
            catch (TEx ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(TEx));
        }

        public static void Throws<TEx>([NotNull] Func<object> action)
            where TEx : Exception
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            TEx exception = null;
            try
            {
                action();
            }
            catch (TEx ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(TEx));
        }

        public static void Throws([NotNull] Action action)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            Exception exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(Exception));
        }

        public static void Throws([NotNull] Func<object> action)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            Exception exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(Exception));
        }

        public static void Throws<TEx>([NotNull] string message, [NotNull] Action action)
            where TEx : Exception
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (message == null)
                Assert.Inconclusive("Argument null: {0}", nameof(message));
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            TEx exception = null;
            try
            {
                action();
            }
            catch (TEx ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(TEx));
            Assert.AreEqual(message, exception?.Message);
        }

        public static void Throws<TEx>([NotNull] string message, [NotNull] Func<object> action)
            where TEx : Exception
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (message == null)
                Assert.Inconclusive("Argument null: {0}", nameof(message));
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            TEx exception = null;
            try
            {
                action();
            }
            catch (TEx ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(TEx));
            Assert.AreEqual(message, exception?.Message);
        }

        public static void Throws([NotNull] string message, [NotNull] Action action)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (message == null)
                Assert.Inconclusive("Argument null: {0}", nameof(message));
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            Exception exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(Exception));
            Assert.AreEqual(message, exception?.Message);
        }

        public static void Throws([NotNull] string message, [NotNull] Func<object> action)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (message == null)
                Assert.Inconclusive("Argument null: {0}", nameof(message));
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (action == null)
                Assert.Inconclusive("Argument null: {0}", nameof(action));

            Exception exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            Assert.IsInstanceOfType(exception, typeof(Exception));
            Assert.AreEqual(message, exception?.Message);
        }
    }
}