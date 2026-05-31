namespace UseSend;

/// <summary>
///     Renders email templates to HTML strings.
///     Implementations include <c>UseSend.Razor</c> (Razor/.cshtml) and <c>UseSend.Fluid</c> (Liquid/.liquid).
/// </summary>
public interface IEmailTemplateRenderer
{
    /// <summary>
    ///     Renders the template identified by <paramref name="templateKey" /> with the given
    ///     <paramref name="model" />, returning the rendered HTML string.
    /// </summary>
    /// <typeparam name="TModel">Type of the view model passed into the template.</typeparam>
    /// <param name="templateKey">
    ///     Relative path (without extension) to the template under the configured template root.
    ///     For example, <c>"Emails/Welcome"</c>.
    /// </param>
    /// <param name="model">The view model populated from application data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<string> RenderAsync<TModel>(string templateKey, TModel model, CancellationToken cancellationToken = default);
}
