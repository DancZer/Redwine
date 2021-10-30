using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int SIZE = 2;
    public BlockPos Pos {get;}
    private BlockState[,,] BlockStates = new BlockState[SIZE, SIZE, SIZE];
    private World World;
    public Chunk(BlockPos pos)
    {
        Pos = pos;
    }

    public BlockState GetBlockStateRelative(BlockPos relativePos){
        //Debug.Log("GetBlockState GetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));

        if(BlockPos.InRange(0, SIZE, relativePos)){
            return BlockStates[relativePos.X, relativePos.Y, relativePos.Z];
        }else{
            throw new UnityException("Invalid GetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));
        }
    }

    public void SetBlockState(BlockPos relativePos, BlockState state){
        //Debug.Log("SetBlockState SetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));

        if(BlockPos.InRange(0, SIZE, relativePos)){   
            BlockStates[relativePos.X, relativePos.Y, relativePos.Z] = state;
        }else{
            throw new UnityException("Invalid SetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));
        }
    }
}
