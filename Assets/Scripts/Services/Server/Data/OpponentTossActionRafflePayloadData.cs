namespace Services.Server.Data
{
    public class OpponentTossActionRafflePayloadData : PayloadData
    {
        public override string MethodName => "getMatchUpdates";
        
        public OpponentTossActionRafflePayloadData(string matchId, int turn)
        {
            Parameters.Add("matchId", matchId);
            Parameters.Add("turn", turn);
        }
    }
}