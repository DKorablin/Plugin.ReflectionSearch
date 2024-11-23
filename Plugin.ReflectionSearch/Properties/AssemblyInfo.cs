using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("836fc84e-5488-4946-b7a0-f997eb7962c4")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://dkorablin.ru/project/Default.aspx?File=110")]
#else

[assembly: AssemblyTitle("Plugin.ReflectionSearch")]
[assembly: AssemblyDescription("Plugin to search in objects using reflection")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.ReflectionSearch")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2023")]
#endif