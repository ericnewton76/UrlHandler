using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Common
{
	using UrlHandler.Core;

	/// <summary>
	/// Represents a relative uri
	/// </summary>
	public class RelativeUri
	{
		public RelativeUri(Func<UrlHandlerBase> urlhandler, string relativePath)
			: this(relativePath)
		{
			//urlhandler can be null.
			this._urlhandlerFunc = urlhandler;
		}
		public RelativeUri(Func<UrlHandlerBase> urlhandler, RelativeUri baseuri, string relativePath)
			: this(baseuri, relativePath)
		{
			//urlhandler can be null.
			this._urlhandlerFunc = urlhandler;
		}
		public RelativeUri(RelativeUri baseuri, string relativePath)
			: this(relativePath)
		{
			if(baseuri == null) throw new ArgumentNullException("baseuri");
			this._baseUri = baseuri;
		}
		public RelativeUri(string relativePath)
		{
			if(relativePath == null) throw new ArgumentNullException("relativePath");
			this._relativePath = relativePath;
		}

		Func<IUrlHandler> _urlhandlerFunc;
		protected IUrlHandler GetUrlHandler()
		{
			if(_urlhandlerFunc == null)
				return null;
			else
				return _urlhandlerFunc();
		}

		RelativeUri _baseUri;
		string _relativePath;

		/// <summary>
		/// The RelativePath included with this instance.
		/// </summary>
		public string RelativePath { get { return _relativePath; } }

		public static implicit operator string(RelativeUri relativeUri)
		{
			if(relativeUri == null) return "";
			return relativeUri.ToString();
		}

		protected virtual string _FullyQualified(string scheme = null, bool? checkFullyQualified = false, bool? forceFullyQualified = false, bool? forRedirect = false)
		{
			string absoluteuri = GetAbsoluteUri();

			var urlhandler = GetUrlHandler();

			bool checkingFullyQualified = false;

			if(checkFullyQualified == true || forRedirect == true)
				checkingFullyQualified = true;

			if(checkingFullyQualified)
			{
				if(urlhandler == null)
					throw new InvalidOperationException("UrlHandler is missing, unable to deliver Fully Qualified url.");
				else
				{
					forceFullyQualified = urlhandler.NeedsFullyQualified();
				}
			}

			if(forRedirect.GetValueOrDefault() == true || forceFullyQualified.GetValueOrDefault() == true)
			{
				absoluteuri = urlhandler.FullyQualified(absoluteuri);
			}

			return absoluteuri;
		}

		/// <summary>
		/// Returns a string representation of the RelativeUri using default handling.  Note that if used in a Response.Redirect statement, you must use ForRedirect() to guarantee that a schemeless url will be avoided.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			//BEST:return _FullyQualified(checkFullyQualified: true);
			string absoluteuri = GetAbsoluteUri();

			var urlhandler = GetUrlHandler();
			if(urlhandler != null && urlhandler.NeedsFullyQualified())
			{
				absoluteuri = urlhandler.FullyQualified(absoluteuri);
			}

			return absoluteuri;
		}

		/// <summary>
		/// Returns FullyQualified uri, using the desired default scheme when not specified directly.  NOTE: May return the double slash scheme which is not usable in Response.Redirect or Http Location header usage.  For this, use ForRedirect() method.
		/// </summary>
		/// <param name="scheme">optional, directly specify the scheme to use, http or https or "" for schemeless</param>
		/// <returns></returns>
		public string FullyQualified(string scheme = null)
		{
			//BEST:return _FullyQualified(scheme, forceFullyQualified: true);
			string absoluteuri = GetAbsoluteUri();

			var urlHandler = GetUrlHandler();
			if(urlHandler != null)
			{
				absoluteuri = urlHandler.FullyQualified(absoluteuri, scheme);
			}

			return absoluteuri;
		}

		/// <summary>
		/// Used for redirects, where it will guarantee the scheme is appropriate for use in Response.Redirect or Http Location header.  Utilized instead of FullyQualified() in order to prevent a schemeless url from being returned.
		/// </summary>
		/// <param name="scheme">optional, directly specify the scheme to use, http or https.  if string.empty or null then it will attempt to use most appropriate scheme.</param>
		/// <returns></returns>
		public string ForRedirect(string scheme = null)
		{
			//BEST:return _FullyQualified(scheme, forceFullyQualified: true, forRedirect: true);
			string absoluteuri = GetAbsoluteUri();

			var urlHandler = GetUrlHandler();
			if(urlHandler != null)
			{
				if(string.IsNullOrEmpty(scheme)) scheme = urlHandler.DefaultScheme;
				if(string.IsNullOrEmpty(scheme))
				{
					throw new InvalidOperationException("Specified scheme is empty, and UrlHandler's DefaultScheme is empty.  Must specify http or https in this case.");
				}
				absoluteuri = urlHandler.FullyQualified(absoluteuri, scheme);
			}

			return absoluteuri;
		}

		public virtual string GetAbsoluteUri()
		{
			string absoluteuri;
			if(_baseUri == null)
				absoluteuri = _relativePath;
			else
			{
				string baseuristring = _baseUri.ToString();
				if(_relativePath.StartsWith("/") == false && string.IsNullOrEmpty(baseuristring) == false && baseuristring.EndsWith("/") == false)
					absoluteuri = baseuristring + "/" + _relativePath;
				else
					absoluteuri = baseuristring + _relativePath;
			}

			return absoluteuri;
		}


	}
}
