using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMethodExtension
{
    public static Vector3Int Above(this Vector3Int v){
        return new Vector3Int(v.x, v.y+1, v.z);
    }
    public static Vector3Int AboveChunk(this Vector3Int v){
        return new Vector3Int(v.x, v.y+Config.ChunkSize, v.z);
    }

    public static Vector3Int Below(this Vector3Int v){
        return new Vector3Int(v.x, v.y-1, v.z);
    }
    public static Vector3Int BelowChunk(this Vector3Int v){
        return new Vector3Int(v.x, v.y-Config.ChunkSize, v.z);
    }

    public static Vector3Int North(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z+1);
    }

    public static Vector3Int NorthChunk(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z+Config.ChunkSize);
    }

    public static Vector3Int South(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z-1);
    }

    public static Vector3Int SouthChunk(this Vector3Int v){
        return new Vector3Int(v.x, v.y, v.z-Config.ChunkSize);
    }

    public static Vector3Int East(this Vector3Int v){
        return new Vector3Int(v.x+1, v.y, v.z);
    }
    public static Vector3Int EastChunk(this Vector3Int v){
        return new Vector3Int(v.x+Config.ChunkSize, v.y, v.z);
    }
    
    public static Vector3Int West(this Vector3Int v){
        return new Vector3Int(v.x-1, v.y, v.z);
    }
    public static Vector3Int WestChunk(this Vector3Int v){
        return new Vector3Int(v.x-Config.ChunkSize, v.y, v.z);
    }

    public static Vector3Int Abs(this Vector3Int v){
        return new Vector3Int(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static Vector3Int ToChunkAligned(this Vector3Int v){
        return new Vector3Int(RoundToChunkSize(v.x), RoundToChunkSize(v.y), RoundToChunkSize(v.z));
    }

    public static Vector3 Horizontal(this Vector3 v){
        return new Vector3(v.x, 0, v.z);
    }

    public static Vector3Int Horizontal(this Vector3Int v){
        return new Vector3Int(v.x, 0, v.z);
    }

    private static int RoundToChunkSize(int a){
        return (int)Mathf.Floor((float)a/(float)Config.ChunkSize)*Config.ChunkSize;
    }

    public static Vector3 Multiply(this Vector3 a, Vector3 b){
        return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
    }

    public static Vector3 Inverse(this Vector3 a){
        return new Vector3(Mathf.Abs(a.x-1), Mathf.Abs(a.y-1), Mathf.Abs(a.z-1));
    }
}
