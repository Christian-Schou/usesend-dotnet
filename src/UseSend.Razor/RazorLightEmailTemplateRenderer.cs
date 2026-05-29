using RazorLight;

namespace UseSend.Razor;

/// <summary>
///     RazorLight-backed implementation of <see cref="IEmailTemplateRenderer" />.
/// </summary>
public sealed class RazorLightEmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorLightEmailTemplateRenderer(EmailTemplateOptions options)
    {
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(options.TemplateRootPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    /// <inheritdoc />
    public Task<string> RenderAsync<TModel>(
        string templateKey,
        TModel model,
        CancellationToken cancellationToken = default)
    {
        // RazorLight does not accept a CancellationToken natively,
        // but we honour cancellation before handing off.
        cancellationToken.ThrowIfCancellationRequested();
        return _engine.CompileRenderAsync(templateKey, model);
    }
}
