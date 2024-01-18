using LearnProject.Domain.Repositories;

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
        /// выполнение операции с последующим сохранением
        /// </summary>
        /// <param name="body">операция</param>
        protected async Task OperationEnvironmentAsync(Action body)
        {
            body.Invoke();
            await Context.SaveChangesAsync();
        }

        /// <summary>
        /// выполнение операции, возвращающей результат, с последующим сохранением
        /// </summary>
        /// <typeparam name="TRet"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        protected async Task<TRet> OperationEnvironmentAsync<TRet>(Func<Task<TRet>> body)
        {
            return await body.Invoke();
        }

        protected async Task<TRet> OperationEnvironmentAsync<TRet>(Func<TRet> body)
        {
            var result = body.Invoke();
            await Context.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// абстрактный метод реализации запроса на получение сущностей
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс количество</param>
        protected abstract Task<IEnumerable<T>> ReadAllImplementationAsync(int offset, int limit);

        /// <summary>
        /// абстрактный метод реализации запроса на получение сущности по id
        /// </summary>
        /// <param name="key">id сущности</param>
        protected abstract Task<T?> ReadImplementationAsync(TKey key);

        /// <summary>
        /// абстрактный метод реализации запроса на создание сущности
        /// </summary>
        /// <param name="value">сущность</param>
        protected abstract T CreateImplementation(T value);

        /// <summary>
        /// абстрактный метод реализации запроса на обновление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        protected abstract void UpdateImplementation(T value);

        /// <summary>
        /// абстрактный метод реализации запроса на удаление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        protected abstract void DeleteImplementation(T value);

        /// <summary>
        /// абстрактный метод реализации запроса по умолчанию
        /// </summary>
        /// <returns>дефолтный запрос</returns>
        protected abstract IQueryable<T> QueryImplementation();

        /// <summary>
        /// реализация IRepository для получения сущностей
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">лимит</param>
        /// <returns>коллекция сущностей</returns>
        public async Task<IEnumerable<T>> ReadAllAsync(int offset = 0, int limit = 1000)
        {
            return await OperationEnvironmentAsync(async () => await ReadAllImplementationAsync(offset, limit));
        }

        /// <summary>
        /// реализация IRepository для получения сущности по id
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>сущность или null</returns>
        public async Task<T?> ReadAsync(TKey key)
        {
            return await OperationEnvironmentAsync(async () => await ReadImplementationAsync(key));
        }

        /// <summary>
        /// реализация IRepository для создания сущности
        /// </summary>
        /// <param name="value"></param>
        public async Task<T> CreateAsync(T value)
        {
            return await OperationEnvironmentAsync(() => CreateImplementation(value));
        }

        /// <summary>
        /// реализация IRepository для изменения сущности
        /// </summary>
        /// <param name="value">сущность</param>
        public async Task UpdateAsync(T value)
        {
            await OperationEnvironmentAsync(() => UpdateImplementation(value));
        }

        /// <summary>
        /// реализация IRepository для удаления сущности
        /// </summary>
        /// <param name="value">сущность</param>
        public async Task DeleteAsync(T value)
        {
            await OperationEnvironmentAsync(() => DeleteImplementation(value));
        }

        /// <summary>
        /// реализация IRepository: выполнение кастомного запроса
        /// </summary>
        /// <typeparam name="TResult">тип результата запроса</typeparam>
        /// <param name="body">уточняющая функция</param>
        /// <returns>результат запроса</returns>
        public async Task<TResult> Query<TResult>(Func<IQueryable<T>, Task<TResult>> body)
        {
            return await OperationEnvironmentAsync(() =>
            {
                var query = QueryImplementation();
                return body(query);
            });
        }
    }
}
