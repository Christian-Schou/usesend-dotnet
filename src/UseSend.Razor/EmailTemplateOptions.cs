using System.ComponentModel.DataAnnotations;

namespace UseSend.Razor;

/// <summary>
///     Configuration options for the Razor email template renderer.
/// </summary>
public sealed class EmailTemplateOptions
{
    /// <summary>
    ///     Absolute path to the directory that contains the .cshtml template files.
    ///     Defaults to a sub-folder named <c>Templates</c> under the current working directory.
    /// </summary>
    [Required]
    public string TemplateRootPath { get; set; } =
        Path.Combine(Directory.GetCurrentDirectory(), "Templates");
}
