using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class MaitaTests
{
    private string sceneName = "MaitaScene";
    private Scene scene = new Scene();

    private MaitaBehaviour maitaBehaviour = null;
    private GameObject player = null;
    private float standardWaitTime = 0.1f;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
        maitaBehaviour = Object.FindObjectOfType<MaitaBehaviour>();
        player = Object.FindObjectOfType<PlayerBehaviour>().gameObject;
        player.SetActive(false);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        maitaBehaviour = null;
        player = null;
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_StartInAir_FallDown()
    {
        var y0 = maitaBehaviour.transform.position.y;
        yield return new WaitForSeconds(standardWaitTime);
        var y1 = maitaBehaviour.transform.position.y;
        Assert.Less(y1,y0);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_StartMoveLeft_TurnRightAtWall_ThenTurnLeftAtWall()
    {
        maitaBehaviour.isFacingRight = false;
        float xScale0 = maitaBehaviour.transform.localScale.x;
        var x0 = maitaBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        var x1 = maitaBehaviour.transform.position.x;
        Assert.Less(x1,x0); //Is moving left
        while (!maitaBehaviour.isFacingRight)
        {
            yield return new WaitForFixedUpdate(); 
        }
        yield return new WaitForSeconds(standardWaitTime);
        float xScale1 = maitaBehaviour.transform.localScale.x;
        x0 = maitaBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        x1 = maitaBehaviour.transform.position.x;
        Assert.True(xScale0 == 1 && xScale1 == -1); //First turned to the left, then turned to the right
        Assert.Less(x0, x1); //Is moving right
        while (maitaBehaviour.isFacingRight)
        {
            yield return new WaitForFixedUpdate(); 
        }
        yield return new WaitForSeconds(standardWaitTime);
        float xScale2 = maitaBehaviour.transform.localScale.x;
        x0 = maitaBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime);
        x1 = maitaBehaviour.transform.position.x;
        Assert.True(xScale1 == -1 && xScale2 == 1); //First turned to the left, then turned to the right
        Assert.Less(x1,x0); //Is moving left
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_PlayerInRange_StartMoveToPlayer_MoveToPlayer()
    {
        player.SetActive(true);
        var playerPosX = player.transform.position.x;
        var playerPosY = player.transform.position.y;
        var x0 = maitaBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime*4);
        var x1 = maitaBehaviour.transform.position.x;
        Assert.Less(Math.Abs(playerPosX - x1), Math.Abs(playerPosX - x0)); //The new distance should be less than the first
        
        player.transform.position = new Vector3(-playerPosX, playerPosY); //Move the player to a new position to the right of the enemy
        playerPosX = player.transform.position.x;
        x0 = maitaBehaviour.transform.position.x;
        yield return new WaitForSeconds(standardWaitTime*4);
        x1 = maitaBehaviour.transform.position.x;
        Assert.Less(Math.Abs(playerPosX - x1), Math.Abs(playerPosX - x0)); //The new distance should be less than the first
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_PlayerInRange_StartMoveToPlayer_StopInFrontOfPlayer()
    {
        player.SetActive(true);
        var maitaX = maitaBehaviour.transform.position.x;
        var playerX = player.transform.position.x;
        var distance = Mathf.Abs(playerX - maitaX);
        do
        {
            yield return new WaitForFixedUpdate();
            maitaX = maitaBehaviour.transform.position.x;
            distance = Mathf.Abs(playerX - maitaX);
        } while (distance > maitaBehaviour.distanceToKeepFromPlayer);
        yield return new WaitForSeconds(standardWaitTime);
        Assert.True(distance <= maitaBehaviour.distanceToKeepFromPlayer);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_PlayerInRange_ShootPlayer()
    {
        player.SetActive(true);
        player.transform.position = new Vector3(player.transform.position.x, maitaBehaviour.transform.position.y);
        yield return new WaitForSeconds(standardWaitTime*4);
        Assert.True(Object.FindObjectOfType<BoulderBehaviour>());
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Maita_Patrolling_AfterAngryTime_TurnAngry()
    {
        Assert.False(maitaBehaviour.isAngry);
        while (!maitaBehaviour.isAngry)
        {
            yield return new WaitForFixedUpdate(); 
        }
        Assert.True(maitaBehaviour.isAngry);
        yield return null;
    }
}
