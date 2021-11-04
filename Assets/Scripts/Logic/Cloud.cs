using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : IChunkInterface
{
    private const float MaxOpacity = 0.7f;
    private const float PasePercentage = 0.1f;
    private BlockType[,,] blockStates;
    public Vector3 Pos;
    public Vector3Int Size {get;}
    public float AgeTime {get; private set;}
    private float lifeTime;
    private Vector3 velocity;
    public string Name {get;}

    public float Opacity {
        get{
            var phaseTime = lifeTime*PasePercentage;

            if(AgeTime<phaseTime){
                return MaxOpacity*AgeTime/phaseTime;
            }else if(AgeTime<lifeTime-phaseTime){
                return MaxOpacity;
            }else{
                var remaining = Mathf.Min(AgeTime, lifeTime)-lifeTime;

                return MaxOpacity*remaining/phaseTime;
            }
        }
    }

    public Cloud(Vector3 pos,  Vector3Int size, Vector3 vel, float life){
        Pos = pos;
        Size = size;
        velocity = vel;
        lifeTime = life;
        blockStates = new BlockType[size.x, size.y, size.z];

        Name = "cloud_"+size+"_"+life;
    }

    public void Update(){
        AgeTime += Time.deltaTime;
        Pos += velocity*Time.deltaTime;
    }

    public BlockType GetBlockType(Vector3Int pos)
    {
        if(pos.x < 0 || pos.x>=Size.x || pos.y < 0 || pos.y>=Size.y || pos.z < 0 || pos.z>=Size.z) return BlockType.Air;

        return blockStates[pos.x, pos.y, pos.z];
    }

    public void SetBlockType(Vector3Int pos, BlockType type)
    {
        blockStates[pos.x, pos.y, pos.z] = type;
    }
}
