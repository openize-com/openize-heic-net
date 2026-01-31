using System;
using System.ComponentModel;

namespace Openize.Heic.Decoder
{
    /// <summary>
    /// Stores a set of four integers that represent the location and size of a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='System.Drawing.Rectangle'/>.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='System.Drawing.Rectangle'/>.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this Rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this Rectangle.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the Rectangle with the specified location and size.
        /// </summary>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new Rectangle with the specified location and size.
        /// </summary>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom) =>
            new Rectangle(left, top, unchecked(right - left), unchecked(bottom - top));

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this object.
        /// </summary>
        [Browsable(false)]
        public int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this object.
        /// </summary>
        [Browsable(false)]
        public int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this object.
        /// </summary>
        [Browsable(false)]
        public int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this object.
        /// </summary>
        [Browsable(false)]
        public int Bottom => unchecked(Y + Height);

        /// <summary>
        /// Returns true if all the numeric properties of this Rectangle have values of zero; otherwise, false.
        /// </summary>
        [Browsable(false)]
        public bool IsEmpty => Height == 0 && Width == 0 && X == 0 && Y == 0;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a Rectangle with the same location and size of this Rectangle.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle))
                return false;
            
            return this == (Rectangle)obj;
        }

        /// <summary>
        /// Tests whether the current Rectangle is equal to another Rectangle.
        /// </summary>
        public bool Equals(Rectangle other) => this == other;

        /// <summary>
        /// Tests whether two Rectangle objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rectangle left, Rectangle right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two Rectangle objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        /// <summary>
        /// Returns the hash code for this Rectangle.
        /// </summary>
        /// <returns>An integer that represents the hash code for this rectangle.</returns>
        public override int GetHashCode()
        {
            return (int)((UInt32)X ^
                        (((UInt32)Y << 13) | ((UInt32)Y >> 19)) ^
                        (((UInt32)Width << 26) | ((UInt32)Width >> 6)) ^
                        (((UInt32)Height << 7) | ((UInt32)Height >> 25)));
        }

        /// <summary>
        /// Converts the attributes of this Rectangle to a human readable string.
        /// </summary>
        public override string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }
}
