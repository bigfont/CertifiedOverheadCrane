﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProfiledSqlCeDataServicesProvider.cs" company="Daniel Dabrowski - rod.42n.pl">
//   Copyright (c) 2008 Daniel Dabrowski - 42n. All rights reserved.
// </copyright>
// <summary>
//   Defines the ProfiledSqlCeDataServicesProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Four2n.Orchard.MiniProfiler.Data.Providers
{
    using FluentNHibernate.Cfg.Db;
    using global::Orchard.Environment.Extensions;
    using MsSqlCeConfiguration = global::Orchard.Data.Providers.MsSqlCeConfiguration;

    [OrchardSuppressDependency("Orchard.Data.Providers.SqlCeDataServicesProvider")]
    public class ProfiledSqlCeDataServicesProvider : global::Orchard.Data.Providers.SqlCeDataServicesProvider
    {
        public ProfiledSqlCeDataServicesProvider(string dataFolder, string connectionString)
            : base(dataFolder, connectionString)
        {
        }

        public static string ProviderName
        {
            get { return global::Orchard.Data.Providers.SqlCeDataServicesProvider.ProviderName; }
        }

        public override IPersistenceConfigurer GetPersistenceConfigurer(bool createDatabase)
        {
            var persistence = (MsSqlCeConfiguration)base.GetPersistenceConfigurer(createDatabase);
            return persistence.Driver(typeof(ProfiledSqlServerCeDriver).AssemblyQualifiedName);
        }
    }
}