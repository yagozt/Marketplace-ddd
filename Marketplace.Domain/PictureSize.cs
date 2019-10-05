using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class PictureSize : Value<PictureSize>
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public PictureSize(int width, int height)
        {
            if (Width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Picture width must be a positive");
            if (Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height), "Picture height must be a positive");

            Width = width;
            Height = height;
        }
        internal PictureSize() { }
    }
}