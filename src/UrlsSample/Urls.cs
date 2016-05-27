using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlHandler.Common;

namespace UrlsSample
{
	public static partial class Urls
	{
		public static ContentWebUrls ContentWeb = new ContentWebUrls();

		public static BackendWebUrls BackendWeb = new BackendWebUrls();

		public static DefaultUrlHandler SimpleUrlsWeb = new DefaultUrlHandler();

		//public static RelativeUri AdfsBaseUrl = new Uri("http://mainweb.mycompany.com/");
	}


	public class ContentWebUrls : DefaultUrlHandler
	{
		public RelativeUri Home { get { return GetUri("/"); } }
		public RelativeUri ContactUs { get { return GetUri("/Contact-Us"); } }
	}

	public class BackendWebUrls : DefaultUrlHandler
	{
		public RelativeUri Login { get { return GetUri("/login"); } }
		public RelativeUri Account_Overview { get { return GetUri("/account/overview"); } }
	}

}
