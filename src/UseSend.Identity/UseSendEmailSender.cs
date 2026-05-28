using Microsoft.AspNetCore.Identity.UI.Services;

namespace UseSend.Identity;

/// <summary>
///     ASP.NET Core Identity <see cref="IEmailSender" /> implementation backed by the useSend API.
/// </summary>
public class UseSendEmailSender : IEmailSender
{
    private readonly IEmailService _emails;
    private readonly EmailSenderOptions _options;

    /// <param name="emails">The useSend email service.</param>
    /// <param name="options">Sender options (from address, display name).</param>
    public UseSendEmailSender(IEmailService emails, EmailSenderOptions options)
    {
        _emails = emails;
        _options = options;
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await _emails.SendAsync(new EmailMessage
        {
            From    = _options.FormattedFrom,
            To      = email,
            Subject = subject,
            Html    = htmlMessage,
        });
    }
}

/// <summary>
///     ASP.NET Core Identity <see cref="Microsoft.AspNetCore.Identity.IEmailSender{TUser}" /> implementation backed by the useSend API.
///     Provides default HTML templates for confirmation links, password reset links, and reset codes.
///     Override any method to customise the email content.
/// </summary>
public class UseSendEmailSender<TUser> : UseSendEmailSender,
    Microsoft.AspNetCore.Identity.IEmailSender<TUser> where TUser : class
{
    /// <inheritdoc />
    public UseSendEmailSender(IEmailService emails, EmailSenderOptions options)
        : base(emails, options) { }

    /// <inheritdoc />
    public virtual Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, "Confirm your email address",
            $"""
             <p>Thanks for signing up! Please confirm your email address by clicking the link below.</p>
             <p><a href="{confirmationLink}">Confirm my email</a></p>
             <p>If you did not create an account, you can ignore this email.</p>
             """);

    /// <inheritdoc />
    public virtual Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink) =>
        SendEmailAsync(email, "Reset your password",
            $"""
             <p>You requested a password reset. Click the link below to choose a new password.</p>
             <p><a href="{resetLink}">Reset my password</a></p>
             <p>If you did not request a password reset, you can ignore this email.</p>
             """);

    /// <inheritdoc />
    public virtual Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode) =>
        SendEmailAsync(email, "Reset your password",
            $"""
             <p>Your password reset code is:</p>
             <p><strong>{resetCode}</strong></p>
             <p>If you did not request a password reset, you can ignore this email.</p>
             """);
}
