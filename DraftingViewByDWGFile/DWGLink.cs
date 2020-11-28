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
	static class DWGLink
	{
		public static ElementId Insert(View view, string fileNmae)
		{
			Document doc = view.Document;
			DWGImportOptions options = new DWGImportOptions();
			ElementId linkId;

			using (Transaction t = new Transaction(doc, "InsertDWGLink"))
			{
				t.Start();
				if (doc.Link(fileNmae, options, view, out linkId))
				{
					t.Commit();
				}
				else
				{
					t.RollBack();
					throw (new Exception("Не удалось вставить DWG файл"));
				}
			}
			return linkId;
		}
		public static void Delete(Document doc, ElementId id)
		{
			using (Transaction t = new Transaction(doc, "DeleteDWGLink"))
			{
				t.Start();
				doc.Delete(id);
				t.Commit();
			}
		}
	}
}