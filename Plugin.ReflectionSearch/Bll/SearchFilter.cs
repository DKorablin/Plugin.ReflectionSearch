using System;
using Plugin.ReflectionSearch.Properties;

namespace Plugin.ReflectionSearch.Bll
{
	// Можно попробовать вывести весь PEInfo в виде дерева в
	// TreeView+Form чтобы для каждого элемента можно было указать условия поиска.
	// При поиске получить то-же самое дерево, но с применением поиска к каждому элементу объекта.
	internal struct SearchFilter
	{
		public Object Value { get; set; }

		public Sign? Sign { get; set; }

		public SearchFilter(Sign? sign, Object value)
		{
			this.Sign = sign;
			this.Value = value;
		}

		public SearchFilter(Sign? sign, Object value, Type target)
		{
			this.Sign = sign;
			this.Value = target.ConvertToType(value);
		}

		public String AsString()
		{
			String value = this.Value == null ? Resources.NullString : this.Value.ToString();
			String sign;
			if(this.Sign.HasValue)
			{
				switch(this.Sign.Value)
				{
				case Bll.Sign.Contains:
					sign = "contains";
					break;
				case Bll.Sign.Equals:
					sign = "==";
					break;
				case Bll.Sign.Less:
					sign = "<";
					break;
				case Bll.Sign.LessOrEquals:
					sign = "<=";
					break;
				case Bll.Sign.More:
					sign = ">";
					break;
				case Bll.Sign.MoreOrEquals:
					sign = ">=";
					break;
				case Bll.Sign.NotEquals:
					sign = "!=";
					break;
				default:
					throw new NotImplementedException();
				}
				return sign + " " + value;
			} else
				return "= " + value;
		}
	}
}