﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Transport.Interfaces;

namespace Transport.Core
{
    [Export(typeof(ITransportFactory))]
    public sealed class CompositeTransportFactory : ITransportFactory
    {
        private readonly IEnumerable<Lazy<ITransportProvider, ITransportMetadata>> _transports;

        [ImportingConstructor]
        public CompositeTransportFactory([ImportMany]IEnumerable<Lazy<ITransportProvider, ITransportMetadata>> transports)
        {
            _transports = transports;
        }

        public ITransport<T> Create<T>(string name, Func<ITransportConfiguration, ITransportConfiguration> configuration)
        {
            var transport = _transports.FirstOrDefault(t => t.Metadata.Name == name);

            return transport?.Value.Create<T>(configuration);
        }
    }
}