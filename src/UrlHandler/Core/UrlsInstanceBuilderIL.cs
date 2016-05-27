using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Core
{
	using System.Reflection;
	using System.Reflection.Emit;

	public class UrlsInstanceBuilderIL
	{

		public static dynamic CreateInstance(Type urlsType)
		{
			Type instanceType = Type.GetType("UrlHandler.UrlsInstance");
			if(instanceType == null)
			{
				instanceType = CreateUrlsInstance(urlsType);

			}
			dynamic urlsInstance = Activator.CreateInstance(instanceType);

			foreach(var propertyInfo in GetUrlHandlerBaseProperties(urlsType))
			{
				urlsInstance[propertyInfo.Name] = propertyInfo.GetMethod.Invoke(null, S_EmptyObjectArray);
			}

			return urlsInstance;
		}

		private static object[] S_EmptyObjectArray = new object[] { };
		private static Type[] S_EmptyTypeArray = new Type[] { };

		private static IEnumerable<System.Reflection.PropertyInfo> GetUrlHandlerBaseProperties(Type urlsType)
		{
			var propertyInfoArray = urlsType.GetProperties().Where(_ => _.GetMethod.ReturnType.IsAssignableFrom(typeof(UrlHandlerBase))).ToArray();
			return propertyInfoArray;
		}

		private static Type CreateUrlsInstanceIL(Type urlsType)
		{
			AssemblyName assemblyName = new AssemblyName();
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("UrlsInstanceModule");

			TypeBuilder typeBuilder = moduleBuilder.DefineType("UrlHandler.UrlsInstance", TypeAttributes.Public);

			typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

			//Add properties
			var propertyInfoArray = GetUrlHandlerBaseProperties(urlsType);

			foreach(var propertyInfo in propertyInfoArray)
			{
				var propertyGetMethod = propertyInfo.GetGetMethod();
				PropertyBuilder newProp =
					typeBuilder.DefineProperty(
						propertyInfo.Name,
						PropertyAttributes.None,
						propertyGetMethod.ReturnType,
						S_EmptyTypeArray);

				//ILGenerator ilGen = 

				// Create IL code for the method

			}
			// ...

			// Create the type itself
			Type newType = typeBuilder.CreateType();

			return newType;
		}

	}
}
