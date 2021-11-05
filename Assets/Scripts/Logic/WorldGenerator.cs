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
                for(int y=-1; y<Config.ChunkSize+1; y++){
                    chunk.SetBlockType(new Vector3Int(x, y, z), GetBlockType(chunk.Pos + new Vector3Int(x,y,z)));
                }
            }
        }

        return chunk;
    }

    public int GetBaseLandHeight(Vector3Int pos){
        float simplex1 = noise.GetSimplex(pos.x*.8f, pos.z*.8f)*10;
        float simplex2 = noise.GetSimplex(pos.x * 3f, pos.z * 3f) * 10*(noise.GetSimplex(pos.x*.3f, pos.z*.3f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        return Mathf.CeilToInt(Config.ChunkSize * .5f + heightMap);
    }  

    private BlockType GetBlockType(Vector3Int pos){
        
        var baseLandHeight = GetBaseLandHeight(pos);

        //over the surface
        if(pos.y > baseLandHeight){
            return BlockType.Air;
        //the surface
        }else if(pos.y == baseLandHeight){
            return BlockType.DirtGrass;
        }

         //3d noise for caves and overhangs and such
        float caveNoise1 = noise.GetPerlinFractal(pos.x*5f, pos.y*10f, pos.z*5f);
        float caveMask = noise.GetSimplex(pos.x * .3f, pos.z * .3f)+.3f;

        if(caveNoise1 > Mathf.Max(caveMask, .2f)){
            return BlockType.Air;
        }

        float simplexStone1 = noise.GetSimplex(pos.x * 1f, pos.z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(pos.x * 5f, pos.z * 5f)+.5f) * 20 * (noise.GetSimplex(pos.x * .3f, pos.z * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = Config.StoneLayerStart + stoneHeightMap;

        if(pos.y > baseStoneHeight){
            return BlockType.Dirt;
        }else{
            return BlockType.Stone;
        }
    }
}
