namespace UseSend.Tests;

public class UseSendClientCreateTests
{
    [Fact]
    public void Create_WithApiToken_Succeeds()
    {
        var client = UseSendClient.Create("us_test_123");

        Assert.NotNull(client);
    }


    [Fact]
    public void Create_NoArg_WithEnvVar_Succeeds()
    {
        Environment.SetEnvironmentVariable("USESEND_API_KEY", "us_env_123");

        try
        {
            var client = UseSendClient.Create();

            Assert.NotNull(client);
        }
        finally
        {
            Environment.SetEnvironmentVariable("USESEND_API_KEY", null);
        }
    }


    [Fact]
    public void Create_WithOptions_NoToken_EnvVarFallback_Succeeds()
    {
        Environment.SetEnvironmentVariable("USESEND_API_KEY", "us_env_123");

        try
        {
            var client = UseSendClient.Create(new UseSendClientOptions());

            Assert.NotNull(client);
        }
        finally
        {
            Environment.SetEnvironmentVariable("USESEND_API_KEY", null);
        }
    }


    [Fact]
    public void Create_NoArg_NoEnvVar_ThrowsInvalidOperationException()
    {
        Environment.SetEnvironmentVariable("USESEND_API_KEY", null);

        var ex = Assert.Throws<InvalidOperationException>(() => UseSendClient.Create());

        Assert.Contains("USESEND_API_KEY", ex.Message);
    }


    [Fact]
    public void Create_WithCustomApiUrl_SetsBaseAddress()
    {
        var client = UseSendClient.Create(new UseSendClientOptions
        {
            ApiToken = "us_test_123",
            ApiUrl = "https://send.mycompany.com/api/"
        });

        Assert.NotNull(client);
    }
}