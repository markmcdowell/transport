﻿using System;
using System.Threading.Tasks;

namespace Transport.Pipes
{
    internal sealed class DisposeDelegatingPipe : IPipe
    {
        private readonly IPipe _pipe;
        private readonly IDisposable _disposable;

        public DisposeDelegatingPipe(IPipe pipe, IDisposable disposable)
        {
            _pipe = pipe;
            _disposable = disposable;
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _pipe.Dispose();
        }

        public string Name => _pipe.Name;

        public event EventHandler<MessageEventArgs> MessageReceived
        {
            add { _pipe.MessageReceived += value; }
            remove { _pipe.MessageReceived -= value; }
        }

        public void Connect()
        {
            _pipe.Connect();
        }

        public Task Send(byte[] data)
        {
            return _pipe.Send(data);
        }

        public void Receive()
        {
            _pipe.Receive();
        }        
    }
}