// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = ProviderFactory.cs

namespace RestServiceV1.Providers
{
    using StockApp.Provider;
    using System;
    using System.Configuration;
    using System.Runtime.Remoting;

    /// <summary>
    /// Provider factory that initializes providers
    /// </summary>
    public class ProviderFactory
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static ProviderFactory instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ProviderFactory Instance
        {
            get
            {
                if (ProviderFactory.instance == null)
                {
                    ProviderFactory.instance = new ProviderFactory();
                }

                return ProviderFactory.instance;
            }
        }

        /// <summary>
        /// Creates the provider.
        /// </summary>
        /// <typeparam name="T">Provider interface type</typeparam>
        /// <returns>The instance of the provider</returns>
        /// <exception cref="ApplicationException">No setting for Provider type: + providerType.Name</exception>
        public IProvider CreateProvider<T>(string providerName = null)
        {
            var providerType = typeof(T);
            string providerInstanceName = ConfigurationManager.AppSettings[providerType.Name];
            if (!string.IsNullOrEmpty(providerName))
            {
                providerInstanceName = "RestServiceV1.Providers." + providerName;
            }

            if (string.IsNullOrEmpty(providerInstanceName))
            {
                throw new ApplicationException("No setting for Provider type:" + providerType.Name);
            }

            string binaryName = ConfigurationManager.AppSettings["BinaryName"];
            ObjectHandle handle = Activator.CreateInstance(binaryName, providerInstanceName);
            return (IProvider)handle.Unwrap();
        }
    }
}