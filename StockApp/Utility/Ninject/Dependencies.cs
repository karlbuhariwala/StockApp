﻿// Copyright = Karl Buhariwala
// ServiceMe App
// FileName = Dependencies.cs

namespace StockApp.Utility.Ninject
{
    using global::Ninject.Modules;
    using Provider.DiviData;
    using Provider.GoogleFinancePage;
    using Provider.GoogleStock;
    using Provider.IExTradingProvider;
    using Provider.YahooEarnings;
    using Provider.YahooStock;
    using StockApp.Interfaces;
    using StockApp.Provider.SqlProvider;
    using StockApp.Provider.StockProvider;
    using StockApp.ServiceLayer;

    public class Dependencies : NinjectModule
    {
        public override void Load()
        {
            // Providers
            this.Bind<IStorageProvider>().To<SqlProvider>();
            this.Bind<IStockProvider>().To<IExTradingProvider>();
            this.Bind<IGoogleProvider>().To<GoogleProvider>();
            this.Bind<ICurrentVolumeProvider>().To<GoogleFinancePageProvider>();
            this.Bind<IYahooProvider>().To<YahooProvider>();
            this.Bind<IExDividendDateProvider>().To<DiviDataProvider>();
            this.Bind<IEarningsDateProvider>().To<YahooEarningsProvider>();

            // Service layer
            this.Bind<StockListGenerator>().To<StockListGenerator>().InSingletonScope();
            this.Bind<LiveStockInfoCollector>().To<LiveStockInfoCollector>();
            this.Bind<StockRangeGenerator>().To<StockRangeGenerator>();
            this.Bind<StockScoreGenerator>().To<StockScoreGenerator>();
            this.Bind<TradingHoursChecker>().To<TradingHoursChecker>();

            // WorkFlow layer
            this.Bind<StockInformationCollector>().To<StockInformationCollector>();
            this.Bind<StockInformationAfterHoursProcessor>().To<StockInformationAfterHoursProcessor>();
        }
    }
}
