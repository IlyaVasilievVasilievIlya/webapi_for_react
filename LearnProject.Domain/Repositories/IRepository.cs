namespace LearnProject.Domain.Repositories
{
    /// <summary>
    /// контракт репозитория
    /// </summary>
    /// <typeparam name="T">тип сущности</typeparam>
    /// <typeparam name="TKey">PK сущности</typeparam>
    public interface IRepository<T, TKey>
    {
        /// <summary>
        /// получить сущности
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. количество</param>
        /// <returns>коллекция сущностей</returns>
        Task<IEnumerable<T>> ReadAllAsync(int offset = 0, int limit = 1000);

        /// <summary>
        /// получить сущность по id
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>сущность либо null</returns>
        Task<T?> ReadAsync(TKey key);

        /// <summary>
        /// создать сущность
        /// </summary>
        /// <param name="value">сущность</param>
        Task<T> CreateAsync(T value);

        /// <summary>
        /// изменить сущность
        /// </summary>
        /// <param name="value">сущность</param>
        Task UpdateAsync(T value);

        /// <summary>
        /// удаление сущности
        /// </summary>
        /// <param name="value">сущность</param>
        Task DeleteAsync(T value);

        /// <summary>
        /// выполнить кастомный запрос
        /// </summary>
        /// <typeparam name="TResult">тип результата запроса</typeparam>
        /// <param name="body">функция для запроса</param>
        /// <returns>результат запроса</returns>
        Task<TResult> Query<TResult>(Func<IQueryable<T>, Task<TResult>> body);
    }
}
