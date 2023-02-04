namespace Services.Server.Data
{
    public class LoginWithDeviceTokenPayloadData : PayloadData
    {
        public override string MethodName => "loginWithDeviceToken";
        
        public LoginWithDeviceTokenPayloadData(string deviceToken)
        {
            Parameters.Add("deviceToken", deviceToken);
        }
    }
}