using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Common
{
	/// <summary>
	/// Allows for deferring determination of the uri.
	/// </summary>
	public class RelativeUriDeferred : RelativeUri
	{
		public RelativeUriDeferred(Func<string> deferringFunc)
			: base("")
		{
			_deferringFunc = deferringFunc;
		}
		Func<string> _deferringFunc;

		public override string ToString()
		{
			string value = _deferringFunc();
			return value;
		}
	}
}
