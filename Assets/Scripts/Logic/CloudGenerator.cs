using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator
{
    private FastNoise noise = new FastNoise();

    public void Init(int seed, int height){
        noise.SetSeed(seed);

        Random.InitState(seed);
    }

    public Cloud Generate(Vector3 pos, Vector3 dir){
        var size = new Vector3Int(Random.Range(5,10),2, Random.Range(5,10));

        var velocity = dir * Random.Range(1,5);

        var cloud = new Cloud(pos, size, velocity, Time.timeSinceLevelLoad, Random.Range(60, 600));

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
