using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Core
{
	public interface IUrlHandler
	{
		string WebDomain { get; }
		bool NeedsFullyQualified();
		string DefaultScheme { get; }
		string FullyQualified(string relativePath, string scheme = null);
	}
}
