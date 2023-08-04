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
using Volo.Abp.EventBus;
using Volo.Abp.TextTemplating;

namespace Stargazer.Abp.Account.Application.Events;

public class EmailChangedEventHandler : ILocalEventHandler<EmailChangedEvent>, ITransientDependency
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly IDistributedCache<string> _cache;
    private readonly IUserService _userService;
    private readonly ILogger<EmailChangedEventHandler> _logger;

    public EmailChangedEventHandler(IConfiguration configuration, IEmailSender emailSender,
        IDistributedCache<string> cache, ILogger<EmailChangedEventHandler> logger, ITemplateRenderer templateRenderer, IUserService userService)
    {
        _configuration = configuration;
        _emailSender = emailSender;
        _cache = cache;
        _logger = logger;
        _templateRenderer = templateRenderer;
        _userService = userService;
    }

    public async Task HandleEventAsync(EmailChangedEvent eventData)
    {
        try
        {
            _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} verify start");
            var user = await _userService.FindByEmailAsync(eventData.Email);
            if (user == null)
            {
                _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} not found");
                return;
            }
            bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
            if (!verifyEmail)
            {
                _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} not need verify");
                return;
            }

            string? host = _configuration.GetSection("App:Host").Value ?? "";
            var token = Ext.CreateNonceStr(128);
            await _cache.SetAsync($"VerifyEmail:{eventData.Email}", token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            });

            var verifyUrl = $"{host}/verify-email?email={eventData.Email}&token={token}";
            StringBuilder message = new();
            message.Append("<div style='text-align:center;'>");
            message.Append($"<p style='font-size:20px;'>{user.NickName}，请确认您的电子邮件地址！</p>");
            message.Append($"<p style='font-size:18px;'>单击下面的链接以在<a href='{host}'>{host}</a>上验证您的电子邮件地址（<a href='mailto:{eventData.Email}'>{eventData.Email}</a>）</p>");
            message.Append($"<p style='font-size:18px;'><a href='{verifyUrl}'>确认您的邮件地址</a></p>");
            message.Append($"<p style='font-size:18px;'>如果您没有执行此请求，您可以安全地忽略此电子邮件。</p>");
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
        } finally
        {
            _logger.LogInformation($"###EmailChangedEventHandler###-------{eventData.Email} verify end");
        }
    }
}