using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int Size = Config.CHUNK_SIZE+2;
    public string Name {get;}
    public BlockPos Pos {get;}
    private BlockType[,,] blockStates;
    public Chunk(BlockPos pos)
    {
        Pos = pos;
        Name=$"Chunk_{pos.X}_{pos.Y}_{pos.Z}";
        blockStates = new BlockType[Size, Size, Size];
    }

    public BlockType GetBlockState(BlockPos pos)
    {
        //Debug.Log("GetBlockState:"+pos);
        return GetBlockState(pos.X, pos.Y, pos.Z);
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
