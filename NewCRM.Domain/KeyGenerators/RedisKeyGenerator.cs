using System;

namespace NewCRM.Domain.KeyGenerators
{
    public abstract class RedisKeyGenerator : IKeyGenerator
    {
        public virtual String KeyGenerator() => throw new NotImplementedException();
    }
}