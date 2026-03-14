using System;
using System.Runtime.InteropServices;
#if NET48_OR_GREATER
using NativeLibraryLoader;
#endif
namespace Cuemon.Assets
{
    public class UnmanagedDisposable : FinalizeDisposable
    {
        internal IntPtr _handle = IntPtr.Zero;
        internal IntPtr _libHandle = IntPtr.Zero;

        public delegate bool CloseHandle(IntPtr hObject);

        public delegate IntPtr CreateFileDelegate(string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        public delegate IntPtr PtSname(int fd);

#if NET48_OR_GREATER
        internal NativeLibrary _nativeLibrary;
#endif

        public UnmanagedDisposable()
        {
#if NET10_0_OR_GREATER
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (NativeLibrary.TryLoad("kernel32.dll", GetType().Assembly, DllImportSearchPath.System32, out _libHandle))
                {
                    if (NativeLibrary.TryGetExport(_libHandle, "CreateFileW", out var functionHandle))
                    {
                        var createFileFunc = Marshal.GetDelegateForFunctionPointer<CreateFileDelegate>(functionHandle);
                        _handle = createFileFunc(@"C:\TestFile.txt",
                            0x80000000, //access read-only
                            1, //share-read
                            IntPtr.Zero,
                            3, //open existing
                            0,
                            IntPtr.Zero);
                    }
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (NativeLibrary.TryLoad("libc.so.6", GetType().Assembly, DllImportSearchPath.SafeDirectories, out _libHandle))
                {
                    _handle = _libHandle; // i don't know of any native methods on unix
                }
            }
#else
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _nativeLibrary = new NativeLibrary("kernel32.dll");
                _libHandle = _nativeLibrary.Handle;
                var functionHandle = _nativeLibrary.LoadFunction("CreateFileW");
                var createFileFunc = Marshal.GetDelegateForFunctionPointer<CreateFileDelegate>(functionHandle);
                _handle = createFileFunc(@"C:\TestFile.txt",
                    0x80000000, //access read-only
                    1, //share-read
                    IntPtr.Zero,
                    3, //open existing
                    0,
                    IntPtr.Zero);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _nativeLibrary = new NativeLibrary("libc.so.6");
                _libHandle = _nativeLibrary.Handle;
                _handle = _libHandle; // i don't know of any native methods on unix
            }
#endif
        }

        protected override void OnDisposeManagedResources()
        {

        }

        protected override void OnDisposeUnmanagedResources()
        {
#if NET10_0_OR_GREATER
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (_handle != IntPtr.Zero)
                {
                    if (NativeLibrary.TryGetExport(_libHandle, "CloseHandle", out var closeHandle))
                    {
                        var closeHandleAction = Marshal.GetDelegateForFunctionPointer<CloseHandle>(closeHandle);
                        closeHandleAction(_handle);
                    }
                }
                NativeLibrary.Free(_libHandle);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                NativeLibrary.Free(_libHandle);
            }
#else
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (_handle != IntPtr.Zero)
                {
                    var closeHandle = _nativeLibrary.LoadFunction("CloseHandle");
                    var closeHandleAction = Marshal.GetDelegateForFunctionPointer<CloseHandle>(closeHandle);
                    closeHandleAction(_handle);
                }
                _nativeLibrary.Dispose();
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                _nativeLibrary.Dispose();
            }
#endif
        }
    }
}