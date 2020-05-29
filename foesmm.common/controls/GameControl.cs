using System.IO;
using Eto.Drawing;
using Eto.Forms;

namespace foesmm.controls
{
    public sealed class GameControl : StackLayout
    {
        public static Size CoverSize => new ScaledSize(240, 300);
        public IGame Game { get; }

        private readonly ImageView _cover;
        private readonly ImageView _overlay;
        private readonly Label _title;

        private GameControl()
        {
            Orientation = Orientation.Vertical;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;

            var coverHolder = new PixelLayout();
            _cover = new ImageView {Size = CoverSize};
            coverHolder.Add(_cover, 0, 0);
            _overlay = new ImageView {Size = CoverSize};
            coverHolder.Add(_overlay, 0, 0);
            Items.Add(coverHolder);

            _title = new Label
            {
                TextAlignment = TextAlignment.Center,
            };
            Items.Add(_title);
        }

        public GameControl(IGame game) : this()
        {
            Game = game;
            _cover.Image = new Bitmap(Path.Combine(App.DataPath, "Covers",
                game.Info.Guid + ".png"));
            _overlay.Image =
                new Bitmap(Path.Combine(App.DataPath, "Overlays", $"ReleaseState.{game.Info.ReleaseState}.png"));
            _title.Text = game.Title;
        }
    }
}