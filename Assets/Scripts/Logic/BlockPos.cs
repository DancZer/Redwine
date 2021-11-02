using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BlockPos
{
    public int X {
        get
        {
            return Vect.x;
        }
    }

    public int Y {
        get
        {
            return Vect.y;
        }
    }

    public int Z {
        get
        {
            return Vect.z;
        }
    }

    public Vector3Int Vect { get;}

    public BlockPos(int x, int y, int z)
    {
        Vect = new Vector3Int(x,y,z);
    }

    public BlockPos(Vector3Int pos)
    {
        Vect = pos;
    }

    public BlockPos Above(){
        return new BlockPos(X, Y+1, Z);
    }

    public BlockPos Below(){
        return new BlockPos(X, Y-1, Z);
    }

    public BlockPos North(){
        return new BlockPos(X, Y, Z+1);
    }

    public BlockPos South(){
        return new BlockPos(X, Y, Z-1);
    }

    public BlockPos East(){
        return new BlockPos(X+1, Y, Z);
    }
    
    public BlockPos West(){
        return new BlockPos(X-1, Y, Z);
    }

    public BlockPos Abs(){
        return new BlockPos(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z));
    }

    override public string ToString(){
        return $"X:{X} Y:{Y} Z:{Z}";
    }

    public static BlockPos operator +(BlockPos a){
        return a;
    }
    public static BlockPos operator -(BlockPos a){
        return new BlockPos(-a.X, -a.Y, -a.Z);
    }
    
    
    public static BlockPos operator +(BlockPos a, Vector3Int b){
        return new BlockPos(a.X+b.x, a.Y+b.y, a.Z+b.z);
    }
    public static BlockPos operator -(BlockPos a, Vector3Int b){
        return new BlockPos(a.X-b.x, a.Y-b.y, a.Z-b.z);
    }
    

    public static BlockPos operator +(BlockPos a, int b){
        return new BlockPos(a.X+b, a.Y+b, a.Z+b);
    }
    public static BlockPos operator -(BlockPos a, int b){
        return new BlockPos(a.X-b, a.Y-b, a.Z-b);
    }


    public static BlockPos operator +(BlockPos a, BlockPos b){
        return new BlockPos(a.X+b.X, a.Y+b.Y, a.Z+b.Z);
    }
    public static BlockPos operator -(BlockPos a, BlockPos b){
        return new BlockPos(a.X-b.X, a.Y-b.Y, a.Z-b.Z);
    }
    
    public static Vector3Int operator +(Vector3Int b, BlockPos a){
        return new Vector3Int(a.X+b.x, a.Y+b.y, a.Z+b.z);
    }
    public static Vector3Int operator -(Vector3Int b, BlockPos a){
        return new Vector3Int(a.X-b.x, a.Y-b.y, a.Z-b.z);
    }

    public static Vector3 operator +(Vector3 b, BlockPos a){
        return new Vector3(a.X+b.x, a.Y+b.y, a.Z+b.z);
    }
    public static Vector3 operator -(Vector3 b, BlockPos a){
        return new Vector3(a.X-b.x, a.Y-b.y, a.Z-b.z);
    } 

    
    public static BlockPos operator *(int b, BlockPos a){
        return new BlockPos(a.X*b, a.Y*b, a.Z*b);
    }
    public static BlockPos operator *(BlockPos a, int b){
        return b*a;
    }

    public static BlockPos operator /(BlockPos a, int b){
        return new BlockPos(a.X/b, a.Y/b, a.Z/b);
    }

    public static BlockPos operator %(BlockPos a, int b){
        return new BlockPos(a.X%b, a.Y%b, a.Z%b);
    }

    public static bool operator ==(BlockPos a, BlockPos b){
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }
    public static bool operator !=(BlockPos a, BlockPos b){
        return !(a == b);
    }

    public static bool InRange(BlockPos from, BlockPos to, BlockPos a){
        return from.X <= a.X && a.X <= to.X && from.Y <= a.Y && a.Y <= to.Y && from.Z <= a.Z && a.Z <= to.Z;
    }

    public static bool InRange(int from, int to, BlockPos a){
        return from <= a.X && a.X < to && from <= a.Y && a.Y < to && from <= a.Z && a.Z < to;
    }

    public override bool Equals(object obj)
    {
        return Vect.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Vect.GetHashCode();
    }
}
