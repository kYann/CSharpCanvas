using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCanvas
{
    public partial class CanvasRenderingContext2d
    {
        protected Cairo.Color ParseColor(string value)
        {
            var idxRgb = value.IndexOf("rgb(");
            var idxRgba = value.IndexOf("rgba(");

            if (idxRgb < 0 && idxRgba < 0)
                throw new NotSupportedException("Fillstyle syntax is not supported");
            var idxStart = Math.Max(idxRgba, idxRgb);
            idxStart += idxRgb < 0 ? 5 : 4;
            var fctEnd = value.IndexOf(")", idxStart);
            var fctArgs = value.Substring(idxStart, fctEnd - idxStart);

            var args = fctArgs.Split(',').Select(c => c.Trim()).ToList();
            Func<string, double> dInvParse = (s) => double.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
            var color = new Cairo.Color(dInvParse(args[0]) / 255.0,
                dInvParse(args[1]) / 255.0,
                dInvParse(args[2]) / 255.0);
            if (args.Count == 4)
                color.A = dInvParse(args[3]);

            return color;
        }

        public string patternQuality
        {
            get 
            { 
                switch (state.patternQuality)
                {
                    case Cairo.Filter.Best:
                        return "best";
                    case Cairo.Filter.Fast:
                        return "fast";
                    case Cairo.Filter.Good:
                        return "good";
                }
                return null;
            }
            set 
            {
                switch (value)
                {
                    case "best":
                        state.patternQuality = Cairo.Filter.Best;
                        break;
                    case "fast":
                        state.patternQuality = Cairo.Filter.Fast;
                        break;
                    case "good":
                        state.patternQuality = Cairo.Filter.Good;
                        break;
                }
            }
        }

        public string globalCompositeOperation
        {
            get 
            { 
                switch (this.canvas.Context.Operator)
                {
                    case Cairo.Operator.Atop:
                        return "source-atop";
                    case Cairo.Operator.In:
                        return "source-in";
                    case Cairo.Operator.Out:
                        return "source-out";
                    case Cairo.Operator.Xor:
                        return "xor";
                    case Cairo.Operator.DestAtop:
                        return "destination-atop";
                    case Cairo.Operator.DestIn:
                        return "destination-in";
                    case Cairo.Operator.DestOut:
                        return "destination-out";
                    case Cairo.Operator.DestOver:
                        return "destination-over";
                    case Cairo.Operator.Add:
                        return "lighter";
                    case Cairo.Operator.Clear:
                        return "clear";
                    case Cairo.Operator.Source:
                        return "source";
                    case Cairo.Operator.Dest:
                        return "dest";
                    case Cairo.Operator.Over:
                        return "over";
                    case Cairo.Operator.Saturate:
                        return "saturate";
                }
                return null;
            }
            set 
            {
                switch (value)
                {
                    case "source-atop":
                        canvas.Context.Operator = Cairo.Operator.Atop; break;
                    case "source-in":
                        canvas.Context.Operator = Cairo.Operator.In; break;
                    case "source-out":
                        canvas.Context.Operator = Cairo.Operator.Out; break;
                    case "xor":
                        canvas.Context.Operator = Cairo.Operator.Xor; break;
                    case "destination-atop":
                        canvas.Context.Operator = Cairo.Operator.DestAtop; break;
                    case "destination-in":
                        canvas.Context.Operator = Cairo.Operator.DestIn; break;
                    case "destination-out":
                        canvas.Context.Operator = Cairo.Operator.DestOut; break;
                    case "destination-over":
                        canvas.Context.Operator = Cairo.Operator.DestOver; break;
                    case "lighter":
                        canvas.Context.Operator = Cairo.Operator.Add; break;
                    case "clear":
                        canvas.Context.Operator = Cairo.Operator.Clear; break;
                    case "source":
                        canvas.Context.Operator = Cairo.Operator.Source; break;
                    case "dest":
                        canvas.Context.Operator = Cairo.Operator.Dest; break;
                    case "over":
                        canvas.Context.Operator = Cairo.Operator.Over; break;
                    case "saturate":
                        canvas.Context.Operator = Cairo.Operator.Saturate; break;
                }
            }
        }

        public double globalAlpha
        {
            get { return state.globalAlpha; }
            set { state.globalAlpha = value; }
        }

        public string shadowColor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string fillStyle
        {
            get 
            {
                return "rgba(" + state.fillColor.R
                    + "," + state.fillColor.G
                    + "," + state.fillColor.B
                    + "," + state.fillColor.A + ")"; 
            }
            set 
            {
                this.state.fillColor = ParseColor(value);
            }
        }

        public string strokeStyle
        {
            get
            {
                return "rgba(" + state.strokeColor.R
                    + "," + state.strokeColor.G
                    + "," + state.strokeColor.B
                    + "," + state.strokeColor.A + ")";
            }
            set
            {
                this.state.strokeColor = ParseColor(value);
            }
        }

        public double miterLimit
        {
            get 
            {
                return this.canvas.Context.MiterLimit;
            }
            set 
            {
                this.canvas.Context.MiterLimit = value;
            }
        }

        public double lineWidth
        {
            get { return this.canvas.Context.LineWidth; }
            set { this.canvas.Context.LineWidth = value; }
        }

        public string lineCap
        {
            get 
            { 
                switch (this.canvas.Context.LineCap)
                {
                    case Cairo.LineCap.Butt:
                        return "butt";
                    case Cairo.LineCap.Round:
                        return "round";
                    case Cairo.LineCap.Square:
                        return "square";
                }
                return "butt";
            }
            set 
            {
                switch (value)
                {
                    case "round":
                        this.canvas.Context.LineCap = Cairo.LineCap.Round; break;
                    case "square":
                        this.canvas.Context.LineCap = Cairo.LineCap.Square; break;
                    default:
                        this.canvas.Context.LineCap = Cairo.LineCap.Butt; break;
                }
            }
        }

        public string lineJoin
        {
            get
            {
                switch (this.canvas.Context.LineJoin)
                {
                    case Cairo.LineJoin.Bevel:
                        return "bevel";
                    case Cairo.LineJoin.Miter:
                        return "miter";
                    case Cairo.LineJoin.Round:
                        return "round";
                }
                return "miter";
            }
            set
            {
                switch (value)
                {
                    case "bevel":
                        this.canvas.Context.LineJoin = Cairo.LineJoin.Bevel; break;
                    case "round":
                        this.canvas.Context.LineJoin = Cairo.LineJoin.Round; break;
                    default:
                        this.canvas.Context.LineJoin = Cairo.LineJoin.Miter; break;
                }
            }
        }

        public int shadowOffsetX
        {
            get { return state.shadowOffsetX; }
            set { state.shadowOffsetX = value; }
        }

        public int shadowOffsetY
        {
            get { return state.shadowOffsetY; }
            set { state.shadowOffsetY = value; }
        }

        public int shadowBlur
        {
            get { return state.shadowBlur; }
            set { state.shadowBlur = value; }
        }

        public string antialias
        {
            get 
            { 
                switch (this.canvas.Context.Antialias)
                {
                    case Cairo.Antialias.None:
                        return "none";
                    case Cairo.Antialias.Gray:
                        return "gray";
                    case Cairo.Antialias.Subpixel:
                        return "subpixel";
                    default:
                        return "default";
                }
            }
            set 
            {
                switch (value)
                {
                    case "none":
                        this.canvas.Context.Antialias = Cairo.Antialias.None; break;
                    case "gray":
                        this.canvas.Context.Antialias = Cairo.Antialias.Gray; break;
                    case "subpixel":
                        this.canvas.Context.Antialias = Cairo.Antialias.Subpixel; break;
                    default:
                        this.canvas.Context.Antialias = Cairo.Antialias.Default; break;
                }
            }
        }

        public string textDrawingMode
        {
            get { return state.textDrawingMode; }
            set { state.textDrawingMode = value; }
        }
    }
}
