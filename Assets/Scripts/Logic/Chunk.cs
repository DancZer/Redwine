using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int SIZE = Config.CHUNK_SIZE;
    public BlockPos Pos {get;}
    public string Name {get;}
    private BlockState[,,] BlockStates = new BlockState[SIZE, SIZE, SIZE];
    private World World;
    public Chunk(BlockPos pos)
    {
        Pos = pos;
        Name=$"Chunk_{pos.X}_{pos.Y}_{pos.Z}";
    }

    public BlockState GetBlockStateRelative(BlockPos relativePos){
        if(BlockPos.InRange(0, SIZE, relativePos)){
            var state = BlockStates[relativePos.X, relativePos.Y, relativePos.Z];

            //Debug.Log("Get Chunk BlockState ChunkPos:"+Pos+" relativePos:"+relativePos+" state:"+state);

            return state;
        }else{
            throw new UnityException("Invalid GetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));
        }
    }

    public void SetBlockState(BlockPos relativePos, BlockState state){
        if(BlockPos.InRange(0, SIZE, relativePos)){   
            //Debug.Log("Set Chunk BlockState ChunkPos:"+Pos+" relativePos:"+relativePos+" state:"+state);

            BlockStates[relativePos.X, relativePos.Y, relativePos.Z] = state;
        }else{
            throw new UnityException("Invalid SetPos:" + relativePos + " for ChunkPos: "+Pos+" for ChunkPosEnd: "+(Pos+SIZE));
        }
    }
}
