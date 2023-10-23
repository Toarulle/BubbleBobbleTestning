using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

public class ZenChanTests
{
    private string sceneName = "ZenChanScene";
    private Scene scene = new Scene();

    private ZenChanBehaviour zenChanBehaviour = null;
    private float standardWaitTime = 0.1f;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
        zenChanBehaviour = Object.FindObjectOfType<ZenChanBehaviour>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        zenChanBehaviour = null;
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ZenChan_StartInAir_FallDown()
    {
        var y0 = zenChanBehaviour.transform.position.y;
        yield return new WaitForSeconds(standardWaitTime);
        var y1 = zenChanBehaviour.transform.position.y;
        Assert.Less(y1,y0);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ZenChan_Patrolling_AfterAngryTime_TurnAngry()
    {
        Assert.False(zenChanBehaviour.isAngry);
        while (!zenChanBehaviour.isAngry)
        {
            yield return new WaitForFixedUpdate(); 
        }
        Assert.True(zenChanBehaviour.isAngry);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator ZenChan_StartMoveLeft_TurnRightAtWall_ThenTurnLeftAtWall()
    {
        zenChanBehaviour.isFacingRight = false;
        float xScale0 = zenChanBehaviour.transform.localScale.x;
        var x0 = zenChanBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        var x1 = zenChanBehaviour.transform.position.x;
        Assert.Less(x1,x0); //Is moving left
        while (!zenChanBehaviour.isFacingRight)
        {
            yield return new WaitForFixedUpdate(); 
        }
        yield return new WaitForSeconds(standardWaitTime);
        float xScale1 = zenChanBehaviour.transform.localScale.x;
        x0 = zenChanBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        x1 = zenChanBehaviour.transform.position.x;
        Assert.True(xScale0 == 1 && xScale1 == -1); //First turned to the left, then turned to the right
        Assert.Less(x0, x1); //Is moving right
        while (zenChanBehaviour.isFacingRight)
        {
            yield return new WaitForFixedUpdate(); 
        }
        yield return new WaitForSeconds(standardWaitTime);
        float xScale2 = zenChanBehaviour.transform.localScale.x;
        x0 = zenChanBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        x1 = zenChanBehaviour.transform.position.x;
        Assert.True(xScale1 == -1 && xScale2 == 1); //First turned to the left, then turned to the right
        Assert.Less(x1,x0); //Is moving left
        yield return null;
    }
}
