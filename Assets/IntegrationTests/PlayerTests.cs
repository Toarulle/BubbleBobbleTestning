using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

[TestFixture]
public class PlayerTests
{
    private string sceneName = "PlayerScene";
    private Scene scene = new Scene();
    private PlayerBehaviour player = null;
    
    private InputTestFixture input = null;
    private Keyboard keyboard = null;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        input = new InputTestFixture();
        input.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
        player = Object.FindObjectOfType<PlayerBehaviour>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        input.TearDown();
        input = null;
        keyboard = null;
        player = null;
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator PlayerFalls()
    {
        float y0 = player.transform.position.y;
        yield return new WaitForFixedUpdate();
        float y1 = player.transform.position.y;
        Assert.Less(y1,y0); //Test Player second Y position is less than first (falling)
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerInput_UpKeyPressed_PlayerJumps()
    {
        while (!player.IsOnGround())
        {
            yield return new WaitForFixedUpdate();
        }
        float y0 = player.transform.position.y;
        input.Press(keyboard.wKey);
        yield return new WaitForSeconds(0.1f);
        float y1 = player.transform.position.y;
        Assert.Less(y0,y1); //Test Player first y position is less than second (has jumped)
        yield return null;    
    }
    
    [UnityTest]
    public IEnumerator PlayerInput_SpaceKeyPressed_PlayerShoots()
    {
        var bubble = GameObject.FindAnyObjectByType<BubbleBehaviour>();
        Assert.Null(bubble);    //Test that no Bubble exists (before space has been pressed)
        input.Press(keyboard.spaceKey);
        yield return new WaitForSeconds(0.1f);
        bubble = GameObject.FindAnyObjectByType<BubbleBehaviour>();
        var bubbles = GameObject.FindObjectsOfType<BubbleBehaviour>();
        Assert.NotNull(bubble); //Test that Bubble exists (after space has been pressed)
        Assert.AreEqual(1,bubbles.Length);
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator PlayerInput_RightKeyPressed_PlayerMovesRight()
    {
        float x0 = player.transform.position.x;
        input.Press(keyboard.dKey);
        yield return new WaitForSeconds(0.1f);
        float x1 = player.transform.position.x;
        Assert.Less(x0,x1); //Test Player first X position is less than second (moved right)
        yield return null;
    }
    
    [UnityTest]
    public IEnumerator PlayerInput_LeftKeyPressed_PlayerMovesLeft()
    {
        float x0 = player.transform.position.x;
        input.Press(keyboard.aKey);
        yield return new WaitForSeconds(0.1f);
        float x1 = player.transform.position.x;
        Assert.Less(x1,x0); //Test Player second X position is less than first (moved left)
        yield return null;
    }
    
    
}