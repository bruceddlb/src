

using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using iFramework.Framework;
using iFramework.Framework.Log;
using QSDMS.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace QSDMS.Application.Library.AliPay
{
    public class AliPay
    {
        private static string APPID = ConfigurationManager.AppSettings["APPID"];
        private static string APP_PRIVATE_KEY = ConfigurationManager.AppSettings["APP_PRIVATE_KEY"];
        private static string ALIPAY_PUBLIC_KEY = ConfigurationManager.AppSettings["ALIPAY_PUBLIC_KEY"];
        private static string ALIPAY_NOTIFY_URL = ConfigurationManager.AppSettings["ALIPAY_NOTIFY_URL"];
        private static string CHARSET = "utf-8";

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <returns></returns>
        public ReturnMessage AlipayTradeCreate(string orderno)
        {
            var result = new ReturnMessage();
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, CHARSET, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = string.Format("{0}订单支付", "xxxx-xxxx-xxxx");
            model.Subject = "App支付";
            model.TotalAmount = "10000";
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.OutTradeNo = "xxxx-xxxx-xxxx";
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl(ALIPAY_NOTIFY_URL);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            result.Message = "成功";
            result.ResultData["content"] = response.Body;
            result.IsSuccess = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReturnMessage Notify()
        {
            var para = GetRequestPost();
            bool flag = AlipaySignature.RSACheckV1(para, ALIPAY_PUBLIC_KEY, CHARSET, "RSA2", false);
            if (flag)
            {
                var trade_status = para["trade_status"];
                switch (trade_status)
                {
                    case "TRADE_SUCCESS":
                    case "TRADE_FINISHED":
                        var orderNo = para["out_trade_no"];
                        //获取订单详情
                        var amount = Convert.ToDecimal(para["total_amount"]);
                        //实际收款金额
                        var buyer_pay_amount = Convert.ToDecimal(para["buyer_pay_amount"]);
                        var sArray = new Dictionary<string, string>();
                        sArray.Add("OrderNO", orderNo);
                        var sign = SignHelper.GetSignature(sArray);
                        //对比实收金额与订单的所需现金金额是否一致
                        //if (Pay(orderNo, buyer_pay_amount, sign))
                        //{

                        //    content = "success";
                        //}
                        //记录信息
                        var _logger = LogFactory.GetLogger(GetType());
                        _logger.Info(string.Format("{0},{1},{2}", orderNo, amount, buyer_pay_amount));

                        break;
                    case "WAIT_BUYER_PAY":
                        break;
                    case "TRADE_CLOSED":
                        break;
                }

            }
            return null;
        }


        // 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组 
        /// request回来的信息组成的数组
        private Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;
            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}