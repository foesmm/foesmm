using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Eto.Drawing;
using Eto.Forms;
using Faithlife.Utility;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace foesmm.forms
{
    public sealed class PackageManager : Form
    {
        private IGame _game;

        public PackageManager(IGame game)
        {
            _game = game;
            Size = new Size(800, 600);

            var layout = new TableLayout(2, 2);
            var mods = new GridView<IModInfo>
            {
                DataStore = GenerateDataSource(),
                ShowHeader = true,
                AllowMultipleSelection = true,
                Columns =
                {
                    new GridColumn
                    {
                        DataCell = new CheckBoxCell
                        {
                        },
                    },
                    new GridColumn
                    {
                        DataCell = new TextBoxCell
                        {
                            Binding = Binding.Property<IModInfo, string>(x => x.Title)
                        },
                        HeaderText = "Modification",
                        Sortable = true
                    },
                    new GridColumn
                    {
                        DataCell = new ImageTextCell
                        {
                        },
                        HeaderText = "Version",
                    },
                }
            };
            var modsPanel = new Panel
            {
                Padding = 10,
                Content = mods
            };
            layout.Add(modsPanel, 0, 0, true, true);

            var controlsContainer = new StackLayout
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = 10,
                Spacing = 10,
                Items =
                {
                    new Button
                    {
                        Text = "Add Archive",
                        Command = new Command(AddArchive),
                        //_addArchiveCommand,
                        CommandParameter = this
                    },
                    new Button
                    {
                        Text = "Add Folder",
                        Command = _addFolderCommand,
                        CommandParameter = this
                    },
                    null,
                    new Button
                    {
                        Text = "Merge Mods"
                    },
                    null
                }
            };
            layout.Add(controlsContainer, 1, 0, false, true);

            Content = layout;
        }

        private async void AddArchive(object sender, EventArgs args)
        {
            var dialog = new OpenFileDialog
            {
                MultiSelect = true,
            };
            var result = dialog.ShowDialog(this);
            if (result == DialogResult.Ok)
            {
                var modsDirectory = new DirectoryInfo(Path.Combine(Helper.LocalApplicationDataDirectory.FullName,
                    "Mods", _game.Guid.ToString()));
                if (!modsDirectory.Exists)
                {
                    modsDirectory.Create();
                }

                foreach (var filename in dialog.Filenames)
                {
                    var file = new FileInfo(filename);
                    try
                    {
                        var archive = ArchiveFactory.Open(file);
                        App.Log.Info($"Adding file: {file.Name}");
                        var guid = GuidUtility.Create(_game.Guid, file.Name);
                        var modDirectory = modsDirectory.CreateSubdirectory(guid.ToString());
                        App.Log.Info($"{file.Name}: {archive.TotalSize} / {archive.TotalUncompressSize}");
                        archive.EntryExtractionBegin += (o, eventArgs) =>
                            App.Log.Info(
                                $"EntryExtractionBegin: {eventArgs.Item.Key} {eventArgs.Item.CompressedSize} / {eventArgs.Item.Size}");
                        archive.EntryExtractionEnd += (o, eventArgs) => App.Log.Info(
                            $"EntryExtractionEnd: {eventArgs.Item.Key} {eventArgs.Item.CompressedSize} / {eventArgs.Item.Size}");
                        archive.FilePartExtractionBegin += (o, eventArgs) =>
                            App.Log.Info(
                                $"FilePartExtractionBegin: {eventArgs.Name} {eventArgs.CompressedSize} / {eventArgs.Size}");
                        archive.CompressedBytesRead += (o, eventArgs) =>
                            App.Log.Info(
                                $"CompressedBytesRead: {eventArgs.CurrentFilePartCompressedBytesRead} / {eventArgs.CompressedBytesRead}");
                        
                        archive.WriteToDirectory(modDirectory.FullName, new ExtractionOptions {ExtractFullPath = true});
                        Debug.WriteLine("Break");
                    }
                    catch (InvalidOperationException e)
                    {
                        App.Log.Error($"Unknown file: {file.Name}. {e.Message}");
                    }
                }
            }
        }

        private IEnumerable<IModInfo> GenerateDataSource()
        {
            var mods = new List<IModInfo>();

            for (var i = 0; i < 10; i++)
            {
                mods.Add(new Addon
                {
                    Guid = Guid.NewGuid(),
                    Title = Guid.NewGuid().ToString(),
                });
            }

            return mods;
        }

        private readonly ICommand _addFolderCommand = new Command((sender, args) =>
        {
            var dialog = new SelectFolderDialog();
            var result = dialog.ShowDialog((Control) sender);
        });
    }
}