using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Eto.Drawing;
using Eto.Forms;

namespace foesmm.controls
{
    public sealed class GameLibraryControl : PixelLayout
    {
        private TableLayout _layout;
        public event EventHandler<GameSelectedEventArgs> GameSelected;

        public GameLibraryControl()
        {
            _layout = new TableLayout();
            GameLibrary.Instance.Games.CollectionChanged += GamesOnCollectionChanged;

            AllowDrop = true;
            Rearrange();
        }

        private void GamesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Rearrange();
        }

        private void Rearrange()
        {
            RemoveAll();
            var x = 0;
            var y = 0;
            foreach (var game in GameLibrary.Instance.Games)
            {
                var control = new GameControl(game);
                control.MouseUp += (sender, args) =>
                    GameSelected?.Invoke(this, new GameSelectedEventArgs(((GameControl) sender).Game));
                Add(control, x, y);
                x += GameControl.CoverSize.Width;
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if (e.Data.ContainsUris)
            {
                e.Effects = DragEffects.Link;
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (e.Data.ContainsUris)
            {
                foreach (var uri in e.Data.Uris)
                {
                    GameLibrary.Instance.TryLoad(uri);
                }
            }
        }
    }
}