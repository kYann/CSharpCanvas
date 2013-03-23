using Cairo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCanvas
{
    public class Canvas : IDisposable
    {
        public Canvas(int width, int height)
        {
            this.Surface = new ImageSurface(Format.Argb32, width, height);
            this.Context = new Context(Surface);
        }

        internal Surface Surface { get; private set; }

        internal Context Context { get; private set; }

        public void SaveToPng(string path)
        {
            this.Surface.WriteToPng(path);
        }

        public CanvasRenderingContext2d getContext(string context)
        {
            if (context != "2d")
                throw new NotSupportedException("Context is not supported " + context);
            return new CanvasRenderingContext2d(this);
        }

        public void Dispose()
        {
            if (this.Context != null)
                (this.Context as IDisposable).Dispose();
            if (this.Surface != null)
                this.Surface.Dispose();
        }
    }
}
