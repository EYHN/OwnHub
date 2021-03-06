﻿using System.IO;
using System.Text;
using SkiaSharp;
using Svg.Skia;

namespace Anything.Preview
{
    public class RenderUtils
    {
        public static void RenderSvg(RenderContext ctx, string svgStr, SKPaint? paint = null, SKPoint? point = null)
        {
            using SKSvg svg = new();
            svg.Load(new MemoryStream(Encoding.UTF8.GetBytes(svgStr)));

            ctx.Canvas.DrawPicture(svg.Picture, point ?? new SKPoint(0, 0), paint ?? new SKPaint());
        }
    }
}
