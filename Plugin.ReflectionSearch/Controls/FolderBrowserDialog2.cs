using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
#if WPF
using System.Windows;
using System.Windows.Interop;
#endif

namespace Plugin.ReflectionSearch.Controls
{
	class FolderBrowserDialog2
	{
		public virtual String ResultPath { get; protected set; }
		public virtual String ResultName { get; protected set; }
		public virtual String InputPath { get; set; }
		public virtual Boolean ForceFileSystem { get; set; }
		public virtual String Title { get; set; }
		public virtual String OkButtonLabel { get; set; }
		public virtual String FileNameLabel { get; set; }

		protected virtual Int32 SetOptions(Int32 options)
		{
			if(ForceFileSystem)
				options |= (Int32)FOS.FORCEFILESYSTEM;
			return options;
		}

#if WPF
		public bool? ShowDialog(Window owner = null, bool throwOnError = false)
		{
			owner ??= Application.Current.MainWindow;
			return ShowDialog(owner != null ? new WindowInteropHelper(owner).Handle : IntPtr.Zero, throwOnError);
		}
#endif

		// for all .NET
		public virtual Boolean? ShowDialog(IntPtr owner, Boolean throwOnError = false)
		{
			var dialog = (IFileOpenDialog)new FileOpenDialog();
			if(!String.IsNullOrEmpty(this.InputPath))
			{
				if(CheckHr(SHCreateItemFromParsingName(this.InputPath, null, typeof(IShellItem).GUID, out var item), throwOnError) != 0)
					return null;

				dialog.SetFolder(item);
			}

			var options = FOS.PICKFOLDERS;
			options = (FOS)SetOptions((Int32)options);
			dialog.SetOptions(options);

			if(this.Title != null)
				dialog.SetTitle(this.Title);

			if(this.OkButtonLabel != null)
				dialog.SetOkButtonLabel(this.OkButtonLabel);

			if(this.FileNameLabel != null)
				dialog.SetFileName(this.FileNameLabel);

			if(owner == IntPtr.Zero)
			{
				owner = Process.GetCurrentProcess().MainWindowHandle;
				if(owner == IntPtr.Zero)
					owner = GetDesktopWindow();
			}

			var hr = dialog.Show(owner);
			if(hr == ERROR_CANCELLED)
				return null;

			if(CheckHr(hr, throwOnError) != 0)
				return null;

			if(CheckHr(dialog.GetResult(out var result), throwOnError) != 0)
				return null;

			if(CheckHr(result.GetDisplayName(SIGDN.DESKTOPABSOLUTEPARSING, out var path), throwOnError) != 0)
				return null;

			this.ResultPath = path;

			if(CheckHr(result.GetDisplayName(SIGDN.DESKTOPABSOLUTEEDITING, out path), false) == 0)
				this.ResultName = path;
			return true;
		}

		private static Int32 CheckHr(Int32 hr, Boolean throwOnError)
		{
			if(hr != 0 && throwOnError)
				Marshal.ThrowExceptionForHR(hr);
			return hr;
		}

