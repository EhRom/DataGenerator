namespace DataGenerator.Domain.Claim;

public class ClaimData : Data, IData
{
    private const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss";
    public string CustomerId { get; init; } = string.Empty;

    public string FileId { get; init; } = string.Empty;

    public ClaimType ClaimType { get; init; }

    public ClaimPriority ClaimPriority { get; init; }

    public DateTime ClaimCreateDate { get; init; }

    public DateTime ClaimCloseDate { get; init; }

    public ClaimData(DateOnly date, bool isHoliday, string holidayName, string customerId, string fileId, ClaimType claimType, ClaimPriority claimPriority, DateTime claimCreateDate, DateTime claimCloseDate)
        : base(date, isHoliday, holidayName)
    {
        CustomerId = customerId;
        FileId = fileId;
        ClaimType = claimType;
        ClaimPriority = claimPriority;
        ClaimCreateDate = claimCreateDate;
        ClaimCloseDate = claimCloseDate;
    }

    public static ClaimData CreateNew(DateOnly date, bool isHoliday, string holidayName, string customerId, string fileId, ClaimType claimType, ClaimPriority claimPriority, DateTime claimCreateDate, DateTime claimCloseDate)
    {
        return new ClaimData(date, isHoliday, holidayName, customerId, fileId, claimType, claimPriority, claimCreateDate, claimCloseDate);
    }

    public override string GetHeader(char separator)
    {
        string[] headers = new string[]
        {
            nameof(CustomerId),
            nameof(FileId),
            nameof(ClaimType),
            nameof(ClaimPriority),
            nameof(ClaimCreateDate),
            nameof(ClaimCloseDate)
        };

        return string.Join(separator, headers);
    }

    public override string GetContent(char separator)
    {
        string[] contents = new string[]
        {
            CustomerId,
            FileId,
            ClaimType.ToString(),
            ClaimPriority.ToString(),
            ClaimCreateDate.ToString(DATE_FORMAT),
            ClaimCloseDate.ToString(DATE_FORMAT)
        };

        return string.Join(separator, contents);
    }
}