using Stargazer.Abp.Account.Application.Services;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Stargazer.Abp.Account.Application.Events;

public class EmailChangedEventHandler : ILocalEventHandler<EmailChangedEvent>, ITransientDependency
{
    private readonly EmailService _emailService;

    public EmailChangedEventHandler(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleEventAsync(EmailChangedEvent eventData)
    {
        await _emailService.EmailChanged(eventData);
    }
}