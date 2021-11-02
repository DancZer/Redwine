using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is more like the player area, not the whole world. functionality should be split
public class World
{
    public const int Size = 1+Config.WORLD_CHUNK_VIEW_DISTANCE*2;

    private Dictionary<Vector3Int, Chunk> chunks;
    private Vector3Int centerChunkPos;
    private Vector3Int moveDir;
    private WorldLoader loader = new WorldLoader();

    public void Start()
    {
        loader.Init();

        LoadChunks();
    }

    public bool Update(Vector3Int playerPos)
    {
        var chunkPos = PosToChunkPos(playerPos);

        if(Vector3.Distance(chunkPos, centerChunkPos) >= Config.CHUNK_SIZE){
            moveDir = (chunkPos-centerChunkPos)/Config.CHUNK_SIZE;

            centerChunkPos = chunkPos;

            //Debug.Log("MoveDir: "+moveDir);

            LoadChunks();

            return true;
        }

        return false;
    }

    public void LoadChunks(bool clean = false)
    {        
        var chunks = new Dictionary<Vector3Int, Chunk>();

        for(int x=0; x<Size; x++){
            for(int y=0; y<Size; y++){
                for(int z=0; z<Size; z++){
                    var pos = (new Vector3Int(x,y,z)-new Vector3Int(Config.WORLD_CHUNK_VIEW_DISTANCE, Config.WORLD_CHUNK_VIEW_DISTANCE, Config.WORLD_CHUNK_VIEW_DISTANCE))*Config.CHUNK_SIZE + centerChunkPos;
                    
                    Chunk chunk;

                    if(this.chunks == null || !this.chunks.TryGetValue(pos, out chunk)){
                        chunk = loader.LoadChunk(pos);
                    }

                    //Debug.Log("World.LoadChunks:"+chunk.Pos+" hash:"+chunk.Pos.GetHashCode());

                    chunks.Add(pos, chunk);
                }
            }
        }

        this.chunks = chunks;
    }

    public Chunk GetChunk(Vector3Int pos)
    {
        var chunkPos = PosToChunkPos(pos);
        var chunkIdx = (chunkPos - centerChunkPos)/Config.CHUNK_SIZE + new Vector3Int(Config.WORLD_CHUNK_VIEW_DISTANCE, Config.WORLD_CHUNK_VIEW_DISTANCE, Config.WORLD_CHUNK_VIEW_DISTANCE);

        var blockPos = (chunkPos-pos).Abs();

        //Debug.Log("World GetBlockStates:"+pos+ " chunkPos:"+chunkPos+" hash:"+chunkPos.GetHashCode()+" chunkIdx:"+chunkIdx + " relBlockPos: "+blockPos);

        Chunk chunk;

        if(chunks.TryGetValue(chunkPos, out chunk)){
            //Debug.Log("TryGetValue OK");
            return chunk;
        }else{
            //Debug.Log("chunks:"+chunks.Count);
            /*
            foreach (var key in chunks.Keys)
            {
                Debug.Log("Key:"+key+" Value:"+chunks[key]);
            }*/
            return null;
        } 
    }

    public Dictionary<Vector3Int, Chunk>.KeyCollection ChunkKeys(){
        return chunks.Keys;
    }

    public BlockType GetBlockStates(Vector3Int pos)
    {
        Chunk chunk = GetChunk(pos);

        var blockPos = (chunk.Pos-pos).Abs();

        if(chunk != null){
            return chunk.GetBlockState(blockPos);
        }else{
            return BlockType.Air;
        }
    }

    private Vector3Int PosToChunkPos(Vector3Int pos)
    {
        return new Vector3Int(RoundToChunkSize(pos.x), RoundToChunkSize(pos.y), RoundToChunkSize(pos.z));
    }

    private int RoundToChunkSize(int a){
        return (int)Mathf.Floor((float)a/(float)Config.CHUNK_SIZE)*Config.CHUNK_SIZE;
    }
}
