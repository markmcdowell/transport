﻿using System;
using System.ComponentModel.Composition.Hosting;
using Transport.Core;
using Transport.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Transport.InProcess.Tests
{
    public sealed class IntegrationTests
    {
        private readonly ITestOutputHelper _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldBeAbleToCreateTransport()
        {
            var inProcessCatalog = new AssemblyCatalog(typeof(PassThroughTransportProvider).Assembly);
            var coreCatalog = new AssemblyCatalog(typeof(TransportExtensions).Assembly);
            var catalog = new AggregateCatalog(inProcessCatalog, coreCatalog);

            var container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection);

            var transportFactory = container.GetExportedValue<ITransportFactory>();

            var transport = transportFactory.Create<string>(KnownTransports.InProcess.PassThrough);
            transport.Observe("topic/new")
                     .Subscribe(s => _output.WriteLine(s));
            transport.Publish("topic/new")
                     .OnNext("hello!");

            var intTransport = transportFactory.Create<int>(KnownTransports.InProcess.ReplayLast);            
            intTransport.Publish("topic/new")
                        .OnNext(10);
            intTransport.Observe("topic/new")
                        .Subscribe(s => _output.WriteLine(s.ToString()));
        }
    }
}