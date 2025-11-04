using System;
using Plugin.ReflectionSearch.Properties;

namespace Plugin.ReflectionSearch.Bll
{
	// We can try displaying the entire PEInfo as a tree in
	// TreeView+Form so that you can specify search conditions for each element.
	// When searching, get the same tree, but apply the search to each element of the object.
	
	/// <summary>
	/// Represents a search filter that combines a comparison sign with a value.
	/// Used to define search criteria for filtering objects based on property values.
	/// </summary>
	[Serializable]
	public struct SearchFilter
	{
		/// <summary>Gets or sets the value to compare against.</summary>
		public Object Value { get; set; }

		/// <summary>Gets or sets the comparison sign to use for filtering.</summary>
		/// <remarks>If null, equals comparison is assumed.</remarks>
		public Sign? Sign { get; set; }

		/// <summary>Initializes a new instance of the <see cref="SearchFilter"/> struct.</summary>
		/// <param name="sign">The comparison sign to use for filtering.</param>
		/// <param name="value">The value to compare against.</param>
		public SearchFilter(Sign? sign, Object value)
		{
			this.Sign = sign;
			this.Value = value;
		}

		/// <summary>Initializes a new instance of the <see cref="SearchFilter"/> struct with type conversion.</summary>
		/// <param name="sign">The comparison sign to use for filtering.</param>
		/// <param name="value">The value to compare against.</param>
		/// <param name="target">The target type to convert the value to.</param>
		public SearchFilter(Sign? sign, Object value, Type target)
		{
			this.Sign = sign;
			this.Value = target.ConvertToType(value);
		}

		/// <summary>Converts the search filter to a human-readable string representation.</summary>
		/// <returns>
		/// A string in the format "[sign] [value]", where sign is the comparison operator
		/// (e.g., "==", "!=", "&lt;", "&gt;", "contains") and value is the filter value.
		/// If no sign is specified, returns "= [value]".
		/// </returns>
		public String AsString()
		{
			// Convert value to string, using a localized "null" representation if value is null
			String value = this.Value == null ? Resources.NullString : this.Value.ToString();
			String sign;
			if(this.Sign.HasValue)
			{
				// Map the Sign enum value to its string representation
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