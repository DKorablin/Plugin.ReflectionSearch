using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Plugin.ReflectionSearch.Bll
{
	internal class XmlReflectionReader
	{
		private static XmlReflectionReader _instance;
		private readonly Dictionary<Assembly, XmlDocument> _documents = new Dictionary<Assembly, XmlDocument>();
		private readonly Dictionary<String, String> _documentationCache = new Dictionary<String, String>();
		private readonly Object _lock = new Object();

		public static XmlReflectionReader Instance
		{
			get { return _instance ?? (_instance = new XmlReflectionReader()); }
		}

		private XmlReflectionReader()
		{
		}

		public XmlDocument LoadDocument(Assembly asm)
		{
			if(asm.GlobalAssemblyCache)
				return null;

			if(this._documents.TryGetValue(asm, out var document))
				return document;

			lock(_lock)
			{
				String path = GetXmlPath(asm.Location);
				if(!File.Exists(path))
				{
					path = GetXmlPath(new Uri(asm.CodeBase).LocalPath);
					if(!File.Exists(path))
						return null;
				}

				document = new XmlDocument();
				document.Load(path);
				this._documents.Add(asm, document);
				return document;
			}
		}

		public String FindDocumentation(MemberInfo type)
		{
			Assembly asm = type.Module.Assembly;
			if(asm.GlobalAssemblyCache)
				return null;

			String memberName = XmlReflectionReader.GetMemberName(type);
			String key = asm.GetName().Name + ">" + memberName;
			if(this._documentationCache.TryGetValue(key, out String result))
				return result;

			lock(_lock)
			{
				XmlDocument doc = this.LoadDocument(asm);
				if(doc == null)
					result = null;
				else
				{
					var navigator = doc.CreateNavigator();
					var node = navigator.SelectSingleNode(String.Format("/doc/members/member[@name=\"{0}\"]/summary", memberName));
					result = node == null
						? null
						: node.InnerXml.Trim().Replace("  ", " ");
				}
				this._documentationCache.Add(key, result);
				return result;
			}
		}

		private static String GetXmlPath(String assemblyLocation)
		{
			String path = Path.GetDirectoryName(assemblyLocation);
			String xmlFileName = Path.GetFileNameWithoutExtension(assemblyLocation) + ".xml";
			return Path.Combine(path, xmlFileName);
		}

		private static String GetMemberName(MemberInfo type)
		{
			Char prefix;
			switch(type.MemberType)
			{
			case MemberTypes.Field:
				prefix = 'F';
				break;
			case MemberTypes.Property:
				prefix = 'P';
				break;
			case MemberTypes.TypeInfo:
				Type elementType = ((Type)type).GetRealType();
				if(elementType == type)
					prefix = 'T';
				else//Array[MyType]
					return GetMemberName(elementType);
				break;
			case MemberTypes.Method:
				prefix = 'M';
				break;
			default: throw new NotImplementedException();
			}

			String fullName = type.DeclaringType == null
				? type.ToString()
				: type.DeclaringType.FullName;
			return prefix
				+ ":"
				+ fullName.Replace('+', '.')//Nested types declares as Namespace.Class+InnerClass.Field
				+ "."
				+ type.Name;
		}
	}
}