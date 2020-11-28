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
	static class ViewCreator
	{
		public static ViewDrafting CreateViewDrafting(Document doc, string name)
		{
			ViewFamilyType vd = new FilteredElementCollector(doc)
			.OfClass(typeof(ViewFamilyType))
				.Cast<ViewFamilyType>()
			.FirstOrDefault(q => q.ViewFamily == ViewFamily.Drafting);

			ViewDrafting draftView;
			using (Transaction t = new Transaction(doc, "CreateViewDrafting"))
			{
				t.Start();
				draftView = ViewDrafting.Create(doc, vd.Id);
				draftView.Name = name;
				t.Commit();
			}
			if(draftView != null)
			{
				return draftView;
			}
			else { throw (new Exception("Не удалось вставить DWG файл")); }				
		}
	}
}