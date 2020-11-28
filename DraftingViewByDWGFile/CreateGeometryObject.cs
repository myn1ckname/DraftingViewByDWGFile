using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;

namespace DraftingViewByDWGFile
{
	static class CreateGeometryObject
	{
		public static void CreateOnViewDrafting(ViewDrafting view, ElementId linkId)
		{
			Document doc = view.Document;

			ImportInstance instance = doc.GetElement(linkId) as ImportInstance;
			GeometryElement geometryElement = instance.get_Geometry(new Options());
			foreach (GeometryObject go in geometryElement)
			{
				GeometryInstance ginstance = go as GeometryInstance;
				if (null != ginstance)
				{
					CreatePrimitives(view, ginstance.SymbolGeometry);
				}
			}
		}
		static void CreatePrimitives(ViewDrafting view, GeometryElement geometryElement)
		{
			Document doc = view.Document;
			foreach (GeometryObject instObj in geometryElement)
			{
				using (Transaction t = new Transaction(doc, "CreatePrimitive"))
				{
					t.Start();
					if (instObj is Line)
					{
						doc.Create.NewDetailCurve(view, instObj as Line);
					}
					if (instObj is PolyLine)
					{
						IList<XYZ> coordinates = (instObj as PolyLine).GetCoordinates();
						CurveArray curveArray = new CurveArray();
						for (int i = 0; i < coordinates.Count - 1; i++)
						{
							curveArray.Append(Line.CreateBound(coordinates[i], coordinates[i + 1]));
						}
						doc.Create.NewDetailCurveArray(view, curveArray);
					}
					if (instObj is Arc)
					{
						doc.Create.NewDetailCurve(view, instObj as Arc);
					}
					t.Commit();
				}
				//TODO: Для блоков нужно разобраться с передачей координат
				//if (instObj is GeometryInstance)
				//{
				//	GeometryInstance ginstance = instObj as GeometryInstance;
				//	if (null != ginstance)
				//	{
				//		CreatePrimitives(view, ginstance.SymbolGeometry);
				//	}
				//}
			}
		}
	}
}