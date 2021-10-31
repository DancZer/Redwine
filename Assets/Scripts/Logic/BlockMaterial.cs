using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterial
{
        public const float MATERIAL_COUNT = 16f;
        public const float MARGIN = 1/MATERIAL_COUNT/100f;

        private int xPos, yPos;

        private Vector2[] uvs;

        private string Name {get;}

        public BlockMaterial(int xPos, int yPos, string name)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.Name = name;

            uvs = new Vector2[]
            {
                new Vector2(xPos/MATERIAL_COUNT + MARGIN, yPos/MATERIAL_COUNT + MARGIN),
                new Vector2(xPos/MATERIAL_COUNT+ MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
                new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, (yPos+1)/MATERIAL_COUNT - MARGIN),
                new Vector2((xPos+1)/MATERIAL_COUNT - MARGIN, yPos/MATERIAL_COUNT+ MARGIN),
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
            return Name;
        }
    }
