using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    public const int VIEW_DISTANCE_CHUNK = 2; //2
    public const int SIZE = 1+VIEW_DISTANCE_CHUNK*2; //5

    public Chunk[,,] Chunks = new Chunk[SIZE,SIZE,SIZE];

    public BlockPos CenterChunkPos = new BlockPos();

    private WorldLoader loader = new WorldLoader();

    public void Init()
    {
        loader.Init();

        LoadChunks();
    }

    public bool Update(BlockPos playerPos){
        var chunkPos = PosToChunkPos(playerPos);

        if(Vector3.Distance(chunkPos.Vect, CenterChunkPos.Vect) >= Chunk.SIZE){
            CenterChunkPos = chunkPos;

            LoadChunks();

            return true;
        }

        return false;
    }

    public void LoadChunks(){        
        for(int x=0; x<SIZE; x++){
            for(int y=0; y<SIZE; y++){
                for(int z=0; z<SIZE; z++){
                    Chunks[x,y,z] = loader.LoadChunk((new BlockPos(x,y,z)-VIEW_DISTANCE_CHUNK)*Chunk.SIZE + CenterChunkPos);
                }
            }
        }
    }


    public BlockState GetBlockStates(BlockPos pos)
    {
        var chunkPos = PosToChunkPos(pos);
        var chunkIdx = chunkPos - CenterChunkPos + VIEW_DISTANCE_CHUNK;

        var blockPos = (chunkPos*Chunk.SIZE-pos).Abs();

        Debug.Log("World GetBlockStates: "+pos+ " for chunkPos: "+chunkPos+" chunkIdx:"+chunkIdx + " relBlockPos: "+blockPos);

        if(BlockPos.InRange(0, SIZE, chunkIdx)){
            return Chunks[chunkIdx.X, chunkIdx.Y, chunkIdx.Z].GetBlockStateRelative(blockPos);
        }else{
            return Blocks.Air.Default();
        }
    }

    private BlockPos PosToChunkPos(BlockPos pos){
        var vect = new Vector3Int();
        
        if(pos.X < 0){
            vect.x = 1;
        }

        if(pos.Y < 0){
            vect.y = 1;
        }

        if(pos.Z < 0){
            vect.z = 1;
        }

        return (pos-vect*Chunk.SIZE)/Chunk.SIZE+vect;
    }
}
