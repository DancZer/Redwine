using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TriangleBuilderTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void GIVEN_VectorXZ_WHEN_ExtendToZero_THEN_OneFace()
    {
        var builder = new BlockFaceBuilder(new Vector3(1,0,0), new Vector3(0,0,1));

        builder.ExtendTo(new Vector3(0,0,0));
        builder.Close();

        var vect = builder.Verts;

        Assert.AreEqual(4, vect.Count);
        Assert.AreEqual(1, builder.NumFaces);

        Assert.AreEqual(new Vector3(0,0,0), vect[0]);
        Assert.AreEqual(new Vector3(1,0,0), vect[1]);
        Assert.AreEqual(new Vector3(1,0,1), vect[2]);
        Assert.AreEqual(new Vector3(0,0,1), vect[3]);
    }

    [Test]
    public void GIVEN_VectorXZ_WHEN_ExtendTwoTimesByXZ_THEN_OneFace()
    {
        var builder = new BlockFaceBuilder(new Vector3(1,0,0), new Vector3(0,0,1));

        builder.ExtendTo(new Vector3(0,0,0));
        builder.ExtendTo(new Vector3(1,0,1));

        builder.Close();

        var vect = builder.Verts;

        Assert.AreEqual(4, vect.Count);

        Assert.AreEqual(new Vector3(0,0,0), vect[0]);
        Assert.AreEqual(new Vector3(2,0,0), vect[1]);
        Assert.AreEqual(new Vector3(2,0,2), vect[2]);
        Assert.AreEqual(new Vector3(0,0,2), vect[3]);

        Assert.AreEqual(1, builder.NumFaces);
    }

    [Test]
    public void GIVEN_VectorXZ_WHEN_ExtendCloseExtend_THEN_TwoFaces()
    {
        var builder = new BlockFaceBuilder(new Vector3(1,0,0), new Vector3(0,0,1));

        builder.ExtendTo(new Vector3(0,0,0));
        builder.Close();

        builder.ExtendTo(new Vector3(1,0,1));
        builder.Close();

        var vect = builder.Verts;

        Assert.AreEqual(8, vect.Count);
        Assert.AreEqual(2, builder.NumFaces);

        //face one
        Assert.AreEqual(new Vector3(0,0,0), vect[0]);
        Assert.AreEqual(new Vector3(1,0,0), vect[1]);
        Assert.AreEqual(new Vector3(1,0,1), vect[2]);
        Assert.AreEqual(new Vector3(0,0,1), vect[3]);

        //face two
        Assert.AreEqual(new Vector3(1,0,1), vect[4]);
        Assert.AreEqual(new Vector3(2,0,1), vect[5]);
        Assert.AreEqual(new Vector3(2,0,2), vect[6]);
        Assert.AreEqual(new Vector3(1,0,2), vect[7]);
    }
}
