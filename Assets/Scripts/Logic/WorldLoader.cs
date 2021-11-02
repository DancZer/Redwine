using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader
{
    private FastNoise noise = new FastNoise();

    private Dictionary<string, Chunk> _cache = new Dictionary<string, Chunk>();

    public void Init(){
        noise.SetSeed((int)System.DateTime.Now.TimeOfDay.TotalSeconds);
    }

    public Chunk LoadChunk (BlockPos pos){

        Chunk chunk;

        if(!_cache.TryGetValue(pos.ToString(), out chunk)){
            chunk = GenerateChunk(pos);
            
            _cache.Add(pos.ToString(), chunk);
        }

        return chunk;
    }

    private Chunk GenerateChunk(BlockPos pos){
        var chunk = new Chunk(pos);

        for(int x=-1; x<Config.CHUNK_SIZE+1; x++){
            for(int z=-1; z<Config.CHUNK_SIZE+1; z++){

                var baseLine = GetBaseLandHeight(pos.X+x, pos.Z+z);

                for(int y=-1; y<Config.CHUNK_SIZE+1; y++){
                    var block = BlockType.Air;

                    if(chunk.Pos.Y + y <= baseLine){
                        block = BlockType.Dirt;
                    }

                    chunk.SetBlockState(x, y, z, block);
                }
            }
        }

        return chunk;
    }

    public float GetBaseLandHeight(int x, int z){
        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = Config.CHUNK_SIZE * .5f + heightMap;

        return baseLandHeight;
    }
}
