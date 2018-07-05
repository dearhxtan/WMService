锘縰sing HelpDesk.Common;
using HelpDesk.SAPConnector;
using SAP.Middleware.Connector;
using System;
using System.Configuration;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using HelpDesk.WMService.Class;
using System.Linq;
using System.Collections.Generic;

namespace HelpDesk.WebService
{
    /// <summary>
    /// SAPInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://helpdesk.liphing.com/SAPInvoke/", Name = "SAPInvoke")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SAPInvoke : System.Web.Services.WebService
    {

        [WebMethod(Description = "SAP RFC 测试服务")]
        public string Z_RFC_USER(string zuserno, string zpassword)
        {
            Z_RFC_USER value = new Z_RFC_USER();

            RFCManager rm = new RFCManager
            {
                setting = new Setting(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[AppSettingName.CONFIG_SAP_ADDRESS]))
            };
            rm.setting.readSetting("C601");
            rm.setting.Passwd = "cced2211";
            RfcDestination rfc = rm.RFCInitial();
            if (rfc != null)
            {
                try
                {
                    IRfcFunction func = rfc.Repository.CreateFunction("Z_RFC_USER");
                    func.SetValue("ZUSERNO", zuserno);
                    func.SetValue("ZPASSWORD", zpassword);
                    func.Invoke(rfc);

                    value.ZTYPE = func.GetValue("ZTYPE").ToString();
                    var struc = func.GetStructure("GS_ZRFC_TEST2");
                    if (struc != null)
                    {
                        value.GS_ZRFC_TEST2 = new ZRFC_TEST
                        {
                            ZNO1 = struc.GetString("ZNO1"),
                            ZNAME1 = struc.GetString("ZNAME1"),
                            ZREMARK = struc.GetString("ZREMARK"),
                            ZDATE1 = struc.GetString("ZDATE1"),
                            ZTIME1 = struc.GetString("ZTIME1"),
                            ZQTY = struc.GetString("ZQTY")
                        };
                    }
                    ZRFC_TEST zitem = new ZRFC_TEST();
                    var table = func.GetTable("GT_ZRFC_TEST");
                    if (table.Count > 0)
                    {
                        value.GT_ZRFC_TEST = new List<ZRFC_TEST>();
                        for (int i = 0; i < table.Count; i++)
                        {
                            zitem = new ZRFC_TEST
                            {
                                ZNO1 = table[i].GetValue("ZNO1").ToString(),
                                ZNAME1 = table[i].GetValue("ZNAME1").ToString(),
                                ZREMARK = table[i].GetValue("ZREMARK").ToString(),
                                ZDATE1 = table[i].GetValue("ZDATE1").ToString(),
                                ZTIME1 = table[i].GetValue("ZTIME1").ToString(),
                                ZQTY = table[i].GetValue("ZQTY").ToString()
                            };
                            value.GT_ZRFC_TEST.Add(zitem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    value.ERROR_MESSAGE = ex.Message + (ex.InnerException == null ? string.Empty : ex.InnerException.Message);
                }
            }
            else
            {
                value.ERROR_MESSAGE = rm.ErrorMessage;
            }

            return JsonConvert.SerializeObject(value);
        }


        [WebMethod(Description = "输入客户料号，读取相应 SAP 料号")]
        public string GetSAPNoByCustomerPN(string customerpn)
        {
            SingleValue value = new SingleValue();

            RFCManager rm = new RFCManager
            {
                setting = new Setting(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[AppSettingName.CONFIG_SAP_ADDRESS]))
            };
            rm.setting.readSetting("C601");
            rm.setting.Passwd = "cced2211";
            RfcDestination rfc = rm.RFCInitial();
            if (rfc != null)
            {
                RFCData data = new RFCData();
                value.value = data.RFCGetSingleString(rfc, "KNMT", "MATNR", string.Format("KDMAT = '{0}'", customerpn)).TrimStart('0');
                value.error_message = data.ErrorMessage;
            }
            else
            {
                value.error_message = rm.ErrorMessage;
            }

            return JsonConvert.SerializeObject(value);
        }

        public string TestNewMethod()
        {
        	// test adding
            // test adding2
            // test adding3
            // test adding by eclipse
            // test adding by eclipse2
            return string.Empty;
        }
    }
}
