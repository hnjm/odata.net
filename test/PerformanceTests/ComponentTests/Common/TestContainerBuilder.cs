﻿//---------------------------------------------------------------------
// <copyright file="TestContainerBuilder.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace Microsoft.OData.Performance.Common
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.DependencyInjection;

    public class TestContainerBuilder : IContainerBuilder
    {
        private readonly IServiceCollection services = new ServiceCollection();

        public IContainerBuilder AddService(
            OData.ServiceLifetime lifetime,
            Type serviceType,
            Type implementationType)
        {
            Debug.Assert(serviceType != null, "serviceType != null");
            Debug.Assert(implementationType != null, "implementationType != null");

            services.Add(new ServiceDescriptor(
                serviceType, implementationType, TranslateServiceLifetime(lifetime)));

            return this;
        }

        public IContainerBuilder AddService(
            OData.ServiceLifetime lifetime,
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            Debug.Assert(serviceType != null, "serviceType != null");
            Debug.Assert(implementationFactory != null, "implementationFactory != null");

            services.Add(new ServiceDescriptor(
                serviceType, implementationFactory, TranslateServiceLifetime(lifetime)));

            return this;
        }

        public IServiceProvider BuildContainer()
        {
            return services.BuildServiceProvider();
        }

        private static ServiceLifetime TranslateServiceLifetime(
            OData.ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case OData.ServiceLifetime.Scoped:
                    return ServiceLifetime.Scoped;
                case OData.ServiceLifetime.Singleton:
                    return ServiceLifetime.Singleton;
                default:
                    return ServiceLifetime.Transient;
            }
        }
    }
}