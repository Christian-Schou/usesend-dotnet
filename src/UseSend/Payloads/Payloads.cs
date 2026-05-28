namespace UseSend.Payloads;

internal sealed class EmailIdPayload
{
    public string? EmailId { get; set; }
}

internal sealed class ContactIdPayload
{
    public string? ContactId { get; set; }
}

internal sealed class StringIdPayload
{
    public string? Id { get; set; }
}

internal sealed class DataList<T>
{
    public List<T> Data { get; set; } = new();
}

internal sealed class DataListWithCount<T>
{
    public List<T> Data { get; set; } = new();
    public int Count { get; set; }
}

internal sealed class MessagePayload
{
    public string? Message { get; set; }
}