using LearnProject.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LearnProject.Data.DAL.Repositories
{
    /// <summary>
    /// абстрактный класс сохраняющий изменения после операций с контекстом
    /// </summary>
    /// <typeparam name="T">модель сущности</typeparam>
    /// <typeparam name="TKey">ключ модели сущности</typeparam>
    public abstract class AbstractRepository<T, TKey> : IRepository<T, TKey> where T : class
    {
        /// <summary>
        /// абстрактное свойство для получения контекста
        /// </summary>
        protected abstract RepositoryAppDbContext Context
        { 
            get; 
            set; 
        }

        /// <summary>
        /// абстрактный метод реализации запроса на получение сущностей
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс количество</param>
        virtual public IQueryable<T> ReadAll() => Context.Set<T>().AsNoTracking();


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => ReadAll().Where(expression);

        public abstract Task<T?> FindByKeyAsync(TKey key);

        /// <summary>
        /// абстрактный метод реализации запроса на создание сущности
        /// </summary>
        /// <param name="value">сущность</param>
        public void Create(T value) => Context.Add(value);

        /// <summary>
        /// абстрактный метод реализации запроса на обновление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        public void Update(T value) => Context.Update(value);

        /// <summary>
        /// абстрактный метод реализации запроса на удаление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        public void Delete(T value) => Context.Remove(value);
    }
}
