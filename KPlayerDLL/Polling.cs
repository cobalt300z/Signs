using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace KPlayerDLL
{


    public class Polling
    {
        int senderCt;
        Signs signs;
        DynDLL LEDLane;
        Settings settings;
        int recCt;
        receiverStatus_v1[] rv1 = new receiverStatus_v1[2];
        
        
        
       


       
        #region constructor
        public Polling(DataRow lane)
        {

            senderCt = 0;
            signs = new Signs();
            signs.LaneID = lane["LaneID"].ToString();
            
            signs.SenderId = Convert.ToUInt16(lane["SenderID"].ToString());
            signs.SignID = Convert.ToInt16(lane["SignID"].ToString());
            signs.PollingState = Convert.ToBoolean(lane["PollingEnabled"].ToString());
            signs.BrightnessMode = Convert.ToInt16(lane["BrightnessModeID"].ToString());
            signs.CurrentBrightness = Convert.ToByte(lane["BrightnessLevel"].ToString());
            settings = new Settings(signs.SignID);
            LEDLane = new DynDLL(signs.LaneID);


        }
        #endregion

        #region Public Methods
        public void Connect(ipAddressRange kPlayerIP)
        {

            int retries = Convert.ToInt32(settings.GetSetting("ConnectRetries").ToString());
            //if (senderCt == 0)
            //{
            //    for (int i = 1; i <= retries; i++)
            //    {

            //        senderCt = LEDLane.ConnectSenders(iparr);

            //        if (senderCt == 0)
            //        {
            //            Logger.Instance.Log("Retrying...");

            //            Thread.Sleep(2000);
            //        }
            //        else
            //        {

            //            Logger.Instance.Log(string.Format("{0} - Senders Connected: {1} ", signs.SenderId.ToString(), senderCt.ToString()));
            //            Logger.Instance.Log("{0} - IP: {1}.{2}.{3}.{4}", signs.SenderId.ToString(), kPlayerIP.addrSub1, kPlayerIP.addrSub2, kPlayerIP.addrSub3, kPlayerIP.abbrSub4_Start);

            //            recCt = LEDLane.ConnectRecv();
            //            Logger.Instance.Log(string.Format("{0} - Receivers Connected: {1} ", signs.SenderId.ToString(), recCt.ToString()));
            //            //rv1 = new receiverStatus_v1[recCt];
            //            break;
            //        }
            //        if (i == retries)
            //        {

            //        }
            //    }
            //}
        }


        public async void MonitorBrightness(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (signs.PollingState)
                {
                    switch (signs.BrightnessMode)
                    {
                        case 1: //Photocell
                            //GetPhotocell();
                            //GetBrightnessVal(signs.BrightnessMode);
                            //GetSignBrightness();
                            break;
                        case 2: //time of day
                            GetBrightnessVal(signs.BrightnessMode);
                            GetSignBrightness();
                            break;
                        case 3: //static
                            signs.ExpectedBrightness = Convert.ToByte(settings.GetSetting("StaticBrightness"));
                            GetSignBrightness();
                            break;

                    }
                }
                GC.Collect();
                GC.SuppressFinalize(this);
                await Task.Delay(10000);
            }

        }

        public async void GetModuleData(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                tagRecvModlInfo[] tmod = new tagRecvModlInfo[18];
                moduleStatus_v1 ms1 = new moduleStatus_v1();
                if (signs.PollingState)
                {
                    int j = 0;
                    int k = 0;

                    Logger.Instance.Log("SenderID: {0}", signs.SenderId);


                    for (int i = 0; i < 2; i++)
                    {
                        Logger.Instance.Log("{0} - Polling Modules...", signs.SenderId.ToString());

                        LEDLane.GetModuleData(signs.SenderId, (byte)j, 0, out ms1);

                        if (j == 0)
                        {
                            j++;
                        }
                        else
                        {
                            j--;
                            k++;
                        }


                        k = 0;
                        int x = 0;
                        int y = 0;

                        Logger.Instance.Log("SenderID: {0} Receiver Card: {1}", signs.SenderId, i);
                        for (int z = 0; z < 18; z++)
                        {

                            ms1.GetReceiverModlInfo((byte)x, (byte)y, ref tmod[z]);
                            signs.recCard[i].Mod[z].ColAddr = tmod[z].GetModlAddrCol();
                            signs.recCard[i].Mod[z].RowAddr = tmod[z].GetModlAddrRow();
                            signs.recCard[i].Mod[z].ModVolt = tmod[z].GetModlVoltAsFloat();
                            signs.recCard[i].Mod[z].ModTemp = tmod[z].GetModlTemperature();
                            signs.recCard[i].Mod[z].CableStatus = tmod[z].IsCableOk();

                            if (y == 5)
                            {
                                y = 0;
                                x++;
                            }
                            else
                            {
                                y++;
                            }

                            Logger.Instance.Log(string.Format("{0} - {1} {2} {3} {4}", signs.SenderId.ToString(), signs.recCard[i].Mod[z].ColAddr, signs.recCard[i].Mod[z].RowAddr,
                                signs.recCard[i].Mod[z].ModVolt, signs.recCard[i].Mod[z].ModTemp, signs.recCard[i].Mod[z].CableStatus));



                            string sql = ConfigurationManager.AppSettings["InsertModuleData"].ToString();
                            string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

                            using (SqlConnection conn = new SqlConnection(connString))
                            {
                                try
                                {
                                    using (SqlCommand cmd = new SqlCommand(sql))
                                    {
                                        cmd.Connection = conn;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@SignID", SqlDbType.Int).Value = signs.SignID;
                                        cmd.Parameters.Add("@SenderID", SqlDbType.Int).Value = signs.SenderId;
                                        cmd.Parameters.Add("@ReceiverIndexid", SqlDbType.Int).Value = signs.recCard[i].RecCardIndex;
                                        cmd.Parameters.Add("@ModTemp", SqlDbType.Decimal).Value = signs.recCard[i].Mod[z].ModTemp;
                                        cmd.Parameters.Add("@ModVoltage", SqlDbType.Decimal).Value = signs.recCard[i].Mod[z].ModVolt;
                                        cmd.Parameters.Add("@CableStatus", SqlDbType.Bit).Value = signs.recCard[i].Mod[z].CableStatus;
                                        cmd.Parameters.Add("@ModAddress", SqlDbType.VarChar).Value =
                                            signs.recCard[i].Mod[z].ColAddr.ToString() + "," + signs.recCard[i].Mod[z].RowAddr.ToString();
                                        cmd.Connection.Open();

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    Logger.Instance.Log("SQL Error: " + ex.Message);
                                }
                                catch (Exception e)
                                {
                                    Logger.Instance.Log("Error: " + e.Message);
                                }
                            }

                        }
                    }
                }
                await Task.Delay(10000);
            }
        }

        #endregion

        #region Private Methods
        private void GetSignBrightness()
        {
            bool success = false;
            brightnessObject britObj;
            for (int i = 0; i < (senderCt * 2); i++)
            {

                Logger.Instance.Log("SenderID : {0}", signs.SenderId.ToString());
                success = LEDLane.GetBrightness(signs.SenderId, (byte)i, out britObj);
                if (success)
                {
                    Logger.Instance.Log("{0} - Total Brightness: {1} \r\n Red: {2} \r\n Green: {3} \r\n Blue: {4}",
                         signs.SenderId.ToString(), britObj.totalBrightness.ToString(), britObj.redChannel.ToString(), britObj.greenChannel.ToString(), britObj.blueChannel.ToString());

                    signs.CurrentBrightness = britObj.totalBrightness;

                }
                else
                {
                    Logger.Instance.Log("{0} - Get Brightness Failed", signs.SenderId.ToString());
                }
            }

            if (success && signs.CurrentBrightness != signs.ExpectedBrightness && Convert.ToBoolean(settings.GetSetting("SetBrightness").ToString()))
            {
                //SetBrightness(signs.ExpectedBrightness);
            }
        }

        private void SetBrightness(byte level)
        {

            brightnessObject britObj = new brightnessObject();
            britObj.totalBrightness = level;
            britObj.redChannel = 255;
            britObj.greenChannel = 255;
            britObj.blueChannel = 255;

            if (LEDLane.SetBrightness(signs.SenderId, 0, britObj))
            {
                Logger.Instance.Log("{0} - Brightness Set on Port 0", signs.SenderId.ToString());
            }
            else
            {
                Logger.Instance.Log("{0} - Set Brightness failed on Port 0", signs.SenderId.ToString());
            }

            if (LEDLane.SetBrightness(signs.SenderId, 1, britObj))
            {
                Logger.Instance.Log("{0} - Brightness Set on Port 1", signs.SenderId.ToString());
            }
            else
            {
                Logger.Instance.Log("{0} - Set Brightness failed on Port 1", signs.SenderId.ToString());
            }


            string sql = ConfigurationManager.AppSettings["UpdBrightnessValue"];
            string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;


            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Brightness", SqlDbType.Int).Value = signs.CurrentBrightness;
                        cmd.Parameters.Add("@SignID", SqlDbType.Int).Value = signs.SignID;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Instance.Log("SQL Error: " + ex.Message);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log("Error: " + e.Message);
                }
            }

        }

        private void GetPhotocell()
        {
            
            for (int i = 0; i < recCt; i++)
            {
                Logger.Instance.Log("{0} - Polling Photocell...", signs.SenderId.ToString());

                LEDLane.GetReceiverData(signs.SenderId, (byte)i, 0, out rv1[i]);
                //signs.recCard[i].PhotoCell = rv1[i].bwEnviBritValue.GetWord();
                //signs.recCard[i].RecVolt = rv1[i].bwPowerVolt.GetFloat();
                //signs.recCard[i].RecTemp = rv1[i].bnReceiverTemperature.GetShort();
                //signs.recCard[i].BadPixels = rv1[i].bwBadPixelCount[i].GetWord();
                //signs.recCard[i].RecCardProps = rv1[i].byReceiverProps;
                //signs.recCard[i].RecCardIndex = i;

                //if (signs.PhotocellLocation == i)
                //{
                //    signs.recCard[i].PhotoCell = rv1[i].bwEnviBritValue.GetWord();
                //    signs.Lux = rv1[i].bwEnviBritValue.GetWord();
                //}
                //else
                //{
                //    signs.recCard[i].PhotoCell = 0;
                //    signs.Lux = 0;
                //}


                //Logger.Instance.Log("{0} - Receiver Card {1}:", signs.SenderId.ToString(), signs.SenderId.ToString(), (i + 1));
                //Logger.Instance.Log("{0} - LUX: {1} BRIT {2}: ", signs.SenderId.ToString(), signs.Lux, signs.recCard[i].PhotoCell);
                //Logger.Instance.Log("{0} - Rec Voltage: {1}", signs.SenderId.ToString(), signs.recCard[i].RecVolt);
                //Logger.Instance.Log("{0} - Rec Temp: {1}", signs.SenderId.ToString(), signs.recCard[i].RecTemp);
                //Logger.Instance.Log("{0} - Bad Pixels: {1} ", signs.SenderId.ToString(), signs.recCard[i].BadPixels);


                //string sql = ConfigurationManager.AppSettings["InsertRecCardData"];
                //string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;


                //using (SqlConnection conn = new SqlConnection(connString))
                //{
                //    try
                //    {
                //        using (SqlCommand cmd = new SqlCommand(sql))
                //        {
                //            cmd.Connection = conn;
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.Parameters.Add("@SignID", SqlDbType.Int).Value = signs.SignID;
                //            cmd.Parameters.Add("@SenderID", SqlDbType.Int).Value = signs.SenderId;
                //            cmd.Parameters.Add("@ReceiverIndexid", SqlDbType.Int).Value = signs.recCard[i].RecCardIndex;
                //            cmd.Parameters.Add("@RecTemp", SqlDbType.Decimal).Value = signs.recCard[i].RecTemp;
                //            cmd.Parameters.Add("@RecVoltage", SqlDbType.Decimal).Value = signs.recCard[i].RecVolt;
                //            cmd.Parameters.Add("@RecCardMode", SqlDbType.Bit).Value = signs.recCard[i].RecCardMode;
                //            cmd.Parameters.Add("@Phcell", SqlDbType.Int).Value = signs.recCard[i].PhotoCell;
                //            cmd.Parameters.Add("@Lux", SqlDbType.Int).Value = signs.Lux;
                //            cmd.Parameters.Add("@BadPixelCT", SqlDbType.Int).Value = signs.recCard[i].BadPixels;
                //            cmd.Connection.Open();

                //            cmd.ExecuteNonQuery();
                //        }
                //    }
                //    catch (SqlException ex)
                //    {
                //        Logger.Instance.Log("SQL Error: " + ex.Message);
                //    }
                //    catch (Exception e)
                //    {
                //        Logger.Instance.Log("Error: " + e.Message);
                //    }
                //}
            }
        }

        private void GetBrightnessVal(int brightnessMode)
        {
            string sql = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            if (brightnessMode == 1)
            {
                sql = ConfigurationManager.AppSettings["GetPhCellBritVal"].ToString();
            }
            else if (brightnessMode == 2)
            {
                sql = ConfigurationManager.AppSettings["GetToDLevel"].ToString();
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection.Open();

                        var expbrit = cmd.ExecuteScalar();
                        signs.ExpectedBrightness = Convert.ToByte(expbrit);
                    }


                }
                catch (SqlException ex)
                {
                    Logger.Instance.Log("SQL Error: " + ex.Message);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log("Error: " + e.Message);
                }
            }
        }

        private void LoadAlarms()
        {

        }

        #endregion

    }


}
