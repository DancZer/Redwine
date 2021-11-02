using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    private World world;

    private GameObject[,,] ChunkRenderers;

    public MonoBehaviour Player;
    public GameObject ChunkRendererPrefab;

    // Start is called before the first frame update
    void Start()
    {        
        Player.transform.position = new Vector3();

        ChunkRenderers = new GameObject[World.Size, World.Size, World.Size];

        CreateRenderers();

        world = World.Instance();
        world.Init();

        Render();
    }
    private void CreateRenderers()
    {
        for(int x=0;x<World.Size;x++){
            for(int z=0;z<World.Size;z++){
                for(int y=0;y<World.Size;y++){
                    var chunkObject = Instantiate(ChunkRendererPrefab, new Vector3(), Quaternion.identity, transform);       
                    chunkObject.SetActive(false);
                    ChunkRenderers[x,y,z] = chunkObject;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = new BlockPos(Vector3Int.FloorToInt(Player.transform.position));
        if(world.Update(playerPos)){
            world.LoadChunks();
            Render();
        }
    }

    private void Render()
    {
        for(int x=0;x<World.Size;x++){
            for(int z=0;z<World.Size;z++){
                for(int y=0;y<World.Size;y++){
                    var obj = ChunkRenderers[x,y,z];
                    obj.SetActive(false);

                    var renderer = obj.GetComponent<ChunkRendererInterface>();
                    renderer.chunk = world.Chunks[x,y,z];
                    renderer.shouldRender = true;

                    obj.name = renderer.chunk.Name;
                    obj.transform.position = renderer.chunk.Pos.Vect;
                    obj.SetActive(true);
                }
            }
        }
    }

}
