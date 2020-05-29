using System;
using Eto.Drawing;
using Eto.Forms;

namespace foesmm
{
    public class ScaledSize
    {
        private Size _size;

        public ScaledSize(Size size) : this(size.Width, size.Height)
        {
        }

        public ScaledSize(int width, int height)
        {
            _size = new Size(
                (int) Math.Ceiling(width / Screen.PrimaryScreen.RealScale),
                (int) Math.Ceiling(height / Screen.PrimaryScreen.RealScale)
            );
        }

        public int Height => _size.Height;

        public static implicit operator Size(ScaledSize scaledSize) => scaledSize._size;
    }
}