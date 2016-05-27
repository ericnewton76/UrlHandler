using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace UrlsSample
{
	class Program
	{
		static void Main(string[] args)
		{
			//Urls.Initialize()

			Console.WriteLine("ThisEnvironment={0}", Urls.ThisEnvironment);

			Urls.ContentWeb.WebDomain = "http://localhost:8080/";

			Console.WriteLine("\nIndividual Properties:\n----------------------");
			Console.WriteLine("Urls.ContentWeb.Home = {0}", Urls.ContentWeb.Home);
			Console.WriteLine("Urls.ContentWeb.Home = {0}", Urls.ContentWeb.ContactUs);
			Console.WriteLine("Urls.ContentWeb.Home = {0}", Urls.ContentWeb.GetUri("/Some/Random-Page"));


			Console.WriteLine("\nFixUrlLinks:\n----------------------");
			string urls_Fix_Links = @"

Lo and behold, the main app at {Urls:MainUrl}.

Then there's a separate server that has Content: {Urls:ContentWeb}

Last, there's a Backend server that deals with accounts, status, and various other stuff: {Urls:BackendWeb}


";

			string fixedLinks = Urls.FixUrlsLink(urls_Fix_Links);

			Console.WriteLine(fixedLinks);

			//wait for 5 seconds if debugger attached
			if(System.Diagnostics.Debugger.IsAttached)
			{
				if(WaitForKeypress(10).Key == ConsoleKey.P) Console.ReadKey();
			}
		}

		static ConsoleKeyInfo WaitForKeypress(int waitTimeSeconds = 10)
		{
			var original = DateTime.Now;
			var newTime = original;

			var remainingWaitTime = waitTimeSeconds;
			var lastWaitTime = waitTimeSeconds.ToString();
			var keyRead = false;
			Console.Write("Waiting for key press or expiring in {0}", waitTimeSeconds);
			do
			{
				keyRead = Console.KeyAvailable;
				if(!keyRead)
				{
					newTime = DateTime.Now;
					remainingWaitTime = waitTimeSeconds - (int)(newTime - original).TotalSeconds;
					var newWaitTime = remainingWaitTime.ToString();
					if(newWaitTime != lastWaitTime)
					{
						var backSpaces = new string('\b', lastWaitTime.Length);
						var spaces = new string(' ', lastWaitTime.Length);
						Console.Write(backSpaces + spaces + backSpaces);
						lastWaitTime = newWaitTime;
						Console.Write(lastWaitTime);
						System.Threading.Thread.Sleep(500);
					}
				}
			} while(remainingWaitTime > 0 && !keyRead);

			if(Console.KeyAvailable == false)
				return new ConsoleKeyInfo();
			else
				return Console.ReadKey();
		}

	}


}
