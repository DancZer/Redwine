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

        for(int x=0;x<Chunk.SIZE;x++){
            for(int z=0;z<Chunk.SIZE;z++){

                var baseLine = GetBaseLandHeight(pos.X+x, pos.Z+z);

                for(int y=0;y<Chunk.SIZE;y++){
                    Block block = Blocks.Air;

                    if(chunk.Pos.Y + y <= baseLine){
                        block = Blocks.Dirt;
                    }

                    chunk.SetBlockState(new BlockPos(x, y, z), block.Default());
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
        float baseLandHeight = Chunk.SIZE * .5f + heightMap;

        return baseLandHeight;
    }
}
