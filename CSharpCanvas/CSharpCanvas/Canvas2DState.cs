using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCanvas
{
    internal class Canvas2DState
    {
        public Cairo.Filter patternQuality { get; set; }

        public double globalAlpha { get; set; }

        public string shadowColor { get; set; }

        public Cairo.Color fillColor { get; set; }

        public Cairo.Color strokeColor { get; set; }

        public int shadowOffsetX { get; set; }

        public int shadowOffsetY { get; set; }

        public int shadowBlur { get; set; }

        public string textDrawingMode { get; set; }

        public Canvas2DState Clone()
        {
            return new Canvas2DState()
            {
                fillColor = this.fillColor,
                globalAlpha = this.globalAlpha,
                patternQuality = this.patternQuality,
                shadowBlur = this.shadowBlur,
                shadowColor = this.shadowColor,
                shadowOffsetX = this.shadowOffsetX,
                shadowOffsetY = this.shadowOffsetY,
                strokeColor = this.strokeColor,
                textDrawingMode = this.textDrawingMode
            };
        }
    }
}
