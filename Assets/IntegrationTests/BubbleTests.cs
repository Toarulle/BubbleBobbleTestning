using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

[TestFixture]
public class BubbleTests
{
    private string sceneName = "BubbleScene";
    private Scene scene = new Scene();

    private BubbleBehaviour bubbleBehaviour = null;
    private ZenChanBehaviour zenChanEnemy = null;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
        bubbleBehaviour = Object.FindObjectOfType<BubbleBehaviour>();
        zenChanEnemy = Object.FindObjectOfType<ZenChanBehaviour>();
        zenChanEnemy.gameObject.SetActive(false);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        bubbleBehaviour = null;
        zenChanEnemy = null;
        yield return null;
    }

    [UnityTest]
    public IEnumerator Bubble_StartingMovement_PlayerIsFacingRight_BubbleMovesRight()
    {
        var x0 = bubbleBehaviour.transform.position.x;
        bool playerFacingRight = true;
        bubbleBehaviour.BubbleStartingMovement(playerFacingRight);
        yield return new WaitForSeconds(0.1f);
        float x1 = bubbleBehaviour.transform.position.x;
        Assert.Less(x0,x1); //Test bubble first X position is less than second (moved right)
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator Bubble_StartingMovement_PlayerIsFacingLeft_BubbleMovesLeft()
    {
        var x0 = bubbleBehaviour.transform.position.x;
        bool playerFacingRight = false;
        bubbleBehaviour.BubbleStartingMovement(playerFacingRight);
        yield return new WaitForSeconds(0.1f);
        float x1 = bubbleBehaviour.transform.position.x;
        Assert.Greater(x0,x1); //Test bubble first X position is greater than second (moved left)
        yield return null;
    }

    [UnityTest]
    public IEnumerator Bubble_StartingMovementRight_CatchEnemy()
    {
        bool playerFacingRight = true;
        zenChanEnemy.gameObject.SetActive(true);
        Assert.True(zenChanEnemy.transform.parent == null);
        bubbleBehaviour.BubbleStartingMovement(playerFacingRight);
        bubbleBehaviour.GetComponent<Collider2D>().isTrigger = true;
        bubbleBehaviour.firstMovementDone = false; //is for some reason not false as it should be, in the tests..
        yield return new WaitForSeconds(0.2f);
        Assert.True(zenChanEnemy.transform.parent == bubbleBehaviour.transform);
        yield return null;
    }
}