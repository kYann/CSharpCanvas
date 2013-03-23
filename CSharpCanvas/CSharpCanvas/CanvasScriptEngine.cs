using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCanvas
{
    public class CanvasScriptEngine
    {
        private static void CallFunctionFromScriptInstruction(string scriptLine, CanvasRenderingContext2d context)
        {
            var idxArgs = scriptLine.IndexOf('(');
            if (idxArgs < 0) throw new NotSupportedException("Oupps, we don't support fancy js");
            idxArgs++;
            var rawArgs = scriptLine.Substring(idxArgs, scriptLine.IndexOf(')') - idxArgs);
            var args = rawArgs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim()).ToList();
            var idxFct = scriptLine.IndexOf('.');
            if (idxFct < 0 || idxFct >= idxArgs) throw new NotSupportedException("Oupps, we don't support fancy js");

            idxFct++;
            var fctName = scriptLine.Substring(idxFct, idxArgs - idxFct - 1);

            var method = context.GetType().GetMethod(fctName);
            if (method == null)
                throw new NotSupportedException("Could not find method " + fctName);
            var paramsInfo = method.GetParameters();

            if (paramsInfo.Length != args.Count && !paramsInfo.Last().ParameterType.IsArray)
                throw new NotSupportedException("Could not find method " + fctName + " with this signature ");
            var oParams = new object[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                var info = paramsInfo[i];
                if (i + 1 == paramsInfo.Length && paramsInfo[i].ParameterType.IsArray)
                {
                    var elemType = info.ParameterType.GetElementType();
                    var array = Array.CreateInstance(elemType, args.Count - i);
                    var idx = 0;
                    for (int j = i; j < args.Count; j++)
                    {
                        array.SetValue(Convert.ChangeType(args[j].Trim('\'', '"'), elemType, CultureInfo.InvariantCulture),
                            idx++);
                    }
                    oParams[i] = array;
                }
                else
                {
                    oParams[i] = Convert.ChangeType(args[i].Trim('\'', '"'), info.ParameterType, CultureInfo.InvariantCulture);
                }
            }
            method.Invoke(context, oParams);
        }

        private static void CallAssignFromScriptInstruction(string scriptLine, CanvasRenderingContext2d context)
        {
            var parts = scriptLine.Split('=').Select(c => c.Trim()).ToList();

            if (parts.Count != 2) throw new NotSupportedException("Oupps, we don't support fancy js");
            var idxProp = parts[0].IndexOf('.');
            if (idxProp < 0) throw new NotSupportedException("Oupps, we don't support fancy js");

            var propName = parts[0].Substring(idxProp + 1);
            var property = context.GetType().GetProperty(propName);
            var propRawValue = parts[1].Trim('"', '\'');
            var propValue = Convert.ChangeType(propRawValue, property.PropertyType, CultureInfo.InvariantCulture);

            property.SetValue(context, propValue);
        }

        public static void RenderScriptWith2dContextToPng(string pngPath, string contextVariableName, string script, int width, int height)
        {
            using (var canvas = new Canvas(width, height))
            {
                var context = canvas.getContext("2d");

                var instructions = script.Split(new[] { ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var inst in instructions)
                {
                    if (!inst.Contains(contextVariableName))
                        continue;
                    var idxAssign = inst.IndexOf('=');

                    if (idxAssign < 0)
                    {
                        CallFunctionFromScriptInstruction(inst, context);
                    }
                    else
                    {
                        CallAssignFromScriptInstruction(inst, context);
                    }
                }

                canvas.SaveToPng(pngPath);
            }
        }
    }
}
