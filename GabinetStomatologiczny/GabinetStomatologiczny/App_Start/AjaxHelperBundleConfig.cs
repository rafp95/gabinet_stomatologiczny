using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(GabinetStomatologiczny.App_Start.AjaxHelperBundleConfig), "RegisterBundles")]

namespace GabinetStomatologiczny.App_Start
{
	public class AjaxHelperBundleConfig
	{
		public static void RegisterBundles()
		{
			BundleTable.Bundles.Add(new ScriptBundle("~/bundles/ajaxhelper").Include("~/Scripts/jquery.ajaxhelper.js"));
		}
	}
}