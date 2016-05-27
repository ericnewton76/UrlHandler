using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace UrlHandler.Core
{
	using System.Reflection;
	using UrlHandler.Common;

	public static class UrlsCore
	{
		static UrlsCore()
		{
			S_ThisEnvironment = ConfigurationManager.AppSettings["Urls:ThisEnvironment"];
		}
		private static string S_ThisEnvironment;
		public static string ThisEnvironment { get { return S_ThisEnvironment; } }

		public static bool HasUrlsLink(string p)
		{
			if(p == null) return false;
			return S_UrlsRegex.IsMatch(p);
		}

		public static string FixUrlsLink(UrlHandlerBase b, string p)
		{
			var m = S_UrlsRegex.Match(p);

			Type foundUrlsType = b.GetType();

			PropertyInfo prop = foundUrlsType.GetProperty(m.Groups[1].Value, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);

			if(prop != null)
			{
				UrlHandlerBase urlhandlerbase = (UrlHandlerBase)prop.GetGetMethod().Invoke(null, new object[] { });

				p = p.Replace(m.Groups[0].Value, "");

				return urlhandlerbase.FullyQualified(p);
			}

			return p;
		}

		private static System.Text.RegularExpressions.Regex S_UrlsRegex = new System.Text.RegularExpressions.Regex(@"\{Urls.([@A-Z0-9_]+)\}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
	}
}
