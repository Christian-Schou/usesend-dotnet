namespace UseSend.Fluid;

/// <summary>
///     Configuration options for the Fluid (Liquid) email template renderer.
/// </summary>
public sealed class FluidEmailTemplateOptions
{
    /// <summary>
    ///     Absolute path to the directory that contains the <c>.liquid</c> template files.
    ///     Defaults to a sub-folder named <c>Templates</c> under the current working directory.
    /// </summary>
    public string TemplateRootPath { get; set; } =
        Path.Combine(Directory.GetCurrentDirectory(), "Templates");
}
