﻿using System;

namespace Plugin.ReflectionSearch.Bll
{
	/// <summary>Comparison signs</summary>
	internal enum Sign
	{
		/// <summary>==</summary>
		Equals,
		/// <summary>!=</summary>
		NotEquals,
		/// <summary>&gt;</summary>
		More,
		/// <summary>&gt;=</summary>
		MoreOrEquals,
		/// <summary>&lt;</summary>
		Less,
		/// <summary>&lt;=</summary>
		LessOrEquals,
		/// <summary>Contains</summary>
		Contains,
	}
}