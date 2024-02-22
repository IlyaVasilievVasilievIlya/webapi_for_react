using LearnProject.Shared.Common.Settings;

namespace Cars.Api.Configuration
{
    /// <summary>
    /// класс добавления строго типизированных настроек
    /// </summary>
    public static class SettingsConfigurator
    {
        public static T AddAppSettings<T>(this IServiceCollection services, IConfiguration config) where T : class, new()
        {
            var settings = new T();
            var section = config.GetSection(settings.GetType().Name);
            section.Bind(settings);
            services.AddSingleton(settings);

            return settings;
        }
    }
}
