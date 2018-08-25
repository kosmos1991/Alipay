using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using Alipay.AopSdk.Core.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.ViewModel;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        static string AppId = ""; // 应用APPID
        //static string Gatewayurl = "https://openapi.alipaydev.com/gateway.do"; // 支付宝网关（沙箱）
        static string Gatewayurl = "https://openapi.alipay.com/gateway.do"; // 支付宝网关（沙箱）
        static string PrivateKey = "";
        static string AlipayPublicKey = ""; // 公钥
        static string SignType = "RSA2"; // 签名方式
        static string CharSet = "UTF-8"; // 编码格式

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            PayRequest();
            return View();
        }

        /// <summary>
        /// 发起支付请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void PayRequest()
        {
            DefaultAopClient client = new DefaultAopClient(Gatewayurl, AppId, PrivateKey, "json", "2.0",
                SignType, AlipayPublicKey, CharSet, false);

            string tradeno = "1"; // 外部订单号，商户网站订单系统中唯一的订单号
            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = "商品描述"; // 商品描述
            model.Subject = "订单名称"; // 订单名称
            model.TotalAmount = "0.01"; // 付款金额
            model.OutTradeNo = tradeno; // 外部订单号，商户网站订单系统中唯一的订单号
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl("http://localhost:58616/Home/Callback");
            // 设置异步通知接收地址
            request.SetNotifyUrl("http://localhost:58616/Home/Notify");
            // 将业务model载入到request
            request.SetBizModel(model);

            var response = client.SdkExecute(request);
            Console.WriteLine($"订单支付发起成功，订单号：{tradeno}");
            //跳转支付宝支付
            Response.Redirect(Gatewayurl + "?" + response.Body);
        }

        /// <summary>
        /// 同步回调
        /// </summary>
        public ActionResult Callback()
        {
            Dictionary<string, string> sArray = GetRequestGet();
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, AlipayPublicKey, CharSet, SignType, false);
                if (flag)
                {
                    Console.WriteLine($"同步验证通过，订单号：{sArray["out_trade_no"]}");
                    ViewData["PayResult"] = "同步验证通过";
                }
                else
                {
                    Console.WriteLine($"同步验证失败，订单号：{sArray["out_trade_no"]}");
                    ViewData["PayResult"] = "同步验证失败";
                }

                AlipayTradeQueryResponseViewModel traceState = GetTraceMsg(sArray["out_trade_no"], sArray["trade_no"]);
                if (traceState.alipay_trade_query_response.code != "10000")
                    Console.WriteLine($"获取订单失败，失败原因：{traceState.alipay_trade_query_response.msg}");
            }
            return View();
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 支付异步回调通知 需配置域名 因为是支付宝主动post请求这个action 所以要通过域名访问或者公网ip
        /// </summary>
        public ActionResult Notify()
        {
            /* 实际验证过程建议商户添加以下校验。
            1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
            2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
            3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
            4、验证app_id是否为该商户本身。
            */
            Dictionary<string, string> sArray = GetRequestPost(Request);
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, AlipayPublicKey, CharSet, SignType, false);
                if (flag)
                {
                    AlipayTradeQueryResponseViewModel traceState = GetTraceMsg(sArray["out_trade_no"], sArray["trade_no"]);
                    if (traceState.alipay_trade_query_response.code != "10000")
                        Console.WriteLine($"获取订单失败，失败原因：{traceState.alipay_trade_query_response.msg}");

                    //交易状态
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                    //如果有做过处理，不执行商户的业务程序

                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    Console.WriteLine(Request.Form["trade_status"]);

                    return View("Success");
                }
                else
                {
                    return View("Fail");
                }
            }

            return View("Fail");
        }

        /// <summary>
        /// 异步通知页验签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRequestPost(HttpRequestBase request)
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            System.Collections.Specialized.NameValueCollection coll;
            coll = request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], request.Form[requestItem[i]]);
            }
            return sArray;
        }

        /// <summary>
        /// 统一收单线下交易查询接口
        /// </summary>
        /// <returns></returns>
        public AlipayTradeQueryResponseViewModel GetTraceMsg(string out_trade_no, string trade_no)
        {
            IAopClient client = new DefaultAopClient(Gatewayurl, AppId, PrivateKey, "json", "1.0", SignType, AlipayPublicKey, CharSet, false);

            Dictionary<string, string> biz = new Dictionary<string, string>();
            biz.Add("out_trade_no", out_trade_no);
            biz.Add("trade_no", trade_no);

            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = JsonConvert.SerializeObject(biz);

            AlipayTradeQueryResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<AlipayTradeQueryResponseViewModel>(response.Body);

        }
    }
}
