using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int Size = Config.CHUNK_SIZE+2;
    public string Name {get;}
    public Vector3Int Pos {get;}
    private BlockType[,,] blockStates;
    public Chunk(Vector3Int pos)
    {
        Pos = pos;
        Name=$"Chunk_{pos.x}_{pos.y}_{pos.z}";
        blockStates = new BlockType[Size, Size, Size];
    }

    public BlockType GetBlockState(Vector3Int pos)
    {
        //Debug.Log("GetBlockState:"+pos);
        return GetBlockState(pos.x, pos.y, pos.z);
    }

    public BlockType GetBlockState(int x, int y, int z)
    {
        return blockStates[x+1, y+1, z+1];
    }

    public void SetBlockState(int x, int y, int z, BlockType type){
        blockStates[x+1, y+1, z+1] = type;
    }

    public override string ToString()
    {
        return Name;
    }
}
