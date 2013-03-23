using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCanvas
{
    public partial class CanvasRenderingContext2d
    {
        Canvas2DState state = new Canvas2DState();
        readonly Canvas canvas = null;
        Stack<Canvas2DState> stateStack = new Stack<Canvas2DState>();

        Path path = null;

        public CanvasRenderingContext2d(Canvas canvas)
        {
            state.shadowBlur = 0;
            state.shadowOffsetX = 0;
            state.shadowOffsetY = 0;
            state.globalAlpha = 1;
            state.fillColor = new Color(0, 0, 0, 1);
            state.strokeColor = new Color(0, 0, 0, 1);
            state.patternQuality = Cairo.Filter.Good;
            state.textDrawingMode = "path";
            canvas.Context.Antialias = Antialias.Subpixel;

            this.canvas = canvas;
        }

        private void savePath()
        {
            this.path = canvas.Context.CopyPathFlat();
            canvas.Context.NewPath();
        }

        private void restorePath()
        {
            canvas.Context.NewPath();
            canvas.Context.AppendPath(this.path);
            this.path.Dispose();
        }

        private void saveState()
        {
            stateStack.Push(state.Clone());
        }

        private void restoreState()
        {
            this.state = stateStack.Pop();
        }

        public void drawImage(string path, params double[] args)
        {
            double sx = 0
            , sy = 0
            , sw = 0
            , sh = 0
            , dx, dy, dw, dh;

            switch (args.Length) 
            {
                // img, sx, sy, sw, sh, dx, dy, dw, dh
                case 8:
                  sx = args[0];
                  sy = args[1];
                  sw = args[2];
                  sh = args[3];
                  dx = args[4];
                  dy = args[5];
                  dw = args[6];
                  dh = args[7];
                  break;
                // img, dx, dy, dw, dh
                case 4:
                  dx = args[0];
                  dy = args[1];
                  dw = args[2];
                  dh = args[3];
                  break;
                // img, dx, dy
                case 2:
                  dx = args[0];
                  dy = args[1];
                  dw = sw;
                  dh = sh;
                  break;
                default:
                  throw new ArgumentException();
              }

            using (var imgSurface = new ImageSurface(path))
            {

                sw = imgSurface.Width;
                sh = imgSurface.Height;

                // Start draw
                canvas.Context.Save();

                this.savePath();
                canvas.Context.Rectangle(dx, dy, dw, dh);
                canvas.Context.Clip();
                this.restorePath();

                if (dw != sw || dh != sh)
                {
                    double fx = (double)dw / sw;
                    double fy = (double)dh / sh;
                    //scale 
                    using (var imgContext = new Context(imgSurface))
                    {
                        imgContext.Scale(fx, fy);
                        dx /= fx;
                        dy /= fy;
                    }
                }

                using (var pattern = new SurfacePattern(imgSurface))
                {
                    pattern.Filter = state.patternQuality;
                    canvas.Context.SetSource(imgSurface, dx - sx, dy - sy);
                    canvas.Context.PaintWithAlpha(state.globalAlpha);
                }
                canvas.Context.Restore();
            }
        }

        public void putImageData(string path, double dx, double dy, double? dirtyX = null, double? dirtyY = null, double? dirtyWidth = null, double? dirtyHeight = null)
        {
            throw new NotImplementedException();
        }

        public void addPage()
        {
            throw new NotImplementedException();
        }

        public void setTransform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            throw new NotImplementedException();
        }

        public void save()
        {
            this.canvas.Context.Save();
            this.saveState();
        }

        public void restore()
        {
            this.canvas.Context.Restore();
            this.restoreState();
        }

        public void rotate(double angle)
        {
            this.canvas.Context.Rotate(angle);
        }

        public void translate(double x, double y)
        {
            this.canvas.Context.Translate(x, y);
        }

        public void transform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            throw new NotImplementedException();
        }

        public void resetTransform()
        {
            throw new NotImplementedException();
        }

        public bool isPointInPath(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void scale(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void clip()
        {
            this.canvas.Context.ClipPreserve();
        }

        public void fill()
        {
            this.canvas.Context.SetSourceRGBA(
                state.fillColor.R,
                state.fillColor.G,
                state.fillColor.B,
                state.fillColor.A);
            this.canvas.Context.FillPreserve();
        }

        public void stroke()
        {
            this.canvas.Context.SetSourceRGBA(
                state.strokeColor.R,
                state.strokeColor.G,
                state.strokeColor.B,
                state.strokeColor.A);
            this.canvas.Context.StrokePreserve();
        }

        public void fillText(string text, double x, double y, double? maxWidth = null)
        {
            this.savePath();
            if (this.state.textDrawingMode == "glyph")
            {
                this.canvas.Context.Fill();
                this.canvas.Context.TextPath(text);
            }
            else if (this.state.textDrawingMode == "path")
            {
                this.canvas.Context.TextPath(text);
                this.canvas.Context.Fill();
            }
            this.restorePath();
        }

        public void strokeText(string text, double x, double y, double? maxWidth = null)
        {
            throw new NotImplementedException();
        }

        public void fillRect(double x, double y, double width, double height)
        {
            throw new NotImplementedException();
        }

        public void strokeRect()
        {
            throw new NotImplementedException();
        }

        public void clearRect(double x, double y, double width, double height)
        {
            throw new NotImplementedException();
        }

        public void rect(double x, double y, double width, double height)
        {
            throw new NotImplementedException();
        }

        public void measureText(string text)
        {
            throw new NotImplementedException();
        }

        public void moveTo(double x, double y)
        {
            this.canvas.Context.MoveTo(x, y);
        }

        public void lineTo(double x, double y)
        {
            this.canvas.Context.LineTo(x, y);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            this.canvas.Context.CurveTo(cp1x, cp1y, cp2x, cp2y, x, y);
        }

        public void quadraticCurveTo(double x1, double y1, double x2, double y2)
        {
            double x = this.canvas.Context.CurrentPoint.X;
            double y = this.canvas.Context.CurrentPoint.X;

            if (x == 0 && y == 0)
            {
                x = x1;
                y = y1;
            }

            this.canvas.Context.CurveTo(x + 2.0 / 3.0 * (x1 - x), y + 2.0 / 3.0 * (y1 - y)
                , x2 + 2.0 / 3.0 * (x1 - x2), y2 + 2.0 / 3.0 * (y1 - y2)
                , x2
                , y2);
        }

        public void beginPath()
        {
            this.canvas.Context.NewPath();
        }

        public void closePath()
        {
            this.canvas.Context.ClosePath();
        }

        public void arc(double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise = false)
        {
            throw new NotImplementedException();
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            throw new NotImplementedException();
        }
    }
}
