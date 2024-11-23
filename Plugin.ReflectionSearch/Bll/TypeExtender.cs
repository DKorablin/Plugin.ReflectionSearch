using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Plugin.ReflectionSearch.Search;

namespace Plugin.ReflectionSearch.Bll
{
	internal static class TypeExtender
	{
		public static IEnumerable<MemberInfo> GetSearchableMembers(this Type type)
		{
			foreach(MemberInfo member in type.GetMembers())
				if(type.IsMemberSearchable(member))
					yield return member;
		}

		private static Boolean IsMemberSearchable(this Type type, MemberInfo member)
		{
			if(member.DeclaringType != type && member.DeclaringType != type.BaseType)
				return false;//member.DeclaringType == type.BaseType Используется для отображения наследованных классов (Пример: StringHeap:StreamHeaderTyped<String>.Data). Но возможно появление бесконечных рекурсий

			switch(member.MemberType)
			{
			case MemberTypes.Property:
				return type.IsMemberSearchable((PropertyInfo)member);
			case MemberTypes.Method:
				return type.IsMemberSearchable((MethodInfo)member);
			case MemberTypes.Field:
				return type.IsMemberSearchable((FieldInfo)member);
			default:
				return false;
			}
		}

		private static Boolean IsMemberSearchable(this Type type, FieldInfo field)
			=> true;

		private static Boolean IsMemberSearchable(this Type type, PropertyInfo property)
			=> property.GetIndexParameters().Length == 0;

		private static Boolean IsMemberSearchable(this Type type, MethodInfo method)
		{
			Boolean result = method.ReturnType != typeof(void)
				&& !method.Name.StartsWith("get_")
				&& !method.Name.StartsWith("set_");
			if(result)
			{//TODO: Получение значения по енумам
				ParameterInfo[] prms = method.GetParameters();
				if(prms.Length == 1)
					if(!prms[0].ParameterType.IsEnum)
						return false;
			}
			return result;
		}

		public static Type GetMemberType(this MemberInfo member)
		{
			switch(member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			case MemberTypes.Property:
				return ((PropertyInfo)member).PropertyType;
			case MemberTypes.Method:
				return ((MethodInfo)member).ReturnType;
			case MemberTypes.TypeInfo:
			case MemberTypes.NestedType:
				return (Type)member;
			default:
				throw new NotImplementedException();
			}
		}

		public static String GetMemberName(this Type type, String originalName)
		{
			String result = originalName;
			if(type.IsGenericType)
				if(type.GetGenericTypeDefinition() == typeof(System.Nullable<>))
					result += "?";
				else
					result = "'1 " + result;
			if(type.BaseType == typeof(Array))
				result += "[]";
			return result;
		}

		public static Type GetRealType(this Type type)
		{
			if(type.IsGenericType)
			{
				Type genericType = type.GetGenericTypeDefinition();
				if(genericType == typeof(System.Nullable<>)
					|| genericType == typeof(System.Collections.Generic.IEnumerator<>)
					|| genericType == typeof(System.Collections.Generic.IEnumerable<>)
					/*|| genericType == typeof(System.Collections.Generic.SortedList<,>)*/)
					return type.GetGenericArguments()[0].GetRealType();
			}
			if(type.HasElementType)
				//if(type.BaseType == typeof(Array))//+Для out и ref параметров
				return type.GetElementType().GetRealType();
			return type;
		}

		public static ControlType GetFilterControlType(this Type type)
		{
			ControlType result = ControlType.None;

			Type checkType = type.GetRealType();

			if(checkType.IsEnum)
				result = ControlType.Enum;
			else if(checkType.IsBclType())
			{
				if(checkType == typeof(Boolean))
					result = ControlType.Boolean;
				else if(type == typeof(Byte)
					|| type == typeof(SByte)
					|| type == typeof(Int16)
					|| type == typeof(UInt16)
					|| type == typeof(Int32)
					|| type == typeof(UInt32)
					|| type == typeof(Int64)
					|| type == typeof(UInt64)
					|| type == typeof(Single)
					|| type == typeof(Decimal)
					|| type == typeof(Double))
					result = ControlType.Integer;
				else if(type == typeof(DateTime))
					result = ControlType.Date;
				else if(type == typeof(String))
					result = ControlType.String;
				else if(checkType.GetToStringMethod() != null)
					result = ControlType.String;
			}

			return result;
		}

		/// <summary>This type from Basic Class Library</summary>
		/// <param name="type">Type to check</param>
		/// <returns>Type from BCL</returns>
		public static Boolean IsBclType(this Type type)
		{
			switch(type.Assembly.GetName().Name)
			{
			case "mscorlib":
			case "System.Private.CoreLib":
				return true;
			default:
				return false;
			}
		}

		public static MethodInfo GetToStringMethod(this Type type)
			=> IsBclType(type)//Only check in BCL members, because non BCL members will be expanded
				? type.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly, null, new Type[] { }, null)
				: null;

		/// <summary>
		/// We need to specify exact result property type.
		/// Otherwise when we compare 2 objects we will not get equals
		/// </summary>
		/// <param name="type">Type what we needed</param>
		/// <param name="source">Object of some type</param>
		/// <returns>Converted object or object we can convert to</returns>
		public static Object ConvertToType(this Type type, Object source)
		{
			if(source == null)
				return source;
			else if(source.GetType() == type)
				return source;
			else
			{
				try
				{
					return ((IConvertible)source).ToType(type, CultureInfo.CurrentCulture);
				} catch(InvalidCastException)
				{
					if(type.GetToStringMethod() != null)
						return source;
					else
						throw;
				}
			}
		}
	}
}