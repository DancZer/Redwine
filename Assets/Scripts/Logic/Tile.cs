using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public const float MATERIAL_COUNT = 16f;
    public const float MARGIN = 1/MATERIAL_COUNT/100f;
    private string name;
    private int xPos, yPos;
    private Vector2[] uvs;
    public Tile(int xPos, int yPos, string name)
    {
        if(xPos < 0 || yPos < 0 || xPos >= (int)MATERIAL_COUNT || yPos >= (int)MATERIAL_COUNT)
        {
            throw new UnityException($"Invalid tile position for:{name}!");
        }

        this.xPos = xPos;
        this.yPos = yPos;
        this.name = name;

        uvs = new Vector2[]
        {
            new Vector2(xPos/MATERIAL_COUNT + MARGIN, yPos/MATERIAL_COUNT + MARGIN),
            new Vector2(xPos/MATERIAL_COUNT + MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
            new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
            new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, yPos/MATERIAL_COUNT + MARGIN),
        };
    }

    public Vector2 GetOffset(){
        return uvs[0];
    }

    public Vector2 GetScale(){
        return new Vector2(1f/MATERIAL_COUNT, 1f/MATERIAL_COUNT);
    }

    public Vector2[] GetUVs()
    {
        return uvs;
    }

    public override string ToString(){
        return name;
    }
}
