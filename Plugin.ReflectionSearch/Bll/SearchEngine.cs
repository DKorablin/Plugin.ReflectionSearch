using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Plugin.ReflectionSearch.Bll
{
	internal class SearchEngine
	{
		private Object _target;
		private readonly Dictionary<String, SearchFilter> _filters;

		public SearchEngine(String path, SearchFilter filter)
		{
			if(String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			Dictionary<String, SearchFilter> filters = new Dictionary<String, SearchFilter>
			{
				{ path, filter }
			};

			this._filters = filters;
		}

		public SearchEngine(Dictionary<String, SearchFilter> filters)
		{
			_ = filters ?? throw new ArgumentNullException(nameof(filters));
			if(filters.Count == 0) throw new InvalidOperationException("None filters specified");

			this._filters = filters;
		}

		public Boolean StartSearch(Object target)
		{
			_ = target ?? throw new ArgumentNullException(nameof(target));

			this._target = target;
			return this.SearchRecursive(String.Empty, this._target.GetType(), this._target);
		}

		private Boolean SearchRecursive(String path, Type type, Object obj)
		{
			if(obj == null)
				return false;
			try
			{
				Type realType = type.GetRealType();
				foreach(MemberInfo member in realType.GetSearchableMembers())
				{
					String childPath;
					if(member.MemberType == MemberTypes.Method)
					{
						MethodInfo method = (MethodInfo)member;
						ParameterInfo[] prms = method.GetParameters();
						childPath = prms.Length == 1//TODO: Получение значения по енумам
							? this.GetFullKey(path, method.GetMemberType(), method.Name + "(" + prms[0].Name + ")")
							: this.GetFullKey(path, method.GetMemberType(), method.Name + "()");
					} else
						childPath = this.GetFullKey(path, member.GetMemberType(), member.Name);

					if(this.SearchObject(childPath, obj, member))//Searching
						return true;
					else if(this.SearchDeeper(childPath))
						foreach(Object o in this.ExtractObject(obj, member))
							//if(this.SearchRecursive(childPath, member.GetMemberType(), o))
							if(o != null && this.SearchRecursive(childPath, o.GetType().GetMemberType(), o))
								return true;
				}
				return false;
			} finally
			{
				if(!String.IsNullOrEmpty(path))
				{//Корень диспозить не надо
					IDisposable idisp = obj as IDisposable;
					idisp?.Dispose();
				}
			}
		}

		private Boolean SearchObject(String path, Object obj, MemberInfo member)
		{
			if(this._filters.TryGetValue(path, out SearchFilter filter))
			{
				foreach(Object o in this.ExtractObject(obj, member))
				{
					if(o == null && filter.Value == null)
						return true;

					if(o is IComparable cmp2)
					{
						Int32 result = cmp2.CompareTo(filter.Value);
						switch(filter.Sign)
						{
						case Sign.Less:
							return result == -1;
						case Sign.LessOrEquals:
							return result == -1 || result == 0;
						case Sign.More:
							return result == 1;
						case Sign.MoreOrEquals:
							return result == 1 || result == 0;
						case Sign.Equals:
							return result == 0;
						case Sign.NotEquals:
							return result != 0;
						default:
							throw new NotImplementedException(filter.Sign.ToString());
						}
					}

					switch(filter.Sign)
					{
					case Sign.Equals:
					case Sign.NotEquals:
						Boolean result;
						if(filter.Value.Equals(o))
							result = true;
						else if(Object.Equals(filter.Value, o))
							result = true;
						else if(filter.Value is String && !(o is String) && o.GetType().GetToStringMethod() != null)
						{
							MethodInfo toString = o.GetType().GetToStringMethod();
							Object valToStr = toString.Invoke(o, new Object[] { });
							result = String.Equals(filter.Value, valToStr);
						} else
							throw new NotImplementedException(filter.Value.ToString());
						return filter.Sign == Sign.Equals ? result : !result;
					case Sign.Contains:
						if(filter.Value is String fStr)
						{
							if(o is String strO && strO.Contains(fStr))
								return true;
							else
							{
								MethodInfo toString = o.GetType().GetToStringMethod();
								if(toString != null)
								{
									String valToStr = (String)toString.Invoke(o, new Object[] { });
									return valToStr?.Length >= fStr.Length && valToStr.ToLowerInvariant().Contains(fStr.ToLowerInvariant());
								}
							}
						}
						throw new NotImplementedException(filter.Value.ToString());
					default:
						if(Object.Equals(filter.Value, o))
							return true;
						break;
					}
				}
			}
			return false;
		}

		private IEnumerable ExtractObject(Object target, MemberInfo member)
		{
			if(target is IEnumerator enumerator)
			{
				while(enumerator.MoveNext())
					foreach(Object resultArr in this.ExtractObject(enumerator.Current, member))
						yield return resultArr;
				yield break;
			}

			Type type = target.GetType();
			BindingFlags getFlag;
			Array args = Array.CreateInstance(typeof(Object), 1);
			switch(member.MemberType)
			{
				case MemberTypes.Field:
					getFlag = BindingFlags.GetField;
					break;
				case MemberTypes.Property:
					getFlag = BindingFlags.GetProperty;
					break;
				case MemberTypes.Method:
					getFlag = BindingFlags.InvokeMethod;
					MethodInfo method = (MethodInfo)member;
					ParameterInfo[] prms = method.GetParameters();
					if(prms.Length == 1)//TODO: Получение значения по енумам
						args = Enum.GetValues(prms[0].ParameterType);
					break;
				default:
					throw new NotImplementedException(member.MemberType.ToString());
			}
			foreach(Object arg in args)
			{
				Object[] paramValue = null;
				if(arg != null)
					paramValue = new Object[] { arg };

				Object result = type.InvokeMember(member.Name, getFlag, null, target, paramValue);
				if(result != null && result.GetType()!=typeof(String))
				{//Чтобы итерация не пошла по символам
					if(result is IEnumerable enumerable)//SortedList!=Array
						foreach(Object resultArr in enumerable)
							yield return resultArr;
					enumerator = result as IEnumerator;
					if(enumerator != null)
					{
						while(enumerator.MoveNext())
							yield return enumerator.Current;
						yield break;
					}
				}
				yield return result;
			}
		}

		private String GetFullKey(String path,Type type, String name)
			=> String.IsNullOrEmpty(path)
				? type.GetMemberName(name)
				: path + "." + type.GetMemberName(name);

		private Boolean SearchDeeper(String path)
		{
			foreach(String key in this._filters.Keys)
				if(key.StartsWith(path))
					return true;
			return false;
		}
	}
}