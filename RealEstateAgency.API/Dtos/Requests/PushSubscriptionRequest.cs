namespace RealEstateAgency.API.Dtos.Requests;

public class PushSubscriptionRequest
{
    public required string Endpoint { get; set; }
    public PushSubscriptionKeys? Keys { get; set; }

    public class PushSubscriptionKeys
    {
        public required string P256dh { get; set; }
        public required string Auth { get; set; }
    }
}