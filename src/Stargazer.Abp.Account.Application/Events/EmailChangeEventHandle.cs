using System.Text;
using Lemon.Common.Extend;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.EventBus;
using Volo.Abp.TextTemplating;

namespace Stargazer.Abp.Account.Application.Events;

public class EmailChangedEventHandler : ILocalEventHandler<EmailChangedEvent>, ITransientDependency
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly IDistributedCache<string> _cache;
    private readonly ILogger<EmailChangedEventHandler> _logger;

    public EmailChangedEventHandler(IConfiguration configuration, IEmailSender emailSender,
        IDistributedCache<string> cache, ILogger<EmailChangedEventHandler> logger, ITemplateRenderer templateRenderer)
    {
        _configuration = configuration;
        _emailSender = emailSender;
        _cache = cache;
        _logger = logger;
        _templateRenderer = templateRenderer;
    }

    public async Task HandleEventAsync(EmailChangedEvent eventData)
    {
        try
        {
            bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
            if (!verifyEmail)
            {
                return;
            }

            string? host = _configuration.GetSection("App:Host").Value ?? "";
            var token = Ext.CreateNonceStr(128);
            await _cache.SetAsync($"VerifyEmail:{eventData.Email}", token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            });

            var verifyUrl = $"{host}/api/account/verify-email?id={eventData.User.Id}&token={token}";
            StringBuilder message = new();
            message.Append("<div style='text-align:center;font-size:24px;'>");
            message.Append($"{eventData.User.NickName}，您好。");
            message.Append("<br />");
            message.Append($"您的账户邮箱<a href='mailto:{eventData.Email}'>{eventData.Email}</a>已发生变更。");
            message.Append("<br />");
            message.Append($"请<a href='{verifyUrl}'>激活</a>邮箱，此链接有效时间为20分钟。");
            message.Append("</div>");

            var body = await _templateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = message.ToString()
                }
            );
            string subject = "Account Request";
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
    }
}