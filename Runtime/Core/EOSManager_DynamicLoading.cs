/*
* Copyright (c) 2021 PlayEveryWare
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR && !UNITY_IOS && !UNITY_STANDALONE_OSX
#define USE_EOS_GFX_PLUGIN_NATIVE_RENDER
#endif

#if UNITY_EDITOR
#define EOS_DYNAMIC_BINDINGS
#endif


using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System;

#if !EOS_DISABLE
using Epic.OnlineServices.Platform;
using Epic.OnlineServices;
#endif

namespace PlayEveryWare.EpicOnlineServices
{
    public partial class EOSManager
    {
        /// <summary>
        /// Singleton design pattern implementation for <c>EOSManager</c>.
        /// </summary>
        public partial class EOSSingleton
        {
#if !EOS_DISABLE
            static private PlatformInterface s_eosPlatformInterface;

            public const string EOSBinaryName = Epic.OnlineServices.Config.LibraryName;

#if USE_EOS_GFX_PLUGIN_NATIVE_RENDER
            public const string GfxPluginNativeRenderPath =
#if UNITY_STANDALONE_OSX
                "GfxPluginNativeRender-macOS";
#elif UNITY_STANDALONE_WIN
#if UNITY_64
                "GfxPluginNativeRender-x64";
#else
                "GfxPluginNativeRender-x86";
#endif // UNITY_64
#else
                #error Unknown platform
                "GfxPluginNativeRender-unknown";
#endif

            [DllImport(GfxPluginNativeRenderPath,CallingConvention = CallingConvention.StdCall)]
            static extern IntPtr EOS_GetPlatformInterface();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            delegate void PrintDelegateType(string str);
            [DllImport(GfxPluginNativeRenderPath,CallingConvention = CallingConvention.StdCall)]
            static extern void global_log_flush_with_function(IntPtr ptr);
#endif


            //-------------------------------------------------------------------------
            public PlatformInterface GetEOSPlatformInterface()
            {
#if USE_EOS_GFX_PLUGIN_NATIVE_RENDER
                if (s_eosPlatformInterface == null && s_state != EOSState.Shutdown)
                {
                    // Try to log any messages stored when starting up the Plugin.
                    IntPtr logErrorFunctionPointer = Marshal.GetFunctionPointerForDelegate(new PrintDelegateType(SimplePrintStringCallback));
                    SimplePrintStringCallback("Start of Early EOS LOG:");
                    global_log_flush_with_function(logErrorFunctionPointer);
                    SimplePrintStringCallback("End of Early EOS LOG");

                    if (EOS_GetPlatformInterface() == IntPtr.Zero)
                    {
                        throw new Exception("NULL EOS Platform returned by native code: issue probably occurred in GFX Plugin!");
                    }
                    SetEOSPlatformInterface(new Epic.OnlineServices.Platform.PlatformInterface(EOS_GetPlatformInterface()));
                }
#endif
                return s_eosPlatformInterface;

            }

            //-------------------------------------------------------------------------
            void SetEOSPlatformInterface(PlatformInterface platformInterface)
            {
                if (platformInterface != null)
                {
                    s_state = EOSState.Running;
                }
                s_eosPlatformInterface = platformInterface;
            }


            //-------------------------------------------------------------------------
            static private void AddAllAssembliesInCurrentDomain(List<Assembly> list)
            {
                var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in appAssemblies)
                {
                    //var assemblyBasename = Path.GetFileNameWithoutExtension(assembly.Location);

                    list.Add(assembly);
                }
            }

            //-------------------------------------------------------------------------
            // This currently only works on windows
            static private Dictionary<string, DLLHandle> LoadedDLLs = new Dictionary<string, DLLHandle>();
            static public DLLHandle LoadDynamicLibrary(string libraryName)
            {
                if(LoadedDLLs.ContainsKey(libraryName))
                {
                    print("Found existing handle for " + libraryName);
                    return LoadedDLLs[libraryName];
                }
                else
                {
                    var libraryHandle = DLLHandle.LoadDynamicLibrary(libraryName);
                    LoadedDLLs[libraryName] = libraryHandle;
                    return libraryHandle;
                }
            }

            //-------------------------------------------------------------------------
            // This doesn't do anything smart to make sure the delegates aren't being used, so 
            // be mindful to only do this at the very end of the app usage
            static public void UnloadAllLibraries()
            {
                foreach(var entry in LoadedDLLs)
                {
                    entry.Value.Dispose();
                }
                LoadedDLLs.Clear();
                LoadedDLLs = new Dictionary<string, DLLHandle>();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }

            //-------------------------------------------------------------------------
            static public void LoadDelegatesWithEOSBindingAPI()
            {
#if EOS_DYNAMIC_BINDINGS
                print($"Loading EOS binary {EOSBinaryName}");
                var eosLibraryHandle = LoadDynamicLibrary(EOSBinaryName);

                Epic.OnlineServices.Bindings.Hook<DLLHandle>(eosLibraryHandle, (DLLHandle handle, string functionName) => {
                // TODO: Add conditions for all flags (unless OSX is the only one that's weird?)
#if UNITY_EDITOR_OSX
                    return handle.LoadFunctionAsIntPtr(functionName.Trim('_'));
#else
                    return handle.LoadFunctionAsIntPtr(functionName);
#endif
                 });

//#if !UNITY_EDITOR
                EOSManagerPlatformSpecificsSingleton.Instance?.LoadDelegatesWithEOSBindingAPI();
//#endif
#endif
                }
            //-------------------------------------------------------------------------
            // Using runtime reflection, hook up things
            static public void LoadDelegatesWithReflection()
            {
                var selectedAssemblies = new List<Assembly>();
                var eosLibraryHandle = LoadDynamicLibrary(EOSBinaryName);
                selectedAssemblies.Add(typeof(PlatformInterface).Assembly);

                foreach(var assembly in selectedAssemblies)
                {
                    foreach(var clazz in assembly.GetTypes())
                    {
                        if (clazz.Namespace == null)
                        {
                            continue;
                        }
                        //print("Looking at this class " + clazz.FullName);
                        if(!clazz.Namespace.StartsWith("Epic"))
                        {
                            continue;
                        }

                        //TODO figure out if can actually modify binding at runtime between a method impl and the c function 
                        // so I don't have to use delegates. Or don't worry about it because maybe it doesn't impact perf that much?
                        //foreach (var method in clazz.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        //{
                        //    if(method.IsDefined(typeof(DllImportAttribute)))
                        //    {

                        //    }
                        //}

                        // This works due to an assumed naming convention in the EOS Library that was added for this plugin
                        foreach(var member in clazz.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            FieldInfo fieldInfo = clazz.GetField(member.Name);
                            if (fieldInfo == null)
                            {
                                continue;
                            }

                            if (fieldInfo.FieldType.Name.EndsWith("_delegate"))
                            {
                                var delegateType = fieldInfo.FieldType;
                                var functionNameAsString = member.Name.Replace("_delegate", "");
                                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(clazz, delegateType, functionNameAsString);
                            }
                        }
                    }
                }
            }

            //-------------------------------------------------------------------------
            // At the moment, this isn't supported on Consoles.
            static private void LoadDelegatesByHand()
            {
#if UNITY_EDITOR && !UNITY_SWITCH && USE_MANUAL_METHOD_LOADING
                var eosLibraryHandle = LoadDynamicLibrary(Epic.OnlineServices.Config.BinaryName);

                var eosPlatformType = typeof(PlatformInterface);

                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Initialize_delegate), "EOS_Initialize");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Platform_Create_delegate), "EOS_Platform_Create");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Platform_Tick_delegate), "EOS_Platform_Tick");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Platform_GetAuthInterface_delegate), "EOS_Platform_GetAuthInterface");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Platform_Release_delegate), "EOS_Platform_Release");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Shutdown_delegate), "EOS_Shutdown");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosPlatformType, typeof(PlatformInterface.EOS_Platform_GetConnectInterface_delegate), "EOS_Platform_GetConnectInterface");

                var eosHelper = typeof(Helper);
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosHelper, typeof(Helper.EOS_EResult_IsOperationComplete_delegate), "EOS_EResult_IsOperationComplete");


                var eosLoggingType = typeof(LoggingInterface);
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosLoggingType, typeof(LoggingInterface.EOS_Logging_SetCallback_delegate), "EOS_Logging_SetCallback");

                var eosAuthType = typeof(Epic.OnlineServices.Auth.AuthInterface);
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosAuthType, typeof(Epic.OnlineServices.Auth.AuthInterface.EOS_Auth_Login_delegate), "EOS_Auth_Login");
                eosLibraryHandle.ConfigureFromLibraryDelegateFieldOnClassWithFunctionName(eosAuthType, typeof(Epic.OnlineServices.Auth.AuthInterface.EOS_Auth_Logout_delegate), "EOS_Auth_Logout");
#endif
            }

            static bool loadEOSWithReflection
#if UNITY_EDITOR
                = true;
#elif UNITY_SWITCH
                = true;
#else
                = false;
#endif
            //-------------------------------------------------------------------------
            // At the moment this only works on Windows.
            static private void ForceUnloadEOSLibrary()
            {
#if EOS_DYNAMIC_BINDINGS
                Epic.OnlineServices.Bindings.Unhook();
#endif

#if UNITY_EDITOR
                IntPtr existingHandle;
                int timeout = 50;
                do
                {
                    existingHandle = SystemDynamicLibrary.GetHandleForModule(EOSBinaryName);
                    if (existingHandle != IntPtr.Zero)
                    {
                        GC.WaitForPendingFinalizers();
                        SystemDynamicLibrary.UnloadLibraryInEditor(existingHandle);
                    }
                    timeout--;
                } while (IntPtr.Zero != existingHandle && timeout > 0);

                if (IntPtr.Zero != existingHandle)
                {
                    UnityEngine.Debug.LogWarning("Free Library { " + EOSBinaryName + " }:Timeout");
                }
#endif
            }

            //-------------------------------------------------------------------------
            static public void LoadEOSLibraries()
            {
                if (loadEOSWithReflection)
                {
#if EOS_DYNAMIC_BINDINGS
                    LoadDelegatesWithEOSBindingAPI();
#else
                    LoadDelegatesWithReflection();
#endif
                    return;
                }

#if UNITY_EDITOR
                LoadDelegatesByHand();
#endif
            }
#endif
        }
    }
}
