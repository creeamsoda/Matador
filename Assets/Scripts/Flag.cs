using UnityEngine;

namespace DefaultNamespace
{
    public class Flag
    {
        public int index;
        public Vector3 position;
        GameObject flagObject;

        public Flag(int index, Vector3 position, GameObject flagObject)
        {
            this.index = index;
            this.position = position;
            this.flagObject = flagObject;
        }

        public void Destroy()
        {
            GameObject.Destroy(flagObject);
        }
    }
}