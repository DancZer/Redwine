using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator
{
    private FastNoise noise = new FastNoise();

    public void Init(int seed){
        noise.SetSeed(seed);
    }

    public Chunk Generate(Vector3Int pos){
        var chunk = new Chunk(pos.ToChunkAligned());

        for(int x=-1; x<Config.ChunkSize+1; x++){
            for(int z=-1; z<Config.ChunkSize+1; z++){

                var baseLine = GetBaseLandHeight(chunk.Pos.x+x, chunk.Pos.z+z);

                for(int y=-1; y<Config.ChunkSize+1; y++){
                    var block = BlockType.Air;

                    if(chunk.Pos.y + y == baseLine){
                        block = BlockType.DirtGrass;
                    }else if(chunk.Pos.y + y < baseLine){
                        block = BlockType.Dirt;
                    }

                    chunk.SetBlockType(new Vector3Int(x, y, z), block);
                }
            }
        }

        return chunk;
    }

    public int GetBaseLandHeight(int x, int z){
        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = Config.ChunkSize * .5f + heightMap;

        return Mathf.FloorToInt(baseLandHeight);
    }
}
