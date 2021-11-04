using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : IChunkInterface
{
    private BlockType[,,] blockStates;
    public Vector3 Pos;
    public Vector3 Speed {get;}
    public Vector3Int Size {get;}
    private float createdTime {get;}
    public float LifeTime {get;}

    public Cloud(Vector3 pos,  Vector3Int size, Vector3 speed, float createdTime, float lifeTime){
        Pos = pos;
        Size = size;
        Speed = speed;
        LifeTime = lifeTime;
        this.createdTime = createdTime;

        blockStates = new BlockType[size.x, size.y, size.z];
    }

    public float ElapsedSince(float time){
        return time-createdTime;
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
