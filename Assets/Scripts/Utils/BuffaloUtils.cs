using DefaultNamespace;
using UnityEngine;

namespace Utils
{
    public class BuffaloUtils
    {
        public static float GetFrontSqrDistanceFromStageEnd(Vector2 position, Vector2 forward)
        {
            // forward方向のかべとの交点を調べる
            Vector2 normalisedForward = forward.normalized;
            float xDistance = float.PositiveInfinity;
            float yDistance = float.PositiveInfinity;
            if(Mathf.Abs(normalisedForward.x) > 0.001f)
                xDistance = (Mathf.Sign(normalisedForward.x) * GameConst.StageEnd.x - position.x) / normalisedForward.x;
            if(Mathf.Abs(normalisedForward.y) > 0.001f)
                yDistance = (Mathf.Sign(normalisedForward.y) * GameConst.StageEnd.y - position.y) / normalisedForward.y;
            Vector2 intersectionBetweenForwardAndXStageEnd = float.IsInfinity(xDistance) ? Vector2.positiveInfinity : xDistance * normalisedForward + position;
            Vector2 intersectionBetweenForwardAndYStageEnd = float.IsInfinity(yDistance) ? Vector2.positiveInfinity : yDistance * normalisedForward + position;
            if (!float.IsInfinity(intersectionBetweenForwardAndXStageEnd.x) && Mathf.Abs(intersectionBetweenForwardAndXStageEnd.y) < GameConst.StageEnd.y)
                return VectorUtils.SqrDistance(intersectionBetweenForwardAndXStageEnd, position);
            return VectorUtils.SqrDistance(intersectionBetweenForwardAndYStageEnd, position);
        }

        public static bool IsPlayerFindable(Vector2 position, Vector2 forward, Vector2 playerPosition)
        {
            if (VectorUtils.SqrDistance(position, playerPosition) < GameConst.BuffaloSightRange * GameConst.BuffaloSightRange
                && Vector2.Angle(forward, (playerPosition - position)) < GameConst.BuffaloSightAngle)
                return true;
            return false;
        }

        public static bool IsPlayerNearHead(Vector2 position, Vector2 forward, Vector2 playerPosition)
        {
            return VectorUtils.SqrDistance(position, playerPosition) < GameConst.DodgeScoreUpSmallRadius * GameConst.DodgeScoreUpSmallRadius
                   && Vector2.Angle(forward, (playerPosition - position)) < GameConst.DodgeScoreUpSmallAngle;
        }
    }
}