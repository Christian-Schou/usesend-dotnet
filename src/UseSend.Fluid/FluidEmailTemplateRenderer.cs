using global::Fluid;
using System.Collections.Concurrent;

namespace UseSend.Fluid;

/// <summary>
///     Fluid (Liquid) backed implementation of <see cref="IEmailTemplateRenderer" />.
///     Parses and caches <c>.liquid</c> templates on first use; thread-safe.
/// </summary>
public sealed class FluidEmailTemplateRenderer : IEmailTemplateRenderer
{
    private static readonly FluidParser Parser = new();
    private readonly ConcurrentDictionary<string, IFluidTemplate> _cache = new();
    private readonly FluidEmailTemplateOptions _options;

    /// <summary>
    ///     Initialises a new instance using the provided <paramref name="options" />.
    /// </summary>
    public FluidEmailTemplateRenderer(FluidEmailTemplateOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<string> RenderAsync<TModel>(
        string templateKey,
        TModel model,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var template = _cache.GetOrAdd(templateKey, key =>
        {
            var path = Path.Combine(_options.TemplateRootPath, key + ".liquid");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Liquid template '{key}' not found at: {path}", path);

            var source = File.ReadAllText(path);

            if (!Parser.TryParse(source, out var parsed, out var error))
                throw new InvalidOperationException(
                    $"Failed to parse Liquid template '{key}': {error}");

            return parsed;
        });

        // TemplateContext is NOT thread-safe — create one per render.
        var templateOptions = new TemplateOptions();
        if (model is not null)
            templateOptions.MemberAccessStrategy.Register(model.GetType());

        var context = new TemplateContext(model, templateOptions);
        return await template.RenderAsync(context).ConfigureAwait(false);
    }
}
