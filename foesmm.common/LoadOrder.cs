using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Eto;

namespace foesmm
{
    public class LoadOrder
    {
        private IGame _game;
        private PluginsTxt _pluginsTxt;
        private bool _initialized;

        private readonly DateTime _epoch;

        private DateTime DateForPlugin(int index)
        {
            return _epoch.AddDays(index);
        }

        public IEnumerable<IPlugin> Plugins => _plugins;
        public IEnumerable<IArchive> Archives => _archives;
        public IEnumerable<IScriptExtenderPlugin> Extensions => _extensions;

        private readonly List<IPlugin> _pluginList;
        private readonly BindingListProxy<IPlugin> _plugins;
        private readonly BindingListProxy<IArchive> _archives;
        private readonly ObservableCollection<IScriptExtenderPlugin> _extensions;

        private static int LastWriteTimeComparator(IPlugin one, IPlugin two) =>
            one.File.LastWriteTime.CompareTo(two.File.LastWriteTime);

        private static int FileNameComparator(IScriptExtenderPlugin one, IScriptExtenderPlugin two) =>
            string.Compare(one.File.Name, two.File.Name, StringComparison.InvariantCultureIgnoreCase);

        public LoadOrder(IGame game)
        {
            _game = game;
            _epoch = new DateTime(_game.Info.ReleaseYear, 1, 1, 0, 0, 0);

            _pluginsTxt = new PluginsTxt(Path.Combine(_game.Directories.ApplicationData, "plugins.txt"));

            _pluginList = new List<IPlugin>();
            _plugins = new BindingListProxy<IPlugin>(_pluginList);
            _plugins.ListChanged += (sender, args) =>
            {
                Debug.WriteLine(args.ListChangedType);
                switch (args.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        break;
                    case ListChangedType.ItemChanged:
                        if (args.PropertyDescriptor != null &&
                            args.PropertyDescriptor.Name.Equals(nameof(IPlugin.Active)))
                        {
                            UpdatePluginIndexes();
                            _plugins.NotifyCollectionChanged(
                                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                            _archives.NotifyCollectionChanged(
                                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        }

                        break;
                }
            };
            _archives = new BindingListProxy<IArchive>();
            _extensions = new ObservableCollection<IScriptExtenderPlugin>();

            Discover();
        }

        private void UpdatePluginIndexes()
        {
            var activePlugins = _pluginList.Where(x => x.Active).ToList();
            foreach (var p in _plugins)
            {
                ((Plugin) p).SetIndex(activePlugins.IndexOf(p));
            }
        }

        private void Discover()
        {
            var dataDirectory = new DirectoryInfo(_game.Directories.Data);

            var extensions = new[] {".esm", ".esp"};
            foreach (var file in dataDirectory.EnumerateFiles())
            {
                if (!extensions.Contains(file.Extension.ToLower())) continue;

                var plugin = Plugin.Load(file, _game.Info.Plugins.FirstOrDefault(x => x.Name.Equals(file.Name)));
                plugin.Active = _pluginsTxt.Plugins.Contains(file.Name.ToLower());
                Add(plugin);
            }

            var extensionsDirectory = new DirectoryInfo(_game.Directories.ScriptExtender);
            foreach (var file in extensionsDirectory.EnumerateFiles())
            {
                if (!file.Extension.ToLower().Equals(".dll")) continue;

                var extension = ScriptExtenderPlugin.Load(file);
                Add(extension);
            }

            _pluginList.Sort(LastWriteTimeComparator);
            _plugins.ResetBindings();
            UpdatePluginIndexes();
            DiscoverArchives();

            _initialized = true;
        }

        private void DiscoverArchives()
        {
            _archives.Clear();
            
            foreach (var plugin in _plugins)
            {
                foreach (var archive in plugin.Archives)
                {
                    _archives.Add(archive);
                }
            }

            var updateFile = new FileInfo(Path.Combine(_game.Directories.Data, "Update.bsa"));
            if (updateFile.Exists)
            {
                _archives.Add(Archive.Load(updateFile,
                    _pluginList.FirstOrDefault(p => p.Info.Guid.Equals(_game.Info.Guid))));
            }
            _archives.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(IPlugin plugin)
        {
            _plugins.Add(plugin);

            if (_initialized)
            {
                _pluginList.Sort(LastWriteTimeComparator);
                DiscoverArchives();
            }
            else
            {
                foreach (var archive in plugin.Archives)
                {
                    _archives.Add(archive);
                }
            }
        }

        public void Add(IScriptExtenderPlugin extension)
        {
            _extensions.Add(extension);
        }

        public void Move(IPlugin plugin, int index)
        {
            var prevIndex = _plugins.IndexOf(plugin);
            _plugins.RemoveAt(prevIndex);
            if (prevIndex <= index)
            {
                index--;
            }
            _plugins.Insert(index, plugin);
            UpdatePluginIndexes();
            _plugins.NotifyCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            DiscoverArchives();
        }
    }
}