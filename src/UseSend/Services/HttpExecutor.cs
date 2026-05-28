using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UseSend;

/// <summary>
///     Shared HTTP execution infrastructure used by all service implementations.
/// </summary>
internal sealed class HttpExecutor
{
    internal const string IdempotencyKeyHeader = "Idempotency-Key";

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };

    internal readonly HttpClient Http;
    internal readonly bool Throw;


    internal HttpExecutor(HttpClient http, bool throwExceptions)
    {
        Http = http;
        Throw = throwExceptions;
    }


    internal async Task<UseSendResponse> Execute(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        HttpResponseMessage resp;

        try
        {
            resp = await Http.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var oex = new UseSendException(0, null, ex.Message, ex);

            if (Throw) throw oex;
            return new UseSendResponse(oex);
        }

        if (!resp.IsSuccessStatusCode)
        {
            var oex = await BuildException(resp, cancellationToken).ConfigureAwait(false);

            if (Throw) throw oex;
            return new UseSendResponse(oex);
        }

        return new UseSendResponse();
    }


    internal async Task<UseSendResponse<T2>> Execute<T1, T2>(
        HttpRequestMessage req,
        Func<T1, T2> map,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage resp;

        try
        {
            resp = await Http.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var oex = new UseSendException(0, null, ex.Message, ex);

            if (Throw) throw oex;
            return new UseSendResponse<T2>(oex);
        }

        if (!resp.IsSuccessStatusCode)
        {
            var oex = await BuildException(resp, cancellationToken).ConfigureAwait(false);

            if (Throw) throw oex;
            return new UseSendResponse<T2>(oex);
        }

        T1? obj;

        try
        {
            obj = await resp.Content.ReadFromJsonAsync<T1>(JsonOptions, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var oex = new UseSendException((int)resp.StatusCode, null, "Failed deserializing response: " + ex.Message,
                ex);

            if (Throw) throw oex;
            return new UseSendResponse<T2>(oex);
        }

        if (obj == null)
        {
            var oex = new UseSendException((int)resp.StatusCode, null, "Empty response body.");

            if (Throw) throw oex;
            return new UseSendResponse<T2>(oex);
        }

        return new UseSendResponse<T2>(map(obj));
    }


    private async Task<UseSendException> BuildException(HttpResponseMessage resp, CancellationToken cancellationToken)
    {
        string? apiError = null;

        try
        {
            var errObj = await resp.Content.ReadFromJsonAsync<ErrorPayload>(JsonOptions, cancellationToken)
                .ConfigureAwait(false);
            apiError = errObj?.Error ?? errObj?.Message;
        }
        catch
        {
            // best-effort
        }

        return new UseSendException(
            (int)resp.StatusCode,
            apiError,
            $"useSend API returned HTTP {(int)resp.StatusCode}: {apiError ?? resp.ReasonPhrase}");
    }


    private sealed class ErrorPayload
    {
        public string? Error { get; set; }
        public string? Message { get; set; }
    }
}