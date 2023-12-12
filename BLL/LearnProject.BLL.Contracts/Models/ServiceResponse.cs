namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель для методов сервиса, возвращающих объекты
    /// </summary>
    /// <typeparam name="T">возвращаемый объект</typeparam>
    public sealed class ServiceResponse<T>
    {
        private ServiceResponse(bool result) : this(result, string.Empty, default) { }

        private ServiceResponse(bool result, string error) : this(result, error, default) { }

        private ServiceResponse(bool result, T value) : this(result, string.Empty, value) { }

        private ServiceResponse(bool result, string error, T? value)
        {
            IsSuccessful = result;
            Error = error;
            Value = value;
        }

        /// <summary>
        /// успешность операции
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// ошибка
        /// </summary>
        public string Error { get; private set; } = string.Empty;

        /// <summary>
        /// объект отданный сервисом
        /// </summary>
        public T? Value { get; private set; }

        /// <summary>
        /// создать модель с успешным результатом
        /// </summary>
        /// <returns>созданная модель</returns>
        public static ServiceResponse<T> CreateSuccessfulResponse(T value)
        {
            return new ServiceResponse<T>(true, value);
        }

        /// <summary>
        /// создать модель с успешным результатом
        /// </summary>
        /// <returns>созданная модель</returns>
        public static ServiceResponse<T> CreateSuccessfulResponse()
        {
            return new ServiceResponse<T>(true);
        }

        /// <summary>
        /// создать модель с ошибкой
        /// </summary>
        /// <returns>созданная модель</returns>
        public static ServiceResponse<T> CreateFailedResponse(string error)
        {
            return new ServiceResponse<T>(false, error);
        }

        /// <summary>
        /// создать модель с ошибкой
        /// </summary>
        /// <returns>созданная модель</returns>
        public static ServiceResponse<T> CreateFailedResponse(string error, T value)
        {
            return new ServiceResponse<T>(false, error, value);
        }
    }
}
