using System.IO;

namespace UseSend.Fluid.Tests;

public sealed class FluidEmailTemplateRendererTests
{
    private static string TemplateRoot => Path.Combine(
        AppContext.BaseDirectory, "Templates");

    private static FluidEmailTemplateRenderer CreateRenderer() =>
        new(new FluidEmailTemplateOptions { TemplateRootPath = TemplateRoot });

    [Fact]
    public async Task RenderAsync_ReplacesModelProperties()
    {
        var renderer = CreateRenderer();
        var html = await renderer.RenderAsync("Welcome", new
        {
            Name       = "Alice",
            ConfirmUrl = "https://example.com/confirm",
            HasPromo   = false,
            PromoCode  = (string?)null
        });

        Assert.Contains("Alice", html);
        Assert.Contains("https://example.com/confirm", html);
    }

    [Fact]
    public async Task RenderAsync_RendersConditionalBlock_WhenTrue()
    {
        var renderer = CreateRenderer();
        var html = await renderer.RenderAsync("Welcome", new
        {
            Name       = "Bob",
            ConfirmUrl = "https://example.com/confirm",
            HasPromo   = true,
            PromoCode  = "SAVE10"
        });

        Assert.Contains("SAVE10", html);
    }

    [Fact]
    public async Task RenderAsync_OmitsConditionalBlock_WhenFalse()
    {
        var renderer = CreateRenderer();
        var html = await renderer.RenderAsync("Welcome", new
        {
            Name       = "Carol",
            ConfirmUrl = "https://example.com/confirm",
            HasPromo   = false,
            PromoCode  = (string?)null
        });

        Assert.DoesNotContain("PromoCode", html);
        Assert.DoesNotContain("20% off", html);
    }

    [Fact]
    public async Task RenderAsync_ThrowsFileNotFoundException_ForMissingTemplate()
    {
        var renderer = CreateRenderer();
        await Assert.ThrowsAsync<FileNotFoundException>(
            () => renderer.RenderAsync<object>("NonExistent", new()));
    }
}
