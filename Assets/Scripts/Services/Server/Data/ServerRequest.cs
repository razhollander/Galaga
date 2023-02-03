namespace Services.Server.Data
{
    public class ServerRequest
    {
        public PayloadData PayloadData { get; }

        public ServerRequest(PayloadData payloadData)
        {
            PayloadData = payloadData;
        }
    }
}