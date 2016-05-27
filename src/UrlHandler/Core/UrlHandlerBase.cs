using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlHandler.Common;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace UrlHandler.Core
{
	public abstract class UrlHandlerBase : IUrlHandler
	{

		protected UrlHandlerBase()
		{
			string typename = GetType().Name;

			//sometimes components need to postfix Urls to prevent namespace collision problems with the actual base namespace of the project.
			if(typename.EndsWith("Urls"))
				typename = typename.Substring(0, typename.Length - 4);

			Initialize(typename);
		}

		protected void Initialize(string typename)
		{
			string appSettingKey = "Urls." + typename + ":Domain";
			_WebDomain = ConfigurationManager.AppSettings["Urls." + typename + ":Domain"];
			if(string.IsNullOrEmpty(_WebDomain))
			{
#if NLOG
				Log.Warn("Urls.TxWebDomain is empty.");
#endif
			}
			else
			{
				if(_WebDomain.StartsWith("http") || _WebDomain.StartsWith("https"))
				{
					Uri txweb = new Uri(_WebDomain + "/");

					if(txweb.Port == 80)
						_WebDomain = txweb.Host;
					else
						_WebDomain = txweb.Host + ":" + txweb.Port;

					_DefaultScheme = txweb.Scheme;
				}
			}

			string thisEnvironment = ConfigurationManager.AppSettings["Urls:ThisEnvironment"];
			this._NeedsFullyQualified = (string.Compare(thisEnvironment, typename, true) != 0);
		}

		private string _WebDomain;
		private string _DefaultScheme;
		private bool _NeedsFullyQualified;

		/// <summary>
		/// Gets the Default Scheme being used for the current instance.   To change this, its recommended to assign the create a new instance and assign it to the Urls.Set method.
		/// </summary>
		public string DefaultScheme
		{
			get { return _DefaultScheme; }
		}

		/// <summary>
		/// Gets the Domain name for TxWeb
		/// </summary>
		public string WebDomain { get { return _WebDomain; } }

		/// <summary>
		/// Combines the pathAndQuery with the qualified domain to return a fully qualified url with optional scheme.  Defaults to // 
		/// </summary>
		/// <param name="pathAndQuery"></param>
		/// <param name="scheme"></param>
		/// <returns></returns>
		public virtual string FullyQualified(string pathAndQuery, string scheme = null)
		{
			if(scheme == null) scheme = _DefaultScheme;

			if(pathAndQuery.StartsWith("/") == false)
			{
				pathAndQuery = "/" + pathAndQuery;
			}

			if(scheme == null)
			{
				return "//" + _WebDomain + pathAndQuery;
			}
			else
			{
				return scheme + "://" + _WebDomain + pathAndQuery;
			}

		}

		/// <summary>
		/// Returns determines a value based on the current environment, specified within AppSettings["Urls:ThisEnvironment"]
		/// </summary>
		/// <returns></returns>
		public virtual bool NeedsFullyQualified()
		{
			return _NeedsFullyQualified;
		}
		public void setNeedsFullyQualified(bool value)
		{
			this._NeedsFullyQualified = value;
		}

		/// <summary>
		/// Gets a RelativeUri that will adjust for environment.
		/// </summary>
		/// <param name="relativePath"></param>
		/// <returns></returns>
		public abstract RelativeUri GetUri(string relativePath);

	}
}
