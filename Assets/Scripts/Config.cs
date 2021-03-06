using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    public const int ChunkSize = 8;
    public const int ViewDistanceChunkCount = 8;
    public const int ViewDistanceBlockCount = Config.ViewDistanceChunkCount*Config.ChunkSize;
    public const int CloudViewDistanceBlockCount = ViewDistanceBlockCount*3;
    public static readonly int RandomSeed = 10;//(int)System.DateTime.Now.TimeOfDay.TotalSeconds;

    public const int StoneLayerStart = -30;
}