		[DllImport("shell32")]
		private static extern Int32 SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] String pszPath, IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);

		[DllImport("user32")]
		private static extern IntPtr GetDesktopWindow();

		private const Int32 ERROR_CANCELLED = unchecked((Int32)0x800704C7);

		[ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")] // CLSID_FileOpenDialog
		private class FileOpenDialog
		{
		}

		[ComImport, Guid("42f85136-db7e-439c-85f1-e4075d135fc8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IFileOpenDialog
		{
			[PreserveSig] Int32 Show(IntPtr parent); // IModalWindow
			[PreserveSig] Int32 SetFileTypes();  // not fully defined
			[PreserveSig] Int32 SetFileTypeIndex(Int32 iFileType);
			[PreserveSig] Int32 GetFileTypeIndex(out Int32 piFileType);
			[PreserveSig] Int32 Advise(); // not fully defined
			[PreserveSig] Int32 Unadvise();
			[PreserveSig] Int32 SetOptions(FOS fos);
			[PreserveSig] Int32 GetOptions(out FOS pfos);
			[PreserveSig] Int32 SetDefaultFolder(IShellItem psi);
			[PreserveSig] Int32 SetFolder(IShellItem psi);
			[PreserveSig] Int32 GetFolder(out IShellItem ppsi);
			[PreserveSig] Int32 GetCurrentSelection(out IShellItem ppsi);
			[PreserveSig] Int32 SetFileName([MarshalAs(UnmanagedType.LPWStr)] String pszName);
			[PreserveSig] Int32 GetFileName([MarshalAs(UnmanagedType.LPWStr)] out String pszName);
			[PreserveSig] Int32 SetTitle([MarshalAs(UnmanagedType.LPWStr)] String pszTitle);
			[PreserveSig] Int32 SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] String pszText);
			[PreserveSig] Int32 SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] String pszLabel);
			[PreserveSig] Int32 GetResult(out IShellItem ppsi);
			[PreserveSig] Int32 AddPlace(IShellItem psi, Int32 alignment);
			[PreserveSig] Int32 SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] String pszDefaultExtension);
			[PreserveSig] Int32 Close(Int32 hr);
			[PreserveSig] Int32 SetClientGuid();  // not fully defined
			[PreserveSig] Int32 ClearClientData();
			[PreserveSig] Int32 SetFilter([MarshalAs(UnmanagedType.IUnknown)] Object pFilter);
			[PreserveSig] Int32 GetResults([MarshalAs(UnmanagedType.IUnknown)] out Object ppenum);
			[PreserveSig] Int32 GetSelectedItems([MarshalAs(UnmanagedType.IUnknown)] out Object ppsai);
		}

		[ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellItem
		{
			[PreserveSig] Int32 BindToHandler(); // not fully defined
			[PreserveSig] Int32 GetParent(); // not fully defined
			[PreserveSig] Int32 GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out String ppszName);
			[PreserveSig] Int32 GetAttributes();  // not fully defined
			[PreserveSig] Int32 Compare();  // not fully defined
		}

		private enum SIGDN : UInt32
		{
			DESKTOPABSOLUTEEDITING = 0x8004c000,
			DESKTOPABSOLUTEPARSING = 0x80028000,
			FILESYSPATH = 0x80058000,
			NORMALDISPLAY = 0,
			PARENTRELATIVE = 0x80080001,
			PARENTRELATIVEEDITING = 0x80031001,
			PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
			PARENTRELATIVEPARSING = 0x80018001,
			URL = 0x80068000
		}

		[Flags]
		private enum FOS
		{
			OVERWRITEPROMPT = 0x2,
			STRICTFILETYPES = 0x4,
			NOCHANGEDIR = 0x8,
			PICKFOLDERS = 0x20,
			FORCEFILESYSTEM = 0x40,
			ALLNONSTORAGEITEMS = 0x80,
			NOVALIDATE = 0x100,
			ALLOWMULTISELECT = 0x200,
			PATHMUSTEXIST = 0x800,
			FILEMUSTEXIST = 0x1000,
			CREATEPROMPT = 0x2000,
			SHAREAWARE = 0x4000,
			NOREADONLYRETURN = 0x8000,
			NOTESTFILECREATE = 0x10000,
			HIDEMRUPLACES = 0x20000,
			HIDEPINNEDPLACES = 0x40000,
			NODEREFERENCELINKS = 0x100000,
			OKBUTTONNEEDSINTERACTION = 0x200000,
			DONTADDTORECENT = 0x2000000,
			FORCESHOWHIDDEN = 0x10000000,
			DEFAULTNOMINIMODE = 0x20000000,
			FORCEPREVIEWPANEON = 0x40000000,
			SUPPORTSTREAMABLEITEMS = unchecked((Int32)0x80000000)
		}
	}
}