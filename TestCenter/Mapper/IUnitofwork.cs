using System;
using System.Linq.Expressions;

namespace ConsoleApplication1.Mapper
{
    public interface IUnitofwork
    {
        void RegisterAdd<TModel>(TModel model) where TModel : class, new();

        void RegisterModify<TModel>(TModel model, Expression<Func<TModel, Boolean>> where) where TModel : class, new();

        void Commit();
    }
}
