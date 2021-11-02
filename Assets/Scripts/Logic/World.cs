using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    public const int Size = 1+Config.WORLD_CHUNK_VIEW_DISTANCE*2;

    public Chunk[,,] Chunks = new Chunk[Size, Size, Size];

    private BlockPos CenterChunkPos;
    private BlockPos PlayerPos;

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

        if(Vector3.Distance(chunkPos.Vect, CenterChunkPos.Vect) >= Config.CHUNK_SIZE){
            CenterChunkPos = chunkPos;

            //Debug.Log("CenterChunkPos: "+CenterChunkPos);

            LoadChunks();

            return true;
        }

        return false;
    }

    public void LoadChunks()
    {        
        for(int x=0; x<Size; x++){
            for(int y=0; y<Size; y++){
                for(int z=0; z<Size; z++){
                    var pos = (new BlockPos(x,y,z)-Config.WORLD_CHUNK_VIEW_DISTANCE)*Config.CHUNK_SIZE + CenterChunkPos;
                    Chunks[x,y,z] = loader.LoadChunk(pos);
                }
            }
        }
    }


    public BlockType GetBlockStates(BlockPos pos)
    {
        var chunkPos = PosToChunkPos(pos);
        var chunkIdx = (chunkPos - CenterChunkPos)/Config.CHUNK_SIZE + Config.WORLD_CHUNK_VIEW_DISTANCE;

        var blockPos = (chunkPos-pos).Abs();

        //Debug.Log("World GetBlockStates:"+pos+ " chunkPos:"+chunkPos+" CenterChunkPos: "+CenterChunkPos+" chunkIdx:"+chunkIdx + " relBlockPos: "+blockPos);

        if(BlockPos.InRange(0, Size, chunkIdx)){
            return Chunks[chunkIdx.X, chunkIdx.Y, chunkIdx.Z].GetBlockState(blockPos);
        }else{
            //Debug.Log("Over visibility:"+pos);
            return BlockType.Air;
        }
    }

    private BlockPos PosToChunkPos(BlockPos pos)
    {
        return new BlockPos(RoundToChunkSize(pos.X), RoundToChunkSize(pos.Y), RoundToChunkSize(pos.Z));
    }

    private int RoundToChunkSize(int a){
        return (int)Mathf.Floor((float)a/(float)Config.CHUNK_SIZE)*Config.CHUNK_SIZE;
    }
}
