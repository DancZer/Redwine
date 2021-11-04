using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMethodExtension
{
    public static Vector3Int Above(this Vector3Int v){
        return new Vector3Int(v.x, v.y+1, v.z);
    }

    public static Vector3Int Below(this Vector3Int v){
        return new Vector3Int(v.x, v.y-1, v.z);
    }

    public static Vector3Int North(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z+1);
    }

    public static Vector3Int South(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z-1);
    }

    public static Vector3Int East(this Vector3Int v){
        return new Vector3Int(v.x+1, v.y, v.z);
    }
    
    public static Vector3Int West(this Vector3Int v){
        return new Vector3Int(v.x-1, v.y, v.z);
    }

    public static Vector3Int Abs(this Vector3Int v){
        return new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static Vector3Int ToChunkAligned(this Vector3Int v){
        return new Vector3Int(RoundToChunkSize(v.x), RoundToChunkSize(v.y), RoundToChunkSize(v.z));
    }

    private static int RoundToChunkSize(int a){
        return (int)Mathf.Floor((float)a/(float)Config.ChunkSize)*Config.ChunkSize;
    }
}
