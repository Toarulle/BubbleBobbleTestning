using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class DeathPortTests
{
    [Test]
    public void ValidUnityAction(){
        DeathPortObject deathPort = ScriptableObject.CreateInstance<DeathPortObject>();
        Assert.NotNull(deathPort.OnDeath);
    }

    [Test]
    public void UnityActionCallback(){
        DeathPortObject deathPort = ScriptableObject.CreateInstance<DeathPortObject>();
        int callbackCount = 0;
        deathPort.OnDeath += delegate(int pts){callbackCount++;};
        deathPort.Killed(0);
        Assert.AreEqual(1, callbackCount);
    }
}
