﻿using LearnProject.BLL.Contracts.Models;

namespace LearnProject.BLL.Contracts
{
    /// <summary>
    /// интерфейс работы с сервисом аутентификации
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// регистрация пользователя
        /// </summary>
        /// <param name="model">модель регистрации</param>
        /// <returns>результат операции</returns>
        Task<AuthenticationResponse> RegisterAsync(RegisterUserModel model);

        /// <summary>
        /// вход пользователя
        /// </summary>
        /// <param name="model">модель входа</param>
        /// <returns>результат операции</returns>
        Task<AuthenticationResponse> LogInAsync(LoginUserModel model);

        /// <summary>
        /// вход через Google
        /// </summary>
        Task<AuthenticationResponse> LogInWithGoogleAsync(string token);


        /// <summary>
        /// разлогин (удаление refresh token)
        /// </summary>
        /// <param name="token"></param>
        Task<AuthenticationResponse> LogOut(string token); 

        /// <summary>
        /// обновление refresh токена пользователя
        /// </summary>
        /// <param name="token">модель обновления</param>
        /// <returns>результат операции</returns>
        Task<AuthenticationResponse> RefreshTokenAsync(string token);

        /// <summary>
        /// проверить существование refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        Task<AuthenticationResponse> CheckRefreshTokenExists(string refreshToken);
    }
}
