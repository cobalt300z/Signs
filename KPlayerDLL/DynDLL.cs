using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Utility;

namespace KPlayerDLL
{
    internal class DynDLL : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ConnectSendersByIP(ipAddressRange kPlayerIP);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool GetPortBrightness(ushort senderId, byte portIndex, out brightnessObject portBrightness);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool SetPortBrightness(ushort senderId, byte portIndex, brightnessObject portBrightness);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool GetReceiverDiagnostics_v1(ushort senderId, byte portIndex, ushort receiverIndex, out receiverStatus_v1 pReceiverStatus);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ConnectReceivers();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool GetModuleDiagnostics_v1(ushort senderId, byte portIndex, ushort receiverIndex, out moduleStatus_v1 pModuleStatus);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool SetLumAttenuation(ushort coefficient);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ushort GetLumAttenuation();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool IsDiagnosticsEnabled();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EnableDiagnostics(bool on);


        private IntPtr pDll;
        bool disposed;
        private Dictionary<string, IntPtr> pointerDict;
        public ConnectSendersByIP ConnectSenders;
        public ConnectReceivers ConnectRecv;
        public GetPortBrightness GetBrightness;
        public SetPortBrightness SetBrightness;
        public GetModuleDiagnostics_v1 GetModuleData;
        public GetReceiverDiagnostics_v1 GetReceiverData;
        public SetLumAttenuation SetAttenuation;
        public GetLumAttenuation GetAttenuation;
        public IsDiagnosticsEnabled getDiag;
        public EnableDiagnostics enableDiag;


        public DynDLL(string laneNumber)
        {
            pointerDict = new Dictionary<string, IntPtr>();
            pointerDict.Add("ConnectSendersByIP", IntPtr.Zero);
            pointerDict.Add("GetPortBrightness", IntPtr.Zero);
            pointerDict.Add("SetPortBrightness", IntPtr.Zero);
            pointerDict.Add("GetReceiverDiagnostics_v1", IntPtr.Zero);
            pointerDict.Add("ConnectReceivers", IntPtr.Zero);
            pointerDict.Add("GetModuleDiagnostics_v1", IntPtr.Zero);
            pointerDict.Add("SetLumAttenuation", IntPtr.Zero);
            pointerDict.Add("GetLumAttenuation", IntPtr.Zero);
            pointerDict.Add("IsDiagnosticsEnabled", IntPtr.Zero);
            pointerDict.Add("EnableDiagnostics", IntPtr.Zero);

            pDll =  NativeMethods.LoadLibrary(@"DLL\LED-LANE" + laneNumber + @".dll");

            if (pDll == IntPtr.Zero)
            {
                Logger.Instance.Log("Cannot find DLL {0}", laneNumber.ToString());
                throw new NullReferenceException(string.Format("Cannot find DLL for Lane {0}", laneNumber));
            }

            if (!LoadPointers())
            {
                Logger.Instance.Log("Could not load Pointers. Exiting...");
                throw new NullReferenceException(string.Format("Could not load Pointers for Lane {0}", laneNumber));
            }
            else
            {
                Logger.Instance.Log("Pointers Loaded for Lane {0}", laneNumber);
            }

        }

        private bool LoadPointers()
        {
            foreach (var item in pointerDict.ToList())
            {
                pointerDict[item.Key] = NativeMethods.GetProcAddress(pDll, item.Key);

                if (pointerDict[item.Key] == IntPtr.Zero)
                {
                    Logger.Instance.Log("Incorrect Method call {0}", item.Key);
                    throw new NullReferenceException("Could not locate Method");
                }
            }

            ConnectSenders = (ConnectSendersByIP)Marshal.GetDelegateForFunctionPointer(
                pointerDict["ConnectSendersByIP"], typeof(ConnectSendersByIP));

            ConnectRecv = (ConnectReceivers)Marshal.GetDelegateForFunctionPointer(
                pointerDict["ConnectReceivers"], typeof(ConnectReceivers));

            GetBrightness = (GetPortBrightness)Marshal.GetDelegateForFunctionPointer(
                pointerDict["GetPortBrightness"], typeof(GetPortBrightness));

            SetBrightness = (SetPortBrightness)Marshal.GetDelegateForFunctionPointer(
                pointerDict["SetPortBrightness"], typeof(SetPortBrightness));

            GetModuleData = (GetModuleDiagnostics_v1)Marshal.GetDelegateForFunctionPointer(
                pointerDict["GetModuleDiagnostics_v1"], typeof(GetModuleDiagnostics_v1));

            GetReceiverData = (GetReceiverDiagnostics_v1)Marshal.GetDelegateForFunctionPointer(
                pointerDict["GetReceiverDiagnostics_v1"], typeof(GetReceiverDiagnostics_v1));

            SetAttenuation = (SetLumAttenuation)Marshal.GetDelegateForFunctionPointer(
                pointerDict["SetLumAttenuation"], typeof(SetLumAttenuation));

            GetAttenuation = (GetLumAttenuation)Marshal.GetDelegateForFunctionPointer(
                pointerDict["SetLumAttenuation"], typeof(GetLumAttenuation));

            getDiag = (IsDiagnosticsEnabled)Marshal.GetDelegateForFunctionPointer(
                pointerDict["IsDiagnosticsEnabled"], typeof(IsDiagnosticsEnabled));

            enableDiag = (EnableDiagnostics)Marshal.GetDelegateForFunctionPointer(
                pointerDict["EnableDiagnostics"], typeof(EnableDiagnostics));


            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    pointerDict = null;
                }
            }
            NativeMethods.FreeLibrary(pDll) ;
            pDll = IntPtr.Zero;
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}