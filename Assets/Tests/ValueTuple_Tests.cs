using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

[TestFixture]
public class ValueTuple_Tests
{
    [Test]
public void TwoFlatNodeTransforms_Equality()
    {
        Assert.True((FlatNodeTransform.Default, FlatNodeTransform.InvertedY) == (FlatNodeTransform.Default, FlatNodeTransform.InvertedY));
        Assert.False((FlatNodeTransform.Default, FlatNodeTransform.Default) == (FlatNodeTransform.Default, FlatNodeTransform.InvertedY));
    }
}
