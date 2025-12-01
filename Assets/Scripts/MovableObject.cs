using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Utils;
using static DefaultNamespace.GameConst;

public class MovableObject : MonoBehaviour
{
    protected Vector3 positionLastFrame;
    protected Vector3 positionCurrentFrame;
    protected Quaternion rotationLastFrame;
    protected Quaternion rotationCurrentFrame;
    protected Vector3 velocityXYZ => (positionCurrentFrame - positionLastFrame) / Time.deltaTime;
    protected Vector2 velocity => VectorUtils.ToXZ(velocityXYZ);
    protected Quaternion direction;
    
    public List<Flag> FlagsInField { get; set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        positionLastFrame = transform.position;
        rotationLastFrame = transform.rotation;
        positionCurrentFrame = transform.position;
        rotationCurrentFrame = transform.rotation;
        direction = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        positionLastFrame = positionCurrentFrame;
        rotationLastFrame = rotationCurrentFrame;
        transform.rotation = direction;
        positionCurrentFrame = transform.position;
        rotationCurrentFrame = transform.rotation;
    }

    protected void Move(Vector2 speed)
    {
        if (Time.deltaTime == 0) return;
        
        // ( speed * Time.deltaTime + position)
        Vector2 destinationPos = speed * Time.deltaTime + VectorUtils.ToXZ(transform.position);
        if (float.IsNaN(destinationPos.x))
            Debug.Log("Move to: " + destinationPos + " x = "+destinationPos.x + " y = " + destinationPos.y);
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
            movedPos.z = StageEnd.y;
        }else if (destinationPos.y < -StageEnd.y)
        {
            movedPos.z = -StageEnd.y;
        }
        transform.position = movedPos;
    }

    protected void ForceMove(Vector2 speed)
    {
        Vector2 destinationPos = speed * Time.deltaTime + VectorUtils.ToXZ(transform.position);
        Vector3 movedPos = new Vector3(destinationPos.x, transform.position.y, destinationPos.y);
        transform.position = movedPos;
    }
}
