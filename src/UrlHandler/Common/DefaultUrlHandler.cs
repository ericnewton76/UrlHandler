using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Common
{
	public class DefaultUrlHandler : UrlHandler.Core.UrlHandlerBase
	{
		public DefaultUrlHandler() : base() { }

		public override RelativeUri GetUri(string relativePath)
		{
			return new RelativeUri(() => this, relativePath);
		}
	}
}
