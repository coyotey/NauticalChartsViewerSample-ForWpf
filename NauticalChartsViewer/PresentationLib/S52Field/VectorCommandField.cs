using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ThinkGeo.MapSuite
{
    internal class VectorCommandField
    {
        private readonly Collection<string> vectorCommands;

        public VectorCommandField(Collection<string> vectorCommands)
        {
            this.vectorCommands = vectorCommands;
        }

        public Collection<DAIShape> GetDrawingShapes(Dictionary<string, RGBColor> colorRef)
        {
            Collection<DAIShape> shapes = new Collection<DAIShape>();
            Collection<S52BaseSymbol> symbols = new Collection<S52BaseSymbol>();

            // get all symbol values
            Collection<string> symbolValues = vectorCommands;
            int penUpIndex = IndexOfSymbolValue(symbolValues, 0, symbolValues.Count, "PU");
            int ringEndIndex = 0;

            while (penUpIndex != -1)
            {
                int drawingIndex = IndexOfSymbolValue(symbolValues, penUpIndex + 1, symbolValues.Count - 1, "PD", "CI", "AA");
                int ringStartIndex = IndexOfSymbolValue(symbolValues, ringEndIndex + 1, drawingIndex - 1, "PM0");

                if (ringStartIndex == -1 && drawingIndex != -1)
                {
                    PUSymbol singleSymbol = GetSingleSymbolWithWidth(symbolValues, penUpIndex, ref drawingIndex);
                    //singleSymbols.Add(singleSymbol);
                    symbols.Add(singleSymbol);
                    penUpIndex = IndexOfSymbolValue(symbolValues, drawingIndex + 1, symbolValues.Count - 1, "PU");
                }
                else if (ringStartIndex == -1 && drawingIndex == -1)
                {
                    penUpIndex = -1;
                }
                else
                {
                    ringEndIndex = IndexOfSymbolValue(symbolValues, ringStartIndex + 1, symbolValues.Count, "PM2");
                    PMSymbol areaSymbol = GetAreaSymbol(symbolValues, ref penUpIndex, drawingIndex, ringStartIndex, ringEndIndex);
                    //areaSymbols.Add(areaSymbol);
                    symbols.Add(areaSymbol);
                }
            }

            //// get shapes from area symbols
            //foreach (PMSymbol areaSymbol in areaSymbols)
            //{
            //    AreaShape areaShape = areaSymbol.GetAreaShape(colorRef);
            //    shapes.Add(areaShape);
            //}

            //// get shapes from single symbols
            //foreach (PUSymbol singleSymbol in singleSymbols)
            //{
            //    Collection<DAIShape> singleShapes = singleSymbol.GetShapes(colorRef);
            //    foreach (DAIShape shape in singleShapes)
            //    {
            //        shapes.Add(shape);
            //    }
            //}

            foreach (S52BaseSymbol symbol in symbols)
            {
                if (symbol is PUSymbol)
                {
                    Collection<DAIShape> singleShapes = (symbol as PUSymbol).GetShapes(colorRef);
                    foreach (DAIShape shape in singleShapes)
                    {
                        shapes.Add(shape);
                    }
                }
                else if (symbol is PMSymbol)
                {
                    AreaShape areaShape = (symbol as PMSymbol).GetAreaShape(colorRef);
                    shapes.Add(areaShape);
                }
            }



            return shapes;
        }

        private PMSymbol GetAreaSymbol(Collection<string> symbolValues, 
            ref int penUpIndex, 
            int drawingIndex, 
            int ringStartIndex,
            int ringEndIndex)
        {
            PMSymbol areaSymbol = new PMSymbol();

            // set fill pattern symbol
            int fillIndex = IndexOfSymbolValue(symbolValues, ringStartIndex + 1, symbolValues.Count, "FP", "EP");
            areaSymbol.FillPatternSymbol = new PFSymbol(symbolValues[fillIndex]);
            AreaShapeFillPattern fillPattern = areaSymbol.FillPatternSymbol.GetFillPattern();

            // set transparency symbol
            if (fillPattern == AreaShapeFillPattern.Fill)
            {
                int transIndex = LastIndexOfSymbolValue(symbolValues, 0, ringStartIndex, "ST");
                if (transIndex == -1)
                {
                    areaSymbol.TransparencySymbol = new STSymbol("ST0");
                }
                else
                {
                    areaSymbol.TransparencySymbol = new STSymbol(symbolValues[transIndex]);
                }
            }
            else
            {
                areaSymbol.TransparencySymbol = new STSymbol();
            }

            // set ring symbols
            int outerRingEndIndex = IndexOfSymbolValue(symbolValues, ringStartIndex + 1, fillIndex - 1, "PM1");
            if (outerRingEndIndex == -1)
            {
                areaSymbol.OuterRingSymbols = GetRingSymbols(symbolValues, fillPattern, ref penUpIndex, drawingIndex, ringEndIndex);
            }
            else
            {
                areaSymbol.OuterRingSymbols = GetRingSymbols(symbolValues, fillPattern, ref penUpIndex, drawingIndex, outerRingEndIndex);
                drawingIndex = IndexOfSymbolValue(symbolValues, penUpIndex + 1, symbolValues.Count - 1, "PD", "CI", "AA");
                areaSymbol.InnerRingSymbols = GetRingSymbols(symbolValues, fillPattern, ref penUpIndex, drawingIndex, ringEndIndex);
            }

            return areaSymbol;
        }

        private Collection<PUSymbol> GetRingSymbols(Collection<string> symbolValues, 
            AreaShapeFillPattern fillPattern,
            ref int penUpIndex, 
            int drawingIndex, 
            int ringEndIndex)
        {
            Collection<PUSymbol> singleSymbols = new Collection<PUSymbol>();

            while (drawingIndex < ringEndIndex)
            {
                if (fillPattern == AreaShapeFillPattern.OutLine)
                {
                    PUSymbol symbol = GetSingleSymbolWithWidth(symbolValues, penUpIndex, ref drawingIndex);
                    singleSymbols.Add(symbol);
                }
                else
                {
                    PUSymbol symbol = GetSingleSymbolWithoutWidth(symbolValues, penUpIndex, ref drawingIndex);
                    singleSymbols.Add(symbol);
                }

                penUpIndex = IndexOfSymbolValue(symbolValues, drawingIndex + 1, symbolValues.Count - 1, "PU");

                if (penUpIndex == -1)
                {
                    break;
                }

                drawingIndex = IndexOfSymbolValue(symbolValues, penUpIndex, symbolValues.Count - 1, "PD", "AA", "CI");
            }

            return singleSymbols;
        }


        private PUSymbol GetSingleSymbolWithWidth(Collection<string> symbolValues, int penUpIndex, ref int drawingIndex)
        {
            PUSymbol singleSymbol = new PUSymbol();
            singleSymbol.Value = symbolValues[penUpIndex];
            singleSymbol.DrawingSymbols = GetDrawingSymbols(symbolValues, ref drawingIndex);

            int colorIndex = LastIndexOfSymbolValue(symbolValues, 0, drawingIndex - 1, "SP");
            int widthIndex = LastIndexOfSymbolValue(symbolValues, 0, drawingIndex - 1, "SW");

            // set color
            if (colorIndex == -1)
            {
                singleSymbol.ColorSymbol = new SPSymbol();
            }
            else
            {
                singleSymbol.ColorSymbol = new SPSymbol(symbolValues[colorIndex]);
            }

            // set width
            if (widthIndex == -1)
            {
                singleSymbol.WidthSymbol = new SWSymbol();
            }
            else
            {
                singleSymbol.WidthSymbol = new SWSymbol(symbolValues[widthIndex]);
            }

            return singleSymbol;
        }

        private PUSymbol GetSingleSymbolWithoutWidth(Collection<string> symbolValues, int penUpIndex, ref int drawingIndex)
        {
            PUSymbol singleSymbol = new PUSymbol();
            singleSymbol.Value = symbolValues[penUpIndex];
            singleSymbol.DrawingSymbols = GetDrawingSymbols(symbolValues, ref drawingIndex);
            singleSymbol.WidthSymbol = new SWSymbol();

            int colorIndex = LastIndexOfSymbolValue(symbolValues, 0, drawingIndex - 1, "SP");

            // set color
            if (colorIndex == -1)
            {
                singleSymbol.ColorSymbol = new SPSymbol();
            }
            else
            {
                singleSymbol.ColorSymbol = new SPSymbol(symbolValues[colorIndex]);
            }

            return singleSymbol;
        }

        private Collection<S52BaseSymbol> GetDrawingSymbols(Collection<string> symbolValues, ref int drawingIndex)
        {
            Collection<S52BaseSymbol> drawingSymbols = new Collection<S52BaseSymbol>();

            // get drawing symbols
            for (int i = drawingIndex; i < symbolValues.Count; i++)
            {
                if (symbolValues[i].StartsWith("PD"))
                {
                    drawingSymbols.Add(new PDSymbol(symbolValues[i]));
                }
                else if (symbolValues[i].StartsWith("CI"))
                {
                    drawingSymbols.Add(new CISymbol(symbolValues[i]));
                }
                else if (symbolValues[i].StartsWith("AA"))
                {
                    drawingSymbols.Add(new AASymbol(symbolValues[i]));
                }
                else
                {
                    drawingIndex = i - 1;
                    break;
                }
            }

            return drawingSymbols;
        }

        private int IndexOfSymbolValue(Collection<string> symbolValues, int startIndex, int endIndex, params string[] indicates)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = 0; j < indicates.Length; j++)
                {
                    if (symbolValues[i].StartsWith(indicates[j]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private int LastIndexOfSymbolValue(Collection<string> symbolValues, int startIndex, int endIndex, params string[] indicates)
        {
            for (int i = endIndex; i >= startIndex; i--)
            {
                for (int j = 0; j < indicates.Length; j++)
                {
                    if (symbolValues[i].StartsWith(indicates[j]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
