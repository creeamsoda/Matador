using UnityEngine;

namespace Buffalo.Brain
{
    public class BuffaloBrainArgs
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 targetPosition;
        public BuffaloView buffaloView;
        public bool isBig;

        public BuffaloBrainArgs(Vector2 position, Vector2 velocity, Vector2 targetPosition, BuffaloView buffaloView, bool isBig = false)
        {
            this.position = position;
            this.velocity = velocity;
            this.targetPosition = targetPosition;
            this.buffaloView = buffaloView;
            this.isBig = isBig;
        }
    }
}