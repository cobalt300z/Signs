using System;
using System.Linq;
using System.Runtime.InteropServices;


namespace KPlayerDLL
{

    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    //public static class LedSetup
    //{

    //    //public const string DllName = "LED_Setup_Interface.dll";

    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern byte GetNetSenderSearchCount();

    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void SetNetSenderSearchCount(byte searchCount);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool ConnectSenders();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern int ConnectSendersByIP(ipAddressRange sendersSearchRange);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool ClearSenders();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern ushort GetSenderId(byte senderIndex);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern ushort GetSendersCount();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool IsSenderConnected(ushort senderId);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern byte GetSenderSource(int senderId = -1);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern int SetSenderSource(byte source = 1, int senderId = -1);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool IsDVIConnected(ushort senderId);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetSenderInputResolution(ushort senderId, out region inputResolution);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool SetSenderResolution(ushort senderId, byte senderResolution);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool UpgradeSender(ushort senderId, string upgradeFileName);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool CreateReceiver(ushort senderId, byte senderPortIndex, ushort receiverCount);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool DeleteReceiver(ushort senderId, byte senderPortIndex);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void ClearReceiver();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern int ConnectReceivers();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //// 	public static extern bool IsSearchingReceiver();
    //    //public static extern void StopSearchReceiver();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool RestartReceivers(int senderId = -1);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    ////	public static extern int GetFoundReceiverCount();
    //    //public static extern int GetReceiversCount(ushort senderId, byte portIndex);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetPortBrightness(ushort senderId, byte portIndex, out brightnessObject portBrightness);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool SetPortBrightness(ushort senderId, byte portIndex, brightnessObject portBrightness);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetPortCaptureRegion(ushort senderId, byte portIndex, out region portRegion);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool SetPortCaptureRegion(ushort senderId, byte portIndex, region portRegion);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern ushort GetTCPPort();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void SetTCPPort(ushort wNetCommPort = 8399);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetSenderPortState(ushort wSenderId, byte portIndex);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool SetSenderPortState(ushort wSenderId, byte portIndex, bool bPortOn = true);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern ushort GetLumAttenuation();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool SetLumAttenuation(ushort coefficient);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern int SetDisplayState(bool on);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool IsDiagnosticsEnabled();
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern void EnableDiagnostics(bool on);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetSenderDiagnostics(ushort senderId, out senderStatus senderStatus);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetReceiverDiagnostics_v1(ushort senderId, byte portIndex, ushort receiverIndex, out receiverStatus_v1 pReceiverStatus);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetReceiverDiagnostics_v2(ushort senderId, byte portIndex, ushort receiverIndex, out receiverStatus_v2 pReceiverStatus);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetModuleDiagnostics_v1(ushort senderId, byte portIndex, ushort receiverIndex, out moduleStatus_v1 pModuleStatus);
    //    //[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    //    //public static extern bool GetModuleDiagnostics_v2(ushort receiverIndex, out moduleStatus_v2 pModuleStatus);

    //}

        public struct tagRecvModlInfo
        {
            /// <summary>
            /// Module address[BIT7-4] = Row address 0~15, [BIT3-0] = Column address 0~13
            /// </summary>
            public byte ByModuleAddr;

            /// <summary>
            /// Connection status[BIT2] = 0 B data line broke
            /// 　　				 1 good
            ///		　	[BIT1] = 0 G data line broke
            ///			  　　	 1 good
            ///			[BIT0] = 0 R data line broke
            ///					 1 good			
            /// </summary>
            public byte ByCableStatus;

            /// <summary>
            /// Module voltage (actual voltage = byModuleVoltage*32/1000.0)
            /// </summary>
            public byte ByModuleVoltage;

            /// <summary>
            /// Module temperature
            /// </summary>
            public sbyte SbyModuleTemperature;

            public short GetModlVoltAsShort()
            {
                return ByModuleVoltage;
            }

            public float GetModlVoltAsFloat()
            {
                return ByModuleVoltage;
            }

            public sbyte GetModlTemperature()
            {
                return SbyModuleTemperature;
            }

            public byte GetModlAddrRow()
            {
                return (byte)(ByModuleAddr & 0xF);

            }

            public byte GetModlAddrCol()
            {
                return (byte)(ByModuleAddr >> 4);
            }

