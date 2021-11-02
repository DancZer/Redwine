using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Profiler
{
    private static Dictionary<string, long> TimeMeasures = new Dictionary<string, long>();

    public static void Start(string key){
        if(!TimeMeasures.TryAdd(key, GetTime())){
            TimeMeasures[key] = GetTime();
        }
    }

    public static void Stop(string key){
        long time;
        if(TimeMeasures.TryGetValue(key, out time)){
            var diff = (GetTime()-time);
            Debug.Log(key+$":{diff}");
        }
    }

    private static long GetTime(){
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}
