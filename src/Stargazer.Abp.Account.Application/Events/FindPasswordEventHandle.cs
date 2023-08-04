using Stargazer.Abp.Account.Application.Services;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Stargazer.Abp.Account.Application.Events;

public class FindPasswordEventHandle : ILocalEventHandler<FindPasswordEvent>, ITransientDependency
{
    private readonly EmailService _emailService;

    public FindPasswordEventHandle(EmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleEventAsync(FindPasswordEvent eventData)
    {
        await _emailService.FindPassword(eventData);
    }
}