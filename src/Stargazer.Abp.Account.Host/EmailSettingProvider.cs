using Volo.Abp.Emailing;
using Volo.Abp.Settings;

namespace Stargazer.Abp.Account.Host;

public class EmailSettingProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(EmailSettingNames.Smtp.Host, "127.0.0.1"),
            new SettingDefinition(EmailSettingNames.Smtp.Port, "25"),
            new SettingDefinition(EmailSettingNames.Smtp.UserName),
            new SettingDefinition(EmailSettingNames.Smtp.Password, isEncrypted: true),
            new SettingDefinition(EmailSettingNames.Smtp.Domain),
            new SettingDefinition(EmailSettingNames.Smtp.EnableSsl, "false"),
            new SettingDefinition(EmailSettingNames.Smtp.UseDefaultCredentials, "true"),
            new SettingDefinition(EmailSettingNames.DefaultFromAddress, "noreply@abp.io"),
            new SettingDefinition(EmailSettingNames.DefaultFromDisplayName, "ABP application")
        );

        var emailSmtpPassword = context.GetOrNull(EmailSettingNames.Smtp.Password);
        if (emailSmtpPassword != null)
        {
            emailSmtpPassword.IsEncrypted = false;
        }
    }
}