using UnityEngine;

namespace Buffalo.Brain
{
    public class BuffaloBrainArgs
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 targetPosition;

        public BuffaloBrainArgs(Vector2 position, Vector2 velocity, Vector2 targetPosition)
        {
            this.position = position;
            this.velocity = velocity;
            this.targetPosition = targetPosition;
        }
    }
}