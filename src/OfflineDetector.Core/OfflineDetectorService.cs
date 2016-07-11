﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfflineDetector.Core.Configuration;

namespace OfflineDetector.Core
{
    public class OfflineDetectorService : IOfflineDetectorService
    {
        private IList<IUrlListener> _listeners;

        public event Action<EndPoint> ListenerStarted;

        public OfflineDetectorService()
        {
            _listeners = new List<IUrlListener>();
        }

        public void Start(IConfiguration configuration)
        {
            foreach (var endPoint in configuration.EndPoints)
            {
                IUrlListener listener = new UrlListener(endPoint);
                _listeners.Add(listener);
                if (ListenerStarted != null)
                {
                    listener.ListenerStarted += (endpoint) => ListenerStarted(endPoint);
                }

                Task.Factory.StartNew(() => listener.StartListening());
            }
        }
    }
}