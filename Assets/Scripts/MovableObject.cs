using UnityEngine;
using Utils;
using static DefaultNamespace.GameConst;

public class MovableObject : MonoBehaviour
{
    private Vector3 positionLastFrame;
    private Quaternion rotationLastFrame;
    private Vector3 velocity => (this.transform.position - positionLastFrame) / Time.deltaTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positionLastFrame = transform.position;
        rotationLastFrame = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Move(Vector2 speed)
    {
        Vector2 destinationPos = speed * Time.deltaTime + VectorUtils.ToXZ(transform.position);
        Vector3 movedPos = new Vector3(destinationPos.x, transform.position.y, destinationPos.y);
        
        if (StageEnd.x < destinationPos.x)
        {
            movedPos.x = StageEnd.x;
        }else if (destinationPos.x < -StageEnd.x)
        {
            movedPos.x = -StageEnd.x;
        }

        if (StageEnd.y < destinationPos.y)
        {
            movedPos.y = StageEnd.y;
        }else if (destinationPos.y < -StageEnd.y)
        {
            movedPos.y = -StageEnd.y;
        }
        transform.position = movedPos;
    }
}
