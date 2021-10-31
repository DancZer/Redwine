using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    public const int VIEW_DISTANCE_CHUNK = Config.WORLD_CHUNK_VIEW_DISTANCE; //2
    public const int SIZE = 1+VIEW_DISTANCE_CHUNK*2; //3

    public Chunk[,,] Chunks = new Chunk[SIZE, SIZE, SIZE];

    private BlockPos CenterChunkPos = new BlockPos();
    private BlockPos PlayerPos = new BlockPos();

    private WorldLoader loader = new WorldLoader();

    private static World singleton;

    public static World Instance(){
        if(singleton == null){
            singleton = new World();
        }

        return singleton;
    }

    public void Init()
    {
        loader.Init();

        LoadChunks();
    }

    public bool Update(BlockPos playerPos)
    {
        var chunkPos = PosToChunkPos(playerPos);

        if(Vector3.Distance(chunkPos.Vect, CenterChunkPos.Vect) >= Chunk.SIZE){
            CenterChunkPos = chunkPos;

            //Debug.Log("CenterChunkPos: "+CenterChunkPos);

            LoadChunks();

            return true;
        }

        return false;
    }

    public void LoadChunks()
    {        
        for(int x=0; x<SIZE; x++){
            for(int y=0; y<SIZE; y++){
                for(int z=0; z<SIZE; z++){
                    var pos = (new BlockPos(x,y,z)-VIEW_DISTANCE_CHUNK)*Chunk.SIZE + CenterChunkPos;
                    Chunks[x,y,z] = loader.LoadChunk(pos);
                }
            }
        }
    }


    public BlockState GetBlockStates(BlockPos pos)
    {
        var chunkPos = PosToChunkPos(pos);
        var chunkIdx = (chunkPos - CenterChunkPos)/Chunk.SIZE + VIEW_DISTANCE_CHUNK;

        var blockPos = (chunkPos-pos).Abs();

        //Debug.Log("World GetBlockStates:"+pos+ " chunkPos:"+chunkPos+" CenterChunkPos: "+CenterChunkPos+" chunkIdx:"+chunkIdx + " relBlockPos: "+blockPos);

        if(BlockPos.InRange(0, SIZE, chunkIdx)){
            return Chunks[chunkIdx.X, chunkIdx.Y, chunkIdx.Z].GetBlockStateRelative(blockPos);
        }else{
            //Debug.Log("Over visibility:"+pos);
            return Blocks.Air.Default();
        }
    }

    private BlockPos PosToChunkPos(BlockPos pos)
    {
        return new BlockPos(RoundToChunkSize(pos.X), RoundToChunkSize(pos.Y), RoundToChunkSize(pos.Z));
    }

    private int RoundToChunkSize(int a){
        return (int)Mathf.Floor((float)a/(float)Chunk.SIZE)*Chunk.SIZE;
    }
}
