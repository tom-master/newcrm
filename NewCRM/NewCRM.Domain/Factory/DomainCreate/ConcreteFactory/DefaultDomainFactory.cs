using System;
using System.ComponentModel.Composition;

namespace NewCRM.Domain.Factory.DomainCreate.ConcreteFactory
{

    [Export(typeof(DomainFactory))]
    public sealed class DefaultDomainFactory : DomainFactory
    {
        /// <summary>
        /// 创建并返回一个领域对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T Create<T>() => Activator.CreateInstance<T>();

    }
}
