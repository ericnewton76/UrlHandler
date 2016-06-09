# UrlHandler

![Nuget UrlHander](https://img.shields.io/nuget/v/UrlHandler.svg)

UrlHandler deals with inter-web applications that may/may not need FQDN.  Useful for dealing with both large solutions with several web application and the small project with some convenient url handling support.

Easily installable via Nuget:
```
Install-Package UrlHandler
```

The base domain for each environment is configured in web.config or app.config, and are then able to be transformed via XDT transformations for your build environments.  

```xml
<configuration>
  <appSettings>
    <add key="Urls.ContentWeb:Domain" value="http://contentweb.someweb.com" />
    <add key="Urls.BackendWeb:Domain" value="http://backendweb.webhost.com" />
    <add key="Urls:ThisEnvironment" value="ContentWeb" />
  </appSettings>
</configuration>
```

There is also the concept of "this environment" which allows for emitting relative urls within the correct environment.  Meaning, when in ContentWeb, no need to emit the full "http://contentweb.someweb.com" but if this code is run in BackendWeb, then the FQDN will be emitted.  A useful feature is leaving ThisEnvironment blank, which will force all the Uris to be emitted with FQDN.

## Usage



This method of Urls is intended to be low-touch when used in Razor or ASPX:

```cshtml
<nav>
  <div><a href="@Urls.ContentWeb.Home">Home</a></div>
  <div><a href="@Urls.ContentWeb.ContactUs">Contact Us</a></div>
  <div><a href="@Urls.ContentWeb.GetUri("Some/Random-Page")">Some Random Page</a></div>
</nav>
```

And also within code:

```cs
public class HomeController : Controller
{
  public ActionResult Index()
  {
    var model = new {
      HomePageUrl = Urls.ContentWeb.Home,
      SomeTextForModel = "Some Text",
      SomeRandomPageUrl = Urls.ContentWeb.GetUri("Some/Random-Page")
    };
    
    return View(model);
  }
}
```
