# SiteServer CMS 邮件插件(SS.Mail)

用于实现发送邮件功能。

## 插件调用SS.Mail包

开发插件时可以引用并调用 SS.Mail 邮件插件接口发送邮件，方法如下：

### SS.Mail包安装

请在包管理器控制台中运行以下命令：
```
PM> Install-Package SS.Mail
```

### API 调用

```c#
var mailPlugin = PluginApi.GetPlugin<MailPlugin>(MailPlugin.PluginId);
if (mailPlugin != null && mailPlugin.IsReady)
{
    var isSuccess = mailPlugin.Send("address@domain.com", "收件人", "邮件标题", "邮件正文", out var errorMessage);
}
```
