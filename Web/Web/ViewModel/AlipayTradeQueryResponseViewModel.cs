using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModel
{
    /// <summary>
    /// 统一收单线下交易查询接口响应
    /// <para>https://docs.open.alipay.com/api_1/alipay.trade.query</para>
    /// </summary>
    public class AlipayTradeQueryResponseViewModel
    {
        /// <summary>
        /// 统一收单线下交易查询接口响应
        /// </summary>
        public Alipay_Trade_Query_Response alipay_trade_query_response { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }

    public class Alipay_Trade_Query_Response
    {
        /// <summary>
        /// 网关返回码
        /// <para>https://docs.open.alipay.com/common/105806</para>
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 网关返回码描述
        /// <para>同上</para>
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }

        /// <summary>
        /// 商家订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 买家支付宝账号
        /// </summary>
        public string buyer_logon_id { get; set; }

        /// <summary>
        /// 交易状态
        /// <para>WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款）</para>
        /// </summary>
        public string trade_status { get; set; }

        /// <summary>
        /// 交易的订单金额，单位为元，两位小数。该参数的值为支付时传入的total_amount
        /// </summary>
        public float total_amount { get; set; }

        /// <summary>
        /// 标价币种，该参数的值为支付时传入的trans_currency
        /// </summary>
        public string trans_currency { get; set; }

        /// <summary>
        /// 订单结算币种，对应支付接口传入的settle_currency
        /// </summary>
        public string settle_currency { get; set; }

        /// <summary>
        /// 结算币种订单金额
        /// </summary>
        public float settle_amount { get; set; }

        /// <summary>
        /// 订单支付币种
        /// </summary>
        public int pay_currency { get; set; }

        /// <summary>
        /// 支付币种订单金额
        /// </summary>
        public string pay_amount { get; set; }

        /// <summary>
        /// 结算币种兑换标价币种汇率
        /// </summary>
        public string settle_trans_rate { get; set; }

        /// <summary>
        /// 标价币种兑换支付币种汇率
        /// </summary>
        public string trans_pay_rate { get; set; }

        /// <summary>
        /// 买家实付金额，单位为元，两位小数。
        /// <para>该金额代表该笔交易买家实际支付的金额，不包含商户折扣等金额</para>
        /// </summary>
        public float buyer_pay_amount { get; set; }

        /// <summary>
        /// 积分支付的金额，单位为元，两位小数。
        /// <para>该金额代表该笔交易中用户使用积分支付的金额，比如集分宝或者支付宝实时优惠等</para>
        /// </summary>
        public string point_amount { get; set; }

        /// <summary>
        /// 交易中用户支付的可开具发票的金额，单位为元，两位小数。
        /// <para>该金额代表该笔交易中可以给用户开具发票的金额</para>
        /// </summary>
        public float invoice_amount { get; set; }

        /// <summary>
        /// 本次交易打款给卖家的时间
        /// </summary>
        public string send_pay_date { get; set; }

        /// <summary>
        /// 实收金额，单位为元，两位小数。该金额为本笔交易，商户账户能够实际收到的金额
        /// </summary>
        public string receipt_amount { get; set; }

        /// <summary>
        /// 商户门店编号
        /// </summary>
        public string store_id { get; set; }

        /// <summary>
        /// 商户机具终端编号
        /// </summary>
        public string terminal_id { get; set; }

        /// <summary>
        /// 交易支付使用的资金渠道
        /// </summary>
        public Fund_Bill_List[] fund_bill_list { get; set; }

        /// <summary>
        /// 请求交易支付中的商户店铺的名称
        /// </summary>
        public string store_name { get; set; }

        /// <summary>
        /// 买家在支付宝的用户id
        /// </summary>
        public string buyer_user_id { get; set; }

        /// <summary>
        /// 预授权支付模式，该参数仅在信用预授权支付场景下返回。
        /// <para>信用预授权支付：CREDIT_PREAUTH_PAY</para>
        /// </summary>
        public string auth_trade_pay_mode { get; set; }

        /// <summary>
        /// 买家用户类型。
        /// <para>CORPORATE:企业用户；PRIVATE:个人用户。</para>
        /// </summary>
        public string buyer_user_type { get; set; }

        /// <summary>
        /// 商家优惠金额
        /// </summary>
        public string mdiscount_amount { get; set; }

        /// <summary>
        /// 平台优惠金额
        /// </summary>
        public string discount_amount { get; set; }
    }

    /// <summary>
    /// 交易支付使用的资金渠道
    /// </summary>
    public class Fund_Bill_List
    {
        /// <summary>
        /// 交易使用的资金渠道
        /// <para>https://doc.open.alipay.com/doc2/detail?treeId=26&articleId=103259&docType=1</para>
        /// </summary>
        public string fund_channel { get; set; }

        /// <summary>
        /// 银行卡支付时的银行代码
        /// </summary>
        public string bank_code { get; set; }

        /// <summary>
        /// 该支付工具类型所使用的金额
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 渠道实际付款金额
        /// </summary>
        public float real_amount { get; set; }
    }
}