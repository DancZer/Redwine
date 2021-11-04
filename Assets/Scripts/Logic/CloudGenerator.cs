using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator
{
    private readonly FastNoise noise = new FastNoise();
    private int baseHeight;

    public void Init(int seed, int height){
        noise.SetSeed(seed);

        Random.InitState(seed);

        baseHeight = height;
    }

    public Cloud Generate(Vector3 pos, Vector3Int size, Vector3 velocity, float life){

        var cloud = new Cloud(pos, size, velocity, life);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    cloud.SetBlockType(new Vector3Int(x,y,z), BlockType.Cloud);
                }
            }    
        }

        return cloud;
    }
}
