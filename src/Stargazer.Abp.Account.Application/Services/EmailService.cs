using System.Text;
using Lemon.Common.Extend;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.TextTemplating;

namespace Stargazer.Abp.Account.Application.Services;

public class EmailService : ITransientDependency
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly IDistributedCache<string> _cache;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ITemplateRenderer templateRenderer, IConfiguration configuration, IEmailSender emailSender,
        IDistributedCache<string> cache, ILogger<EmailService> logger)
    {
        _templateRenderer = templateRenderer;
        _configuration = configuration;
        _emailSender = emailSender;
        _cache = cache;
        _logger = logger;
    }

    public async Task EmailChanged(EmailChangedEvent eventData)
    {
        try
        {
            _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} verify start");
            bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
            if (!verifyEmail)
            {
                _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} not need verify");
                return;
            }

            string key = $"VerifyEmail:{eventData.Email}";
            string? token = await _cache.GetAsync(key);
            if (!string.IsNullOrWhiteSpace(token))
            {
                _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} token have sent");
                return;
            }

            string host = _configuration.GetSection("App:Host").Value ?? "";
            token = Ext.CreateNonceStr(128);
            await _cache.SetAsync($"VerifyEmail:{eventData.Email}", token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            });

            var verifyUrl = $"{host}/verify-email?email={eventData.Email}&token={token}";
            StringBuilder message = new();
            message.Append("<div style='text-align:center;'>");
            message.Append($"<p style='font-size:20px;'>{eventData.User.NickName}，请确认您的电子邮件地址！</p>");
            message.Append(
                $"<p style='font-size:18px;'>单击下面的链接以在<a href='{host}'>{host}</a>上验证您的电子邮件地址（<a href='mailto:{eventData.Email}'>{eventData.Email}</a>）</p>");
            message.Append($"<p style='font-size:18px;'><a href='{verifyUrl}'>确认您的邮件地址</a></p>");
            message.Append("<p style='font-size:18px;'>如果该验证地址已失效，请通过登录重新发送激活邮件。</p>");
            message.Append("<p style='font-size:18px;'>如果您没有执行此请求，您可以安全地忽略此电子邮件。</p>");
            message.Append("</div>");

            var body = await _templateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = message.ToString()
                }
            );
            string subject = "确认说明";
            await _emailSender.SendAsync(
                to: eventData.Email, // target email address
                subject: subject, // subject
                body: body // email body
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        finally
        {
            _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} verify end");
        }
    }

    public async Task FindPassword(FindPasswordEvent eventData)
    {
        try
        {
            _logger.LogInformation($"###FindPasswordEventHandle###-------{eventData.Email} find password start");
            var token = Ext.CreateNonceStr(64);
            await _cache.SetAsync($"FindPasswordToken:{eventData.Email}", token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            });
            string host = _configuration.GetSection("App:Host").Value ?? "";
            var changePasswordUrl = $"{host}/resetpassword?email={eventData.Email}&token={token}";
            StringBuilder message = new();
            message.Append("<div style='text-align:center;font-size:24px;'>");
            message.Append($"<p style='font-size:20px;'>{eventData.User.NickName}，您好。</p>");
            message.Append($"<p style='font-size:18px;'>有人（希望是您）要求在 <a href='{host}'>{host}</a> 上重置您的账号的密码。</p>");
            message.Append("<p style='font-size:18px;'>如果您没有执行此请求，您可以安全地忽略此电子邮件。</p>");
            message.Append("<p style='font-size:18px;'>否则，点击下面的链接来完成这一进程。</p>");
            message.Append($"<p style='font-size:18px;'><a href='{changePasswordUrl}'>重置密码</a></p>");
            message.Append("</div>");
            var body = await _templateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = message.ToString()
                }
            );
            string subject = "重置密码说明";
            await _emailSender.SendAsync(
                to: eventData.Email, // target email address
                subject: subject, // subject
                body: body // email body
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
        finally
        {
            _logger.LogInformation($"###FindPasswordEventHandle###-------{eventData.Email} find password end");
        }
    }
}