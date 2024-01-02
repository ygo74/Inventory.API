using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Plugins
{
    public class PluginResolver : IDisposable, IAsyncDisposable
    {
        private readonly ILogger<PluginResolver> _logger;
        private ServiceProvider _serviceProvider;
        private readonly ServiceCollection _serviceCollection;

        public PluginResolver(ILogger<PluginResolver> logger)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _serviceCollection = new ServiceCollection();
        }

        #region IDisposable, IAsyncDisposable
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceProvider?.Dispose();
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_serviceProvider is not null)
            {
                await _serviceProvider.DisposeAsync().ConfigureAwait(false);
            }
        }


        #endregion


        private ServiceProvider ServiceProvider 
        { 
            get 
            { 
                if (_serviceProvider == null)
                {
                    _serviceProvider = _serviceCollection.BuildServiceProvider();
                }
                return _serviceProvider; 
            } 
        }

        public T GetService<T>() where T : class
        {
            return this.ServiceProvider.GetService<T>();
        }

        public Assembly LoadPlugin(string path)
        {
            //Log.Information("Load assembly from file {0}", path);
            PluginLoadContext loadContext = new PluginLoadContext(path);

            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        public IEnumerable<T> CreateCommands<T>(Assembly assembly)
        {

            List<T> commands = new List<T>();
            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(T).IsAssignableFrom(type))
                    {
                        T result = (T)Activator.CreateInstance(type);
                        //yield return result;
                        commands.Add(result);
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
            }
            return commands;
        }

        public void RegisterIntegrationsFromAssembly<T>(IConfiguration configuration, Assembly assembly)
        {
            RegisterIntegrationsFromAssembly<T>(_serviceCollection, configuration, assembly);
        }

        /// <summary>
        /// Based on https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#create-the-plugin-interfaces
        /// with alterations to support injecting settings
        /// https://github.com/haapanen/NetCorePluginDI/blob/master/NetCorePluginDI/Program.cs
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="assembly"></param>
        private void RegisterIntegrationsFromAssembly<T>(IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            // Search Startup class to configure services before added the Integration classes
            var startupType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == "Startup");

            if (startupType != null)
            {
                var configureServicesMethod = startupType.GetMethod("ConfigureServices");

                if (configureServicesMethod != null)
                {
                    // Créer une instance de la classe de configuration du plugin
                    var instance = Activator.CreateInstance(startupType);

                    // Appeler la méthode de configuration du service du plugin
                    configureServicesMethod.Invoke(instance, new object[] { services });
                }
                else
                {
                    Console.WriteLine("Méthode de configuration des services introuvable dans le plugin.");
                }
            }
            else
            {
                Console.WriteLine("Classe de configuration des services introuvable dans le plugin.");
            }

            var pluginTypes = assembly.GetTypes().Where(e => typeof(T).IsAssignableFrom(e));
            foreach (var type in pluginTypes)
            {
                // Register all classes that implement the IIntegration interface
                if (typeof(T).IsAssignableFrom(type))
                {
                    // Add as a singleton as the Worker is a singleton and we'll only have one
                    // instance. If this would be a Controller or something else with clearly defined
                    // scope that is not the lifetime of the application, use AddScoped.
                    //services.AddSingleton(typeof(T), type);
                    services.AddScoped(typeof(T), type);
                }

                //if (typeof(IPluginFactory).IsAssignableFrom(type))
                //{
                //    var plugin = Activator.CreateInstance(type) as IPluginFactory;
                //    plugin?.Configure(services);
                //}

                //// Register all classes that implement the ISettings interface
                //if (typeof(ISettings).IsAssignableFrom(type))
                //{
                //    var settings = Activator.CreateInstance(type);
                //    // appsettings.json or some other configuration provider should contain
                //    // a key with the same name as the type
                //    // e.g. "HttpIntegrationSettings": { ... }
                //    if (!configuration.GetSection(type.Name).Exists())
                //    {
                //        // If it does not contain the key, throw an error
                //        throw new ArgumentException($"Configuration does not contain key [{type.Name}]");
                //    }
                //    configuration.Bind(type.Name, settings);

                //    // Settings can be singleton as we'll only ever read it
                //    services.AddSingleton(type, settings);
                //}
            }
        }

    }
}
