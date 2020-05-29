using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Eto.Drawing;
using Eto.Forms;
using foesmm.controls;

namespace foesmm.forms
{
    public sealed class ManageGameForm : Form
    {
        private TabControl _pluginsView;
        private ModDescriptionControl _modDescriptionControl;
        private GridView<IPlugin> _plugins;
        public IGame Game { get; }

        private string GetFormPrefix(int index)
        {
            return index == -1 ? "" : index.ToString("X2");
        }

        private string GetTitle(IPlugin plugin)
        {
            if (plugin?.Info == null) return "";
            var modInfo = Game.Info.Addons.FirstOrDefault(x => x.Guid.Equals(plugin.Info.Guid));
            if (modInfo != null) return $"{plugin.Info.Type}: {modInfo.Title}";
            return plugin.Info.Guid.Equals(Game.Info.Guid) ? $"{plugin.Info.Type}: {Game.Info.Title}" : "";
        }

        public ManageGameForm(IGame game)
        {
            Game = game;
            Size = new Size(800, 600);

            var layout = new TableLayout(2, 2);
            var pluginsViewContainer = new Panel
            {
                Padding = 10
            };
            _pluginsView = new TabControl();
            pluginsViewContainer.Content = _pluginsView;

            var pluginsTab = new TabPage {Text = "Plugins"};

            _plugins = new GridView<IPlugin>
            {
                DataStore = game.LoadOrder.Plugins,
                AllowDrop = true,
                ShowHeader = false,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell {Binding = Binding.Property<IPlugin, bool?>(p => p.Active)},
                        Editable = true,
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<IPlugin, string>(p => p.Index)
                        }
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell {Binding = Binding.Property<IPlugin, string>(p => p.File.Name)}
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<IPlugin, string>(p => GetTitle(p))
                        }
                    },
                    // @todo: add version column
                    // new GridColumn
                    // {
                    //     DataCell = new TextBoxCell
                    //     {
                    //         Binding = Binding.Property<IPlugin, string>(p => "1.0")
                    //     }
                    // }
                }
            };
            _plugins.MouseMove += PluginsOnMouseMove;
            _plugins.DragOver += PluginsOnDragOver;
            _plugins.DragDrop += PluginsOnDragDrop;
            pluginsTab.Content = _plugins;
            _pluginsView.Pages.Add(pluginsTab);
            var archivesTab = new TabPage {Text = "Archives"};
            var archives = new GridView<IArchive>
            {
                DataStore = Game.LoadOrder.Archives,
                ShowHeader = false,
                Columns =
                {
                    // @todo: make active refresh when changing plugin active flag
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell {Binding = Binding.Property<IArchive, bool?>(p => p.Active)},
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell {Binding = Binding.Property<IArchive, string>(p => p.File.Name)}
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<IArchive, string>(p => GetTitle(p.Plugin))
                        }
                    }
                }
            };
            archivesTab.Content = archives;
            _pluginsView.Pages.Add(archivesTab);
            var extenderTab = new TabPage {Text = "Extender Plugins"};
            var extenderPlugins = new GridView<IScriptExtenderPlugin>
            {
                DataStore = Game.LoadOrder.Extensions,
                ShowHeader = false,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell
                            {Binding = Binding.Property<IScriptExtenderPlugin, bool?>(p => true)},
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                            {Binding = Binding.Property<IScriptExtenderPlugin, string>(p => p.File.Name)}
                    },
                }
            };
            extenderTab.Content = extenderPlugins;
            _pluginsView.Pages.Add(extenderTab);

            _pluginsView.SelectedIndexChanged += (sender, args) =>
            {
                _plugins.SelectedRow = -1;
                archives.SelectedRow = -1;
                extenderPlugins.SelectedRow = -1;
            };
            _plugins.SelectedItemsChanged += OnSelectedItemChanged;
            archives.SelectedItemsChanged += OnSelectedItemChanged;
            extenderPlugins.SelectedItemsChanged += OnSelectedItemChanged;

            _modDescriptionControl = new ModDescriptionControl
            {
                Padding = 10,
                Height = new ScaledSize(-1, 300).Height,
                Visible = false
            };
            layout.Add(pluginsViewContainer, 0, 0, true, true);
            layout.Add(_modDescriptionControl, 0, 1, true, false);
            var breakButton = new Button
            {
                Text = "Break",
            };
            breakButton.Click += (sender, args) => { Debug.WriteLine("Break"); };

            var controlStack = new StackLayout
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = 10,
                Spacing = 10,
            };
            controlStack.Items.Add(breakButton);
            controlStack.Items.Add(new Button
            {
                Text = "Package manager",
                Command = OpenPackageManagerCommand
            });
            layout.Add(controlStack, 1, 0, false, true);

            Content = layout;
        }

        private void PluginsOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Buttons == MouseButtons.Primary)
            {
                var cell = _plugins.GetCellAt(e.Location);
                if (cell.Item == null)
                    return;

                var plugin = new DataObject
                {
                    Text = ((IPlugin) cell.Item).File.Name
                };
                _plugins.DoDragDrop(plugin, DragEffects.Move);
                e.Handled = true;
            }
        }

        private void PluginsOnDragOver(object sender, DragEventArgs e)
        {
            var info = _plugins.GetDragInfo(e);
            if (info == null) return;
            e.Effects = DragEffects.Move;
            info.RestrictToInsert();
        }

        private void PluginsOnDragDrop(object sender, DragEventArgs e)
        {
            var info = _plugins.GetDragInfo(e);
            var plugin = Game.LoadOrder.Plugins.FirstOrDefault(p => p.File.Name.Equals(e.Data.Text));
            Game.LoadOrder.Move(plugin, info.InsertIndex);
        }

        public ICommand OpenPackageManagerCommand => new Command((sender, args) =>
        {
            new PackageManager(Game)
            {
                ShowActivated = true,
            }.Show();
        });

        private void OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (sender is GridView gridView)
            {
                var item = gridView.SelectedItem;
                switch (item)
                {
                    case IPlugin plugin when plugin.Info != null:
                    {
                        var modInfo = Game.Info.Addons.FirstOrDefault(x => x.Guid.Equals(plugin.Info.Guid)) ??
                                      (plugin.Info.Guid.Equals(Game.Info.Guid) ? Game.Info : null);
                        _modDescriptionControl.UpdateInfo(modInfo);
                        _modDescriptionControl.Visible = true;
                        break;
                    }
                    case IArchive archive when archive.Plugin?.Info != null:
                    {
                        var modInfo = Game.Info.Addons.FirstOrDefault(x => x.Guid.Equals(archive.Plugin.Info.Guid)) ??
                                      (archive.Plugin.Info.Guid.Equals(Game.Info.Guid) ? Game.Info : null);
                        _modDescriptionControl.UpdateInfo(modInfo);
                        _modDescriptionControl.Visible = true;
                        break;
                    }
                    default:
                        _modDescriptionControl.Visible = false;
                        break;
                }
            }

            Debug.WriteLine("Break");
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            // _pluginsView.Height = (int) Math.Ceiling(Size.Height * 0.8);
        }
    }
}