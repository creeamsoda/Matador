using UnityEngine;

namespace DefaultNamespace
{
    public static class GameConst
    {
        public static readonly Vector2 StageEnd = new Vector2(30f, 15f);
        
        public static readonly float PlayerSpeed = 5f;
        public static readonly float PlayerRotateSpeed = 360f;
        public static readonly float DodgeSpeed = 5f;
        public static readonly float DodgeDurationMax = 0.3f;
        public static readonly float DodgeClothThreshold = 0.2f;
        
        public static readonly float FragGetableDistance = 2f;
        public static readonly float BuffaloWalkSpeed = 6f;
        public static readonly float BuffaloTackleSpeed = 15f;
        public static readonly float SpeedUpPerCombos = 1f;
        public static readonly float BuffaloTackleDurationMax = 2f;
        public static readonly Vector2 BuffaloWalkArea = new Vector2(20f, 5f);
        public static readonly float BuffaloRotateSpeed = 180f;
        
        public static readonly float scoreIncreaseForFlag = 100f;
        public static readonly float scoreIncreaseForCombo = 10f;
    }
}