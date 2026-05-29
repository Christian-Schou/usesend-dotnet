using System.Diagnostics;
using System.Diagnostics.Metrics;
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

    internal static readonly ActivitySource ActivitySource = new("UseSend");
    internal static readonly Meter Meter = new("UseSend");
    private static readonly Counter<long> _requestCounter =
        Meter.CreateCounter<long>("usesend.client.requests", description: "Total useSend API requests.");
    private static readonly Histogram<double> _requestDuration =
        Meter.CreateHistogram<double>("usesend.client.request_duration", unit: "ms",
            description: "Duration of useSend API requests.");

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
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
        var start = Stopwatch.GetTimestamp();
        using var activity = ActivitySource.StartActivity(
            $"{req.Method.Method} {req.RequestUri?.OriginalString}", ActivityKind.Client);
        activity?.SetTag("http.method", req.Method.Method);
        activity?.SetTag("http.url", req.RequestUri?.ToString());

        HttpResponseMessage resp;

        try
        {
            resp = await Http.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Request cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            RecordMetrics(req.Method.Method, 0, start);
            var oex = new UseSendException(0, null, ex.Message, ex);
            if (Throw) throw oex;
            return new UseSendResponse(oex);
        }

        activity?.SetTag("http.status_code", (int)resp.StatusCode);
        RecordMetrics(req.Method.Method, (int)resp.StatusCode, start);

        if (!resp.IsSuccessStatusCode)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
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
        var start = Stopwatch.GetTimestamp();
        using var activity = ActivitySource.StartActivity(
            $"{req.Method.Method} {req.RequestUri?.OriginalString}", ActivityKind.Client);
        activity?.SetTag("http.method", req.Method.Method);
        activity?.SetTag("http.url", req.RequestUri?.ToString());

        HttpResponseMessage resp;

        try
        {
            resp = await Http.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Request cancelled.");
            throw;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            RecordMetrics(req.Method.Method, 0, start);
            var oex = new UseSendException(0, null, ex.Message, ex);
            if (Throw) throw oex;
            return new UseSendResponse<T2>(oex);
        }

        activity?.SetTag("http.status_code", (int)resp.StatusCode);
        RecordMetrics(req.Method.Method, (int)resp.StatusCode, start);

        if (!resp.IsSuccessStatusCode)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
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


    private static void RecordMetrics(string method, int statusCode, long startTimestamp)
    {
        var elapsed = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
        var tags = new TagList { { "http.method", method }, { "http.status_code", statusCode } };
        _requestCounter.Add(1, tags);
        _requestDuration.Record(elapsed, tags);
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