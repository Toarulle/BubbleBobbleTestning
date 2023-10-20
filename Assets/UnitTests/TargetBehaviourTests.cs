using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TargetBehaviourTests
{
    private GameObject testObject = new GameObject();
    
    [Test]
    public void ValidUnityAction(){
        
        TargetBehaviour targetB = ObjectFactory.AddComponent<TargetBehaviour>(testObject);
        Assert.NotNull(targetB.onAttack);
    }

    [Test]
    public void UnityActionCallback(){
        TargetBehaviour targetB = ObjectFactory.AddComponent<TargetBehaviour>(testObject);
        int callbackCount = 0;
        targetB.onAttack += delegate(TargetBehaviour target){callbackCount++;};
        targetB.Attack();
        Assert.AreEqual(1, callbackCount);
    }
}
