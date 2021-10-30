using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRenderer : MonoBehaviour
{
    void Start()
    {
        Render();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Render(){
        var chunkRenderer = GetComponent<ChunkRendererInterface>();

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(this.transform);
        cube.transform.localScale = new Vector3(Chunk.SIZE,Chunk.SIZE,Chunk.SIZE);
        cube.transform.localPosition = new Vector3();
    }
}
