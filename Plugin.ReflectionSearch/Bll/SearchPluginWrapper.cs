using System;
using System.Reflection;
using SAL.Flatbed;

namespace Plugin.ReflectionSearch.Bll
{
	internal class SearchPluginWrapper
	{
		private readonly IPluginDescription _plugin;

		public SearchPluginWrapper(IPluginDescription plugin)
			=> this._plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));

		public static Boolean IsConsumable(IPluginDescription plugin)
		{
			IPluginMethodInfo method = plugin.Type.GetMember<IPluginMethodInfo>(Constant.PluginMethods.GetEntityType);
			return method != null;
		}

		public Type GetEntityType()
		{
			IPluginMethodInfo method = this._plugin.Type.GetMember<IPluginMethodInfo>(Constant.PluginMethods.GetEntityType);
			return (Type)method.Invoke();
		}

		public Object CreateEntityInstance(String filePath)
		{
			try
			{
				IPluginMethodInfo method = this._plugin.Type.GetMember<IPluginMethodInfo>(Constant.PluginMethods.CreateEntityInstance);
				return method.Invoke(filePath);
			}catch(TargetInvocationException exc)
			{
				if(exc.InnerException != null)
					throw exc.InnerException;
				else throw;
			}
		}

		public String[] GetSearchObjects(String folderPath)
		{
			try
			{
				IPluginMethodInfo method = this._plugin.Type.GetMember<IPluginMethodInfo>(Constant.PluginMethods.GetSearchObjects);
				return (String[])method.Invoke(folderPath);
			}catch(TargetInvocationException exc)
			{
				if(exc.InnerException != null)
					throw exc.InnerException;
				else throw;
			}
		}
	}
}