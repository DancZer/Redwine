using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WorldGenerator
{
    [Test]
    public void WorldGeneratorSimplePasses()
    {
        FastNoise noise = new FastNoise();

        noise.SetSeed(100);

        var v = (noise.GetSimplex(10,50)+1f)*100f;

        Debug.Log("GetSimplex:"+v);
    }

}
