using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather
{
    private CloudGenerator generator = new CloudGenerator();
    private FastNoise noise = new FastNoise();
    public readonly List<Cloud> clouds = new List<Cloud>();

    private const int CloudHeight = 100;
    private const int MaxCloudCount = 2;
    private const int MinCloudDistance = 2;
    
    public void Init()
    {
        generator.Init(Config.RandomSeed, CloudHeight);
    }

    public void Update(Vector3 pos, int viewDistance){
        foreach (var cloud in clouds)
        {
            cloud.Pos += cloud.Speed;
        }

        if(clouds.Count<10)
        {
            if(Random.Range(0, 60) == 0){
                clouds.Add(CreateNewCloud(pos, viewDistance));
            }
        }
    }

    public List<Cloud> GetVisibleClouds(Vector3 pos, int viewDistance){
        var result = new List<Cloud>();
        foreach (var cloud in clouds)
        {
            if(Vector3.Distance(pos, cloud.Pos) < Config.ViewDistanceBlockCount){
                result.Add(cloud);
            }
        }

        return result;
    }

    private Cloud CreateNewCloud(Vector3 pos, int areaSize){
        var x = Random.Range(pos.x-areaSize/2,pos.x+areaSize/2);
        var z = Random.Range(pos.z-areaSize/2,pos.z+areaSize/2);
        var y = GetCloudProfile(x, z);

        return generator.Generate(new Vector3(x,y,z), new Vector3(1,0,1));
    }

    
    public int GetCloudProfile(float x, float z){
        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = CloudHeight - CloudHeight/2f + heightMap;

        return Mathf.FloorToInt(baseLandHeight);
    }
}
