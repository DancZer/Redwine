using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int SIZE = 4;
    public BlockPos Pos {get;}
    public BlockPos BlockPos {get;}
    private BlockState[,,] BlockStates = new BlockState[SIZE, SIZE, SIZE];
    private World World;
    public Chunk(BlockPos pos)
    {
        Pos = pos;
        BlockPos = pos * SIZE;
    }

    public BlockState GetBlockState(BlockPos pos){
        Debug.Log("Chunk GetBlockStates: "+pos+ " for BlockPos: "+BlockPos);

        if(BlockPos.InRange(BlockPos, BlockPos+SIZE, pos)){
            var relativePos = pos - BlockPos;
            return BlockStates[relativePos.X, relativePos.Y, relativePos.Z];
        }else{
            throw new UnityException("Invalid Pos:" + pos + " for ChunkPos: "+Pos+" for BlockPos: "+BlockPos);
        }
    }

    public void SetBlockState(BlockPos pos, BlockState state){
        if(BlockPos.InRange(BlockPos, BlockPos+SIZE, pos)){
            var relativePos = pos - BlockPos;
            BlockStates[relativePos.X, relativePos.Y, relativePos.Z] = state;
        }else{
            throw new UnityException("Invalid Pos:" + pos + " for ChunkPos: "+Pos+" for BlockPos: "+BlockPos);
        }
    }
}
