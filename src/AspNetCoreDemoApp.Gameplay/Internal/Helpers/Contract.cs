using System;

namespace AspNetCoreDemoApp.Gameplay
{
    internal class Contract
    {
        public static void Requires(Func<bool> condition, string message)
        {
            if (!condition())
            {
                throw new Exception(message);
            }
        }

        public static PostConditions Ensure => new PostConditions();
    }

    internal class PostConditions
    {
        public T NotNull<T>(T obj, string message)
            where T : class
        {
            Contract.Requires(() => obj != null, message);

            return obj;
        }
    }
}