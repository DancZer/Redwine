using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    private const int VIEW_DISTANCE_BLOCK = 8;
    public const int VIEW_DISTANCE_CHUNK = VIEW_DISTANCE_BLOCK/Chunk.SIZE; //2
    public const int SIZE = 1+VIEW_DISTANCE_CHUNK*2; //5

    public Chunk[,,] Chunks = new Chunk[SIZE,SIZE,SIZE];

    public Vector3Int PlayerPos = new Vector3Int();

    private WorldLoader loader = new WorldLoader();

    public void Init()
    {
        for(int x=0; x<SIZE; x++){
            for(int y=0; y<SIZE; y++){
                for(int z=0; z<SIZE; z++){
                    Chunks[x,y,z] = loader.LoadChunk(new BlockPos(x,y,z)-VIEW_DISTANCE_CHUNK);
                }
            }
        }
    }

    public BlockState GetBlockStates(BlockPos pos)
    {
        var relativePos = pos-PlayerPos;
        //Debug.Log("World GetBlockStates: "+pos+ " for relativePos: "+relativePos + " viewDist: "+VIEW_DISTANCE_BLOCK);

        if(BlockPos.InRange(-VIEW_DISTANCE_BLOCK, VIEW_DISTANCE_BLOCK, relativePos)){
            return GetChunk(relativePos).GetBlockState(pos);
        }else{
            return Blocks.Air.Default();
        }
    }

    private Chunk GetChunk(BlockPos relativePos){
        var chunkIdx = (relativePos+VIEW_DISTANCE_BLOCK)/Chunk.SIZE;

        //Debug.Log("Selected ChunkIdx: "+chunkIdx+ " for relativePos: "+relativePos);

        return Chunks[chunkIdx.X, chunkIdx.Y, chunkIdx.Z];
    }
}
