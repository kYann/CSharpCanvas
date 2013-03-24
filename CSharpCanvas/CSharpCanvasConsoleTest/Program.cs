using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpCanvas;

namespace CSharpCanvasConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var test1Js = File.ReadAllText("files/test1/no_draw_image.js");

            //CanvasScriptEngine.RenderScriptWith2dContextToPng("test1.png", "pdf", test1Js, 904, 1279);

            //Process.Start("test1.png");

            //var test1Js = File.ReadAllText("files/test2/draw_image.js");

            //CanvasScriptEngine.RenderScriptWith2dContextToPng("test2.png", "pdf", test1Js, 904, 1279);

            //Process.Start("test2.png");

            var test3Js = File.ReadAllText("files/test3/draw_image_failed.js");

            CanvasScriptEngine.RenderScriptWith2dContextToPng("test3.png", "pdf", test3Js, 904, 1279);

            Process.Start("test3.png");
        }
    }
}
