using System;
using System.IO;
using System.Xml.Serialization;
using Eto.Drawing;
using Eto.Forms;

namespace foesmm.controls
{
    public class ModDescriptionControl : TableLayout
    {
        private ImageView _cover;
        private Label _title;
        private TextArea _description;

        public ModDescriptionControl() : base(2, 1)
        {
            Padding = 10;
            Spacing = new Size(10, 10);
            _cover = new ImageView();
            Add(_cover, 0, 0, false, false);

            _title = new Label
            {
            };
            _description = new TextArea
            {
                Enabled = false,
                TextColor = _title.TextColor
            };
            var modDescription = new TableLayout(1, 2)
            {
                Spacing = new Size(0, 10)
            };
            modDescription.Add(_title, 0, 0, true, false);
            modDescription.Add(_description, 0, 1, true, true);
            Add(modDescription, 1, 0, true, true);
        }

        public void UpdateInfo(IModInfo modInfo)
        {
            _title.Text = modInfo.Title;
            _description.Text = modInfo.Description;

            var image = Path.Combine(App.DataPath, "Covers",
                modInfo.Guid + ".png");
            if (File.Exists(image))
            {
                _cover.Image = new Bitmap(image);
            }
            else
            {
                _cover.Image = new Bitmap(Path.Combine(App.DataPath, "Covers",
                    "no-cover.png"));
            }

            var size = _cover.Image.Size;
            _cover.Width = (int) Math.Ceiling(130 * (size.Width / (float) size.Height));
            _cover.Height = 130;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
    }
}