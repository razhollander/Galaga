namespace Services.Server.Data
{
    public class CreateSessionPayloadData : PayloadData
    {
        public override string MethodName => "createSession";
        
        public CreateSessionPayloadData(string loginToken)
        {
            Parameters.Add("loginToken", loginToken);
        }
    }
}