using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UrlHandler.Core
{
	using System.Text;
	using System.Reflection;
	using System.CodeDom;
	using Microsoft.CSharp;
	using System.CodeDom.Compiler;

	internal class UrlsInstanceBuilderCodeDom
	{
		Type _UrlsType;
		public UrlsInstanceBuilderCodeDom(Type urlsType)
		{
			this._UrlsType = urlsType;
		}

		public Type CreateUrlsInstance()
		{
			CodeCompileUnit ccu = CreateCodeCompileUnit();
			CodeTypeDeclaration urlsInstanceType = CreateUrlsInstanceType();
			ccu.Namespaces[0].Types.Add(urlsInstanceType);

			CompilerErrorCollection errors;
			Assembly urlsInstanceAssembly = BuildGeneratedCode(ccu, out errors);

			return urlsInstanceAssembly.GetType("UrlHandler.UrlsInstance");
		}

		private Assembly BuildGeneratedCode(CodeCompileUnit ccu, out CompilerErrorCollection errors)
		{
			CSharpCodeProvider codeProvider = new CSharpCodeProvider();

			CompilerParameters parameters = new CompilerParameters();
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add(_UrlsType.Assembly.FullName);
			parameters.GenerateInMemory = true;

			CompilerResults results = codeProvider.CompileAssemblyFromDom(parameters, ccu);
			errors = results.Errors;

			return results.CompiledAssembly;
		}

		private static CodeCompileUnit CreateCodeCompileUnit()
		{
			CodeCompileUnit unit = new CodeCompileUnit();
			//CodeAttributeArgument[] arguments = new CodeAttributeArgument[] 
			//{
			//	new CodeAttributeArgument(
			//		new CodePrimitiveExpression(assemblyAuthorName))//Create parameter for attribute 
			//	, new CodeAttributeArgument(
			//		new CodeSnippetExpression("DynamicCodeGeneration.CustomAttributes.GenerationMode.CodeDOM"));
			//}
			//CodeAttributeDeclaration assemblyLevelAttribute = new CodeAttributeDeclaration(
			//	new CodeTypeReference("DynamicCodeGeneration.CustomAttributes.AssemblyLevelAttribute"),
			//	arguments);//Create attribute to be added to assembly
			//unit.AssemblyCustomAttributes.Add(assemblyLevelAttribute);

			unit.ReferencedAssemblies.Add("System.dll");
			return unit;
		}

		private static CodeTypeDeclaration CreateType(Type urlsType)
		{
			CodeTypeDeclaration urlsInstanceTypeDecl = new CodeTypeDeclaration("UrlsInstance");

			urlsInstanceTypeDecl.Members.Add(
		}
	}
}

