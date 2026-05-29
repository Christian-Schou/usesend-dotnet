namespace UseSend.Razor;

/// <summary>
///     Renders Razor (.cshtml) templates to HTML strings for use as email bodies.
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    ///     Renders the Razor template identified by <paramref name="templateKey" /> with the given
    ///     <paramref name="model" />, returning the rendered HTML.
    /// </summary>
    /// <typeparam name="TModel">Type of the view model passed into the template.</typeparam>
    /// <param name="templateKey">
    ///     Relative path (without extension) to the template under the configured template root.
    ///     For example, <c>"Emails/Welcome"</c> resolves to <c>{templateRoot}/Emails/Welcome.cshtml</c>.
    /// </param>
    /// <param name="model">The view model populated from application data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<string> RenderAsync<TModel>(string templateKey, TModel model, CancellationToken cancellationToken = default);
}
