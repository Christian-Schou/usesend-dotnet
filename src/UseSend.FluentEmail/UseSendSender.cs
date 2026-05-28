using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;

namespace UseSend.FluentEmail;

/// <summary>
///     FluentEmail <see cref="ISender" /> implementation backed by the useSend API.
/// </summary>
public class UseSendSender : ISender
{
    private readonly IEmailService _emails;

    /// <summary>
    ///     Initializes a new instance of <see cref="UseSendSender" />.
    /// </summary>
    /// <param name="emails">The useSend email service.</param>
    public UseSendSender(IEmailService emails)
    {
        _emails = emails;
    }


    /// <inheritdoc />
    public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
    {
        return SendAsync(email, token).GetAwaiter().GetResult();
    }


    /// <inheritdoc />
    public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
    {
        var data = email.Data;
        var ct = token ?? CancellationToken.None;

        var message = new EmailMessage
        {
            From = FormatAddress(data.FromAddress),
            To = data.ToAddresses.Select(FormatAddress).ToList(),
            Subject = data.Subject ?? string.Empty
        };

        if (data.CcAddresses.Count > 0)
            message.Cc = data.CcAddresses.Select(FormatAddress).ToList();

        if (data.BccAddresses.Count > 0)
            message.Bcc = data.BccAddresses.Select(FormatAddress).ToList();

        if (data.ReplyToAddresses.Count > 0)
            message.ReplyTo = data.ReplyToAddresses.Select(FormatAddress).ToList();

        if (!string.IsNullOrEmpty(data.Body))
        {
            if (data.IsHtml)
                message.Html = data.Body;
            else
                message.Text = data.Body;
        }

        if (!string.IsNullOrEmpty(data.PlaintextAlternativeBody))
            message.Text = data.PlaintextAlternativeBody;

        if (data.Attachments.Count > 0)
            message.Attachments = data.Attachments
                .Select(a => new EmailAttachment
                {
                    Filename = a.Filename,
                    Content = ReadAttachmentStream(a.Data)
                })
                .ToList();

        var response = new SendResponse();

        var result = await _emails.SendAsync(message, ct).ConfigureAwait(false);

        if (!result.Success)
        {
            response.ErrorMessages.Add(result.Exception?.Message ?? "useSend returned an error.");
            return response;
        }

        response.MessageId = result.Content;
        return response;
    }


    private static string FormatAddress(Address address)
    {
        return string.IsNullOrWhiteSpace(address.Name)
            ? address.EmailAddress
            : $"{address.Name} <{address.EmailAddress}>";
    }


    private static string ReadAttachmentStream(Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}