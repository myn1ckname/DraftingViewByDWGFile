using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;

namespace DraftingViewByDWGFile
{

	class App : IExternalApplication
	{
		static readonly string ExecutingAssemblyPath = System.Reflection.Assembly
		.GetExecutingAssembly().Location;

		public Result OnStartup(UIControlledApplication app)
		{
			// Создание вкладки
			String tabName = "Egorov_Test";
			app.CreateRibbonTab(tabName);

			CreatePanel(app, tabName);

			return Result.Succeeded;
		}

		public Result OnShutdown(UIControlledApplication app)
		{
			return Result.Succeeded;
		}

		void CreatePanel(UIControlledApplication app, string tabName)
		{
			string v = "v: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// Создание панели
			RibbonPanel panel = app.CreateRibbonPanel(tabName, "DraftingView");

			// PushButtonData для кнопки
			PushButtonData wall = new PushButtonData("Create View", "Create View",
				ExecutingAssemblyPath, "DraftingViewByDWGFile.Command");
			wall.LargeImage = new System.Windows.Media.Imaging.BitmapImage
				(new Uri("pack://application:,,,/DraftingViewByDWGFile;component/img/icon32.png", UriKind.Absolute));
			wall.ToolTip = v;
			// Кнопка
			PushButton button = panel.AddItem(wall) as PushButton;
		}				
	}
}