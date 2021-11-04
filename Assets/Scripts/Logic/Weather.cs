using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather
{
    private CloudGenerator generator = new CloudGenerator();
    private FastNoise noise = new FastNoise();
    public readonly List<Cloud> clouds = new List<Cloud>();
    private const int cloudHeight = 2;
    private const int cloudVSpace = 1;
    private const int cloudLayerStartHeight = 100;
    private const int cloudLayerCount = 3;
    private Vector3 windDir = new Vector3(1,0,1);
    
    public void Init()
    {
        generator.Init(Config.RandomSeed, cloudHeight);
    }

    public void Update(Vector3 pos, int viewDistance){
        foreach (var cloud in clouds)
        {
            cloud.Update();
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
        var horPos = pos.Horizontal();
        foreach (var cloud in clouds)
        {
            if(Vector3.Distance(horPos, cloud.Pos.Horizontal()) < viewDistance){
                result.Add(cloud);
            }
        }

        return result;
    }

    private Cloud CreateNewCloud(Vector3 areaPos, int areaSize){
        var layerIdx = Random.Range(0, cloudLayerCount);

        var x = Random.Range(areaPos.x-areaSize/2, areaPos.x+areaSize/2);
        var z = Random.Range(areaPos.z-areaSize/2, areaPos.z+areaSize/2);
        var y = layerIdx*(cloudHeight+cloudVSpace) + cloudLayerStartHeight;      
        var pos = new Vector3(x,y,z);

        var sizeFactor = windDir * 50;
        var size = new Vector3(Random.Range(5,sizeFactor.x),2, Random.Range(5,sizeFactor.z));

        var velocityFactor = (layerIdx+1)/(float)cloudLayerCount;
        var velocity = windDir * Random.Range(1,5)*velocityFactor;

        var life = Random.Range(60, 600);

        return generator.Generate(pos, Vector3Int.FloorToInt(size), velocity, life);
    }
}
