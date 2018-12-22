# Alipay
支付宝

# 插件
安装支付宝官方插件：Install-Package Alipay.AopSdk.Core

# 步骤
1. 发起支付，将记录写入数据库，并发起支付宝支付
2. 用户支付后，进入同步回调
3. 当同步回调无法执行，则支付在异步回调中执行（异步回调由支付宝自主运行，与用户浏览器状态无关）

# 回调操作
1. 获取回调参数，将参数保存到字典Dictionary<string,string>中
2. 验签
3. 获取订单信息
4. 获取订单支付状态，并更新到数据库

# 代码解析
https://blog.csdn.net/qq_31267183/article/details/84445581
