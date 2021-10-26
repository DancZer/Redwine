using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterial
{
        public const float MATERIAL_COUNT = 16f;
        public const float MARGIN = 0f;

        private int xPos, yPos;

        private Vector2[] uvs;

        public BlockMaterial(int xPos, int yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            uvs = new Vector2[]
            {
                new Vector2(xPos/MATERIAL_COUNT + MARGIN, yPos/MATERIAL_COUNT + MARGIN),
                new Vector2(xPos/MATERIAL_COUNT+ MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
                new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
                new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, yPos/MATERIAL_COUNT+ MARGIN),
            };
        }

        public Vector2[] GetUVs()
        {
            return uvs;
        }
    }
