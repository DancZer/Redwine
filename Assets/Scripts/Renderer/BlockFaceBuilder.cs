using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFaceBuilder
{
    private readonly Vector3 tr1;
    private readonly Vector3 tr1Inverse;
    private readonly Vector3 tr3;
    private readonly Vector3 tr3Inverse;
    private bool isNewStarted;
    private Vector3 startPos;
    private Vector3 endPos;
    public List<Vector3> Verts {get;} = new List<Vector3>();
    public int NumFaces {get; private set;}

    public BlockFaceBuilder(Vector3 _tr1, Vector3 _tr3)
    {
        tr1 = _tr1;
        tr1Inverse = _tr1.Inverse();

        tr3 = _tr3;
        tr3Inverse = _tr3.Inverse();
    }

    public void ExtendTo(Vector3 pos)
    {
        if(!isNewStarted){
            startPos = pos;
            isNewStarted = true;
        }
        endPos = pos + tr1 + tr3;
    }

    public void Close()
    {
        if(isNewStarted){
            Verts.Add(startPos);
            Verts.Add(startPos.Multiply(tr1Inverse) + endPos.Multiply(tr1));
            Verts.Add(endPos);
            Verts.Add(startPos.Multiply(tr3Inverse) + endPos.Multiply(tr3));

            NumFaces++;
        }

        isNewStarted = false;
    }
}
