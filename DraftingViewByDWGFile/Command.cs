using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;

namespace DraftingViewByDWGFile
{
	[TransactionAttribute(TransactionMode.Manual)]
	[RegenerationAttribute(RegenerationOption.Manual)]
	public class Command : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData,
			ref string messege,
			ElementSet elements)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;
			try
			{
				var filePath = string.Empty;
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					openFileDialog.Filter = "dwg files (*.dwg)|*.dwg";
					openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						filePath = openFileDialog.FileName;

						using (TransactionGroup tg = new TransactionGroup(uidoc.Document, "DraftingViewByDWGFile"))
						{
							tg.Start();

							ViewDrafting view = ViewCreator.CreateViewDrafting(uidoc.Document, System.IO.Path.GetFileName(filePath));
							ElementId linkId = DWGLink.Insert(view, filePath);
							CreateGeometryObject.CreateOnViewDrafting(view as ViewDrafting, linkId);
							DWGLink.Delete(uidoc.Document, linkId);

							uidoc.ActiveView = view;

							tg.Assimilate();
						}
					}
				}				
				return Result.Succeeded;
			}
			catch (Autodesk.Revit.Exceptions.OperationCanceledException)
			{
				return Result.Cancelled;
			}
			catch (Exception ex)
			{
				messege = ex.Message;
				return Result.Failed;
			}
		}
	}
}