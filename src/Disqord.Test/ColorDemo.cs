using System;

namespace Disqord.Test
{
    class ColorDemo
    {
        void Code()
        {
            var color = Color.Goldenrod;
            color = new Color(0xDAA520);
            color = new Color(218, 165, 32);
            color = new Color(0.855f, 0.647f, 0.008f);

            // these are also implicit
            color = 0xDAA520;
            color = (218, 165, 32);
            color = (0.855f, 0.647f, 0.008f);

            // works with System.Drawing
            color = System.Drawing.Color.Goldenrod;

            // deconstructable to rgb
            var (r, g, b) = color;
            Console.WriteLine($"{r} {g} {b}");

            // implicit conversions from
            int raw = color;
            (byte R, byte G, byte B) tuple = color;
            System.Drawing.Color drawingColor = color;

            // utility
            color = Color.FromHsv(42, 0.85f, 0.85f);
            color = Color.Random; // hsv goodness
        }
    }
}