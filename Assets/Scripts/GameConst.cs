using UnityEngine;

namespace DefaultNamespace
{
    public static class GameConst
    {
        public static readonly float PlayDuration = 120f;
        public static readonly Vector2 StageEnd = new Vector2(30f, 15f);

        public static readonly int BuffaloRightFreeTime = 20;
        public static readonly int BuffaloLeftFreeTime = 40;
        public static readonly int BuffaloCenterFreeTime = 60;
        public static readonly int EndTime = 120;
        
        public static readonly float PlayerSpeed = 5f;
        public static readonly float PlayerRotateSpeed = 360f;
        public static readonly float DodgeSpeed = 20f;
        public static readonly float DodgeDurationMax = 0.1f;
        public static readonly float DodgeClothThreshold = 0.2f;
        public static readonly float DodgeStunRate = 10f;
        public static readonly float DodgeStunEndThreshold = 0.2f;
        public static readonly float KnockBackDeceleration = 0.9f;
        public static readonly float DodgeBonusCoolTime = 1.5f;
        public static readonly float DodgeBonus = 1.5f;
        
        public static readonly float FragGetableDistance = 2f;
        public static readonly float BuffaloSightRange = 30f;
        public static readonly float BuffaloSightAngle = 45f;
        public static readonly float BuffaloWalkSpeed = 6f;
        public static readonly float BuffaloWalkLerpRate = 2f;
        public static readonly float BuffaloTackleAcceleration = 15f;
        public static readonly float BuffaloTackleStaySpeed = 0.1f;
        public static readonly float BuffaloTackleStayLerpRate = 4f;
        public static readonly float SpeedUpPerCombos = 1f;
        public static readonly float BuffaloTackleDurationMax = 2f;
        public static readonly float BuffaloTackleStartAngle = 2f;
        public static readonly float BuffaloTackleLerpRate = 10f;
        public static readonly float BuffaloTackleOverRunAngle = 90f;
        public static readonly float BuffaloTackleOverRunStartDistance = 2f;
        public static readonly float BuffaloTackleOverRunEndDistanceAgainstWall = 10f;
        public static readonly float BuffaloRapidDecelerationRate = 0.4f;
        public static readonly float BuffaloRapidDecelerationEndSpeed = BuffaloWalkSpeed;
        public static readonly Vector2 BuffaloWalkArea = new Vector2(20f, 5f);
        public static readonly float BuffaloRotateSpeed = 180f;
        public static readonly float BuffaloAttackPower = 100f;
        public static readonly float BuffaloAttackAngle = 30f;
        public static readonly float BuffaloAttackRadius = 2f;
        public static readonly float BigBuffaloBodyLength = 10f;
        public static readonly float BigBuffaloBodyWidth = 5f;
        
        public static readonly float BuffaloViewWalkMaxSpeedThreshold = BuffaloTackleAcceleration * 0.8f;
        public static readonly float BuffaloViewWalkMinSpeedThreshold = 0.3f;
        public static readonly float BuffaloViewNeckMaxAngle = 60f;
        
        public static readonly float scoreIncreaseForFlag = 100f;
        public static readonly float scoreIncreaseForCombo = 10f;
        public static readonly float AttackedScoreDown = -500f;
        public static readonly float DodgeScoreUpInterval = 0.5f;
        public static readonly float DodgeScoreUpBigAngle = 45f;
        public static readonly float DodgeScoreUpGigRadius = 7f;
        public static readonly float DodgeScoreUpGigValue = 100f;
        public static readonly float DodgeScoreUpSmallAngle = 30f;
        public static readonly float DodgeScoreUpSmallRadius = 20f;
        public static readonly float DodgeScoreUpSmallValue = 10f;
    }
}