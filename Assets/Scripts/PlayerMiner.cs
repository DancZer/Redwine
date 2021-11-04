using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModi : MonoBehaviour
{
    public LayerMask groundLayer;

    float maxDist = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if(leftClick || rightClick)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDist, groundLayer))
            {
                Vector3 pointInTargetBlock;

                //destroy
                if(rightClick)
                    pointInTargetBlock = hitInfo.point + transform.forward * .01f;
                else
                    pointInTargetBlock = hitInfo.point - transform.forward * .01f;

                var blockPos = Vector3Int.FloorToInt(pointInTargetBlock);

                var chunk = World.GetChunk(blockPos);
                var blockPosRel = blockPos - chunk.Pos;
            }
        }
    }
}
