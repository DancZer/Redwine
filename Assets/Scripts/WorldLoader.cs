using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader
{
    private FastNoise noise = new FastNoise();

    public Chunk LoadChunk (BlockPos pos){
        var chunk = new Chunk(pos);

        var chunkWorldPosStart = chunk.BlockPos;
        var chunkWorldPosEnd = chunk.BlockPos + Chunk.SIZE;

        for(int x=chunkWorldPosStart.X;x<chunkWorldPosEnd.X;x++){
            for(int z=chunkWorldPosStart.Z;z<chunkWorldPosEnd.Z;z++){
                var baseLine = GetBaseLandHeight(x, z);
                for(int y=chunkWorldPosStart.Y;y<chunkWorldPosEnd.Y;y++){
                    Block block = Blocks.Air;

                    if(y<=baseLine){
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
