using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiner : MonoBehaviour
{
    public LayerMask groundLayer;

    private float maxDist = 4;

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
                if(rightClick){
                    pointInTargetBlock = hitInfo.point + transform.forward * .1f;
                }else{
                    pointInTargetBlock = hitInfo.point - transform.forward * .1f;
                }

                var blockPos = Vector3Int.FloorToInt(pointInTargetBlock);

                var head = Vector3Int.FloorToInt(transform.position);
                var foot = head.Below();
                
                //Debug.Log("blockPos:"+blockPos+" head:"+head+" foot:"+foot);

                if(blockPos != head && blockPos != foot){
                    if(rightClick){
                        World.SetBlockType(blockPos, BlockType.Air);
                    }else{
                        World.SetBlockType(blockPos, BlockType.Dirt);
                    }
                }
            }
        }
    }
}
