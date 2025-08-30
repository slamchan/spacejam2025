using UnityEngine;

public class AtWorldEnd : MonoBehaviour
{
    [Tooltip("Half the width of the looping world (centered at 0).")]
    public float halfWidth = 10f;

    [Tooltip("Half the height if vertical looping too. Set 0 for none.")]
    public float halfHeight = 0f;

    Transform target;

    void Start()
    {
        target = transform; // this script on the player
    }

    void LateUpdate()
    {
        Vector3 pos = target.position;

        // Horizontal wrap
        if (pos.x > halfWidth)
            pos.x = -halfWidth;
        else if (pos.x < -halfWidth)
            pos.x = halfWidth;

        // Optional vertical wrap
        if (halfHeight > 0f)
        {
            if (pos.y > halfHeight)
                pos.y = -halfHeight;
            else if (pos.y < -halfHeight)
                pos.y = halfHeight;
        }

        target.position = pos;
    }
}
