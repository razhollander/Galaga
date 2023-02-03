namespace Services.Server.Data
{
    public class PlayerTossActionRafflePayloadData : PayloadData
    {
        public override string MethodName => "tossActionRaffle";
        
        public PlayerTossActionRafflePayloadData(string matchId, string tossId, int turn)
        {
            Parameters.Add("matchId", matchId);
            Parameters.Add("tossId", tossId);
            Parameters.Add("turn", turn);
        }
    }
}