using UseSend.Razor.Tests.Models;

namespace UseSend.Razor.Tests;

public sealed class RazorLightEmailTemplateRendererTests
{
    private static string TemplatesPath =>
        Path.Combine(AppContext.BaseDirectory, "Templates");

    private static IEmailTemplateRenderer BuildRenderer() =>
        new RazorLightEmailTemplateRenderer(new EmailTemplateOptions { TemplateRootPath = TemplatesPath });

    [Fact]
    public async Task RenderAsync_ReturnsHtmlContainingModelValues()
    {
        var renderer = BuildRenderer();
        var model = new WelcomeModel { Name = "Alice", ConfirmUrl = "https://example.com/confirm" };

        var html = await renderer.RenderAsync("Welcome", model);

        Assert.Contains("Welcome, Alice!", html);
        Assert.Contains("https://example.com/confirm", html);
    }

    [Fact]
    public async Task RenderAsync_HonoursCancellation()
    {
        var renderer = BuildRenderer();
        var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            renderer.RenderAsync("Welcome", new WelcomeModel(), cts.Token));
    }

    [Fact]
    public async Task RenderAsync_WithDifferentModel_RendersCorrectly()
    {
        var renderer = BuildRenderer();
        var html = await renderer.RenderAsync("Welcome",
            new WelcomeModel { Name = "Bob", ConfirmUrl = "https://x.com" });

        Assert.Contains("Welcome, Bob!", html);
        Assert.Contains("https://x.com", html);
    }

    [Fact]
    public async Task RenderAsync_ContainsHtmlStructure()
    {
        var renderer = BuildRenderer();
        var html = await renderer.RenderAsync("Welcome",
            new WelcomeModel { Name = "Charlie", ConfirmUrl = "https://confirm.example.com" });

        Assert.Contains("<h1>", html);
        Assert.Contains("Charlie", html);
    }
}
