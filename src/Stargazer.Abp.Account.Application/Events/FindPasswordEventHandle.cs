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

public class FindPasswordEventHandle : ILocalEventHandler<FindPasswordEvent>, ITransientDependency
{
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IEmailSender _emailSender;
    private readonly IDistributedCache<string> _cache;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailChangedEventHandler> _logger;

    public FindPasswordEventHandle(IEmailSender emailSender, IDistributedCache<string> cache,
        ILogger<EmailChangedEventHandler> logger, IUserService userService, IConfiguration configuration,
        ITemplateRenderer templateRenderer)
    {
        _emailSender = emailSender;
        _logger = logger;
        _userService = userService;
        _configuration = configuration;
        _templateRenderer = templateRenderer;
        _cache = cache;
    }

    public async Task HandleEventAsync(FindPasswordEvent eventData)
    {
        try
        {
            var user = await _userService.FindByEmailAsync(eventData.Email);
            var token = Ext.CreateNonceStr(64);
            await _cache.SetAsync($"FindPasswordToken:{eventData.Email}", token, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            });
            string host = _configuration.GetSection("App:Host").Value ?? "";
            var changePasswordUrl = $"{host}/resetpassword?email={eventData.Email}&token={token}";
            StringBuilder message = new();
            message.Append("<div style='text-align:center;font-size:24px;'>");
            message.Append($"<p style='font-size:20px;'>{user.NickName}，您好。</p>");
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
    }
}