            public bool IsCableOk()
            {
                return ((ByCableStatus & 0x1) != 0
                    && (ByCableStatus & 0x2) != 0
                    && (ByCableStatus & 0x4) != 0);
            }
        }
        public struct ipAddressRange
        {
            public byte addrSub1;
            public byte addrSub2;
            public byte addrSub3;
            public byte abbrSub4_Start;
            public byte abbrSub4_End;
        }
        public struct region
        {
            public ushort Left;
            public ushort Top;
            public ushort Width;
            public ushort Height;
            region(ushort left = 0, ushort top = 0, ushort width = 16, ushort height = 16)
            {
                Left = left;
                Top = top;
                Width = width;
                Height = height;
            }
        }
        public struct brightnessObject
        {
            public byte totalBrightness;
            public byte redChannel;
            public byte greenChannel;
            public byte blueChannel;
        }
        public struct senderStatus
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public string[] Ip;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public string[] Nickname;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public string[] MacAddress;
            public byte CpuMainVer;
            public byte CpuSubVer;
            public byte FpgaMainVer;
            public byte FpgaSubVer;
            public bool _enviBrightFlag;
            //brightness of the environment, the unit lux [(enviBrightValue.GetWord () * GetLumAttenuation()) / 100], GetLumAttenuation(): brightness attenuation coefficient
            public tagApiBigWord _enviBrightValue;
            public bool _enviTemperatureFlag;
            //environment temperature, unit ℃ (enviTemperatureValue.GetShort () / 10)
            public tagApiBigShort _enviTemperatureValue;
        }
        public struct receiverStatus_v2
        {
            // Data0x80-8F data line corresponds to the number of modules and the cable states. bit0 ~ 3: the number of modules; bit7: 1 represents both good and 0 means there is bad;
            public byte[] ByModuleNumberCableStatus;

            // Data90-AF module voltage (actual voltage = bwModuleVolt * 32 / 1000.0)
            public tagApiBigWord[] BwModuleVolt;

            // DataB0-CF module temperature. Low byte: data lines under the maximum temperature; high byte: lower average temperature data lines;
            public tagApiBigWord[] BwModuleTemperature;

            public int IsAllCableOk(byte byIndex)
            {
                return byIndex < (ByModuleNumberCableStatus.Count()) ? (ByModuleNumberCableStatus[byIndex] & 0x80) : 0;
            }
            public int GetModuleNumber(byte byIndex)
            {
                return byIndex < (ByModuleNumberCableStatus.Count()) ? (ByModuleNumberCableStatus[byIndex] & 0xF) : 0;
            }

            public int GetAvgVolt(byte byIndex)
            {
                return byIndex < (BwModuleVolt.Count()) ? ((byte)(BwModuleVolt[byIndex].Hi)) : 0;
            }

            public int GetLowVolt(byte byIndex)
            {
                return byIndex < (BwModuleVolt.Count()) ? ((byte)(BwModuleVolt[byIndex].Lo)) : 0;
            }

            public int GetAvgTemp(byte byIndex)
            {
                return byIndex < (BwModuleTemperature.Count()) ? ((Int16)(BwModuleTemperature[byIndex].Hi)) : 0;
            }
            public int GetHigTemp(byte byIndex)
            {
                return byIndex < (BwModuleVolt.Count()) ? ((Int16)(BwModuleTemperature[byIndex].Lo)) : 0;
            }
        }
        public struct moduleStatus_v1
        {
            public const int MaxReceiverHubPortAmnt = 16;
            public const int MaxRecvHubPortModlAmnt = 14;
            public bool BQueryResultOk;
            public ushort NRealModuleAmount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 232)]
            public tagRecvModlInfo[] StReceiverModlInfo;

            public bool GetReceiverModlInfo(byte byRowAddr/*0~15*/, byte byColAddr/*0~13*/, ref tagRecvModlInfo rReceiverModlInfo)
            {
                bool bResult = false;
                if (NRealModuleAmount > MaxReceiverHubPortAmnt * MaxRecvHubPortModlAmnt)
                {
                    NRealModuleAmount = MaxReceiverHubPortAmnt * MaxRecvHubPortModlAmnt;
                }
                for (int i = 0; i < NRealModuleAmount; i++)
                {
                    if ((StReceiverModlInfo[i].ByModuleAddr >> 4) == byRowAddr && (StReceiverModlInfo[i].ByModuleAddr & 0xF) == byColAddr)
                    {
                        rReceiverModlInfo = StReceiverModlInfo[i];

                        bResult = true;
                        break;
                    }
                }
                return bResult;
            }
        }
        public struct moduleStatus_v2
        {
            // Send quotes Picasso, Picasso sent the first quotation is 0, the second is 1, and so on.
            public byte BySenderIndex;
            // Send a card port index number, A port is 0, B port 1.
            public byte BySenderPortIndex;
            // Hub card port index number, the card is not used Hub is 0, the first port when using the Hub card is 0, the second is 1, and so on.
            public byte ByHubPortIndex;
        }
        public struct receiverStatus_v1
        {
            // Length of 128 bytes of data part
            // Data0-1: PCB version number, byPcbMain major version number, byPcbSub sub-version number.
            public byte byPcbMain;
            public byte byPcbSub;
            // Data2-3: CPU version, byCpuMain major version number, byCpuSub sub-version number.
            public byte byCpuMain;
            public byte byCpuSub;
            // 	// Data4-5: device ID
            // 	tagApiBigWord bwDeviceId;
            // Data4: Hub type: 　　0xA1: iic, 0xA2: QV2 UART, 0xA3: Hub for 5151
            public byte byHubType;
            // Data5: Hub Version: [BIT7-4]: cpu ver, [BIT3-0]: fpga ver					
            public byte byHubVersion;

            // Data6: upgrade code valid flag or Device Type
            public byte byFlagOrChip;
            // Data7: receiving card address mode
            public byte byAddressMode;
            // Data8-9 supply voltage (in mv)
            public tagApiBigWord bwPowerVolt;
            //	// Data0xA-B
            //	byte byReserved[2];
            // Data0xA-B receiver Temperature, unit ℃ (bnReceiverTemperature.GetShort () / 10.0f)
            public tagApiBigShort bnReceiverTemperature;

            // Data0xC ambient brightness flag 0: ambient brightness invalid, 1: Effective ambient brightness, 2: Brightness Overflow
            public byte byEnviBritFlag;
            // Data0xD-E brightness of the environment, equal to 0:00 invalid, the unit lux [(bwEnviBritValue.GetWord () * nBritCfct) / 100], nBritCfct: brightness attenuation coefficient
            public tagApiBigWord bwEnviBritValue;

            // Data0xF humidity cabinet
            public byte byHumidity;

            // Data0x10-0x11: FPGA version number, byFpgaMain major version, byFpgaSub child version.
            public byte byFpgaMain;
            public byte byFpgaSub;

            // Data0x12-0x13: cabinet width
            public tagApiBigWord bwWidth;
            // Data0x14-0x15: cabinet height
            public tagApiBigWord bwHeight;
            // Data0x16 some receiver properties [bit0: receiver work mode, 0:master mode, 1:slave mode],[bit1~7: reserve]
            public byte byReceiverProps;
            //Data0x17:
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] byReserved2;

            // Data0x18-19 cabinet temperature, unit ℃ (bnCabinetTemperature.GetShort () / 10.0f)
            public tagApiBigShort bnCabinetTemperature;
            // Data0x1A-1B module average temperature in ℃ (bnCabinetTemperature.GetShort () / 10.0f)
            public tagApiBigShort bnPaneAvgTemperature;
            // Data0x1C-1D
            public tagApiBigWord bwBadPackage;
            // Data0x1E-1F
            public tagApiBigWord bwPixelChipCount;
            // Data0x20-2F panel status, Bit1: temperature sensor module valid flag, 0: Invalid 1: Valid; Bit0: I2C memory modules valid flag, 0: Invalid 1: Valid
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] byPaneStates;
            // Data0x30-4f panel temperature, unit ℃ (bnCabinetTemperature.GetShort () / 10.0f)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public tagApiBigShort[] bnPaneTemeratures;
            // Data0x50-6f bad pixel detection points (16)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public tagApiBigWord[] bwBadPixelCount;
            // Data0x70-7F
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] byReserved4;

            // Memory modules effective?
            public int IsPaneValid(byte byIndex)
            {
                return byIndex < (byPaneStates.Count()) ? (byPaneStates[byIndex] & 0x01) : 0;
            }
            public int IsPaneTemperatureValid(byte byIndex)
            {
                return byIndex < (byPaneStates.Count()) ? (byPaneStates[byIndex] & 0x02) : 0;
            }



        }
        public struct tagApiBigWord
        {
            public byte Hi;

            public byte Lo;


            public ushort GetWord()
            {
                return (ushort)((Hi << 8) + Lo);
            }

            public float GetFloat()
            {
                return ((Hi << 8) + Lo);
            }

        }
        public struct tagApiBigShort
        {
            public byte Hi;

            public byte Lo;

            public float GetShort()
            {
                if ((Hi & 0x80) != 0)
                {
                    return -((((Hi & 0x7F) << 8) + Lo));
                }
                return ((Hi << 8) + Lo);

            }
        }

    
}







