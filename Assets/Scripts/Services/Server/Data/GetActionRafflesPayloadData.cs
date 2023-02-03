using UnityEngine;

namespace Services.Server.Data
{
    public class GetActionRafflesPayloadData : PayloadData
    {
        public override string MethodName => "getActionRaffles";
        
        public GetActionRafflesPayloadData(string matchId, Vector2Int location)
        {
            Parameters.Add("matchId", matchId);
            Parameters.Add("x", location.x);
            Parameters.Add("y", location.y);
        }
    }
}