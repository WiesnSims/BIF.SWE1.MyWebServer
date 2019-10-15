using BIF.SWE1.Interfaces;
using MyWebServer.src.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MyWebServer
{
    class PluginManager : IPluginManager
    {
        private List<IPlugin> plugins;

        public PluginManager()
        {
            plugins = new List<IPlugin>();
            plugins.Add(new TestPlugin());
            //plugins.Add(new DefaultErrorPlugin());
            //plugins.Add(new ToLowerPlugin());
            //plugins.Add(new WebPagePlugin());
            //plugins.Add(new StaticFilePlugin());
            //plugins.Add(new TemperaturePlugin());
            //plugins.Add(new NaviPlugin());
        }

        public IEnumerable<IPlugin> Plugins
        {
            get { return this.plugins; }
        }

        public void Add(IPlugin plugin)
        {
            this.plugins.Add(plugin);
        }

        public void Add(string plugin)
        {
            Type t = Type.GetType(plugin);
            if (t.IsAssignableFrom(typeof(IPlugin)))
            {
                throw new InvalidOperationException("Object has to implement IPlugin.");
            }
            IPlugin newPlugin = (IPlugin)Activator.CreateInstance(t);
            plugins.Add(newPlugin);        
        }

        public void Clear()
        {
            this.plugins.Clear();
        }

        public IPlugin GetBestSuitingPlugin(IRequest request)
        {
            return plugins.OrderByDescending(x => x.CanHandle(request)).FirstOrDefault();
        }
    }
}
