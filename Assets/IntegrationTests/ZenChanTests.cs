using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class ZenChanTests : MonoBehaviour
{
    private string sceneName = "ZenChanScene";
    private Scene scene = new Scene();

    private ZenChanBehaviour zenChanBehaviour = null;

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
    public IEnumerator ZenChan_StartMoveToPlayer_MoveToPlayer()
    {
        GameObject player = FindObjectOfType<PlayerBehaviour>().gameObject;
        var playerPos = player.transform.position.x;
        var x0 = zenChanBehaviour.transform.position.x;
        yield return new WaitForSeconds(0.1f);
        var x1 = zenChanBehaviour.transform.position.x;
        Assert.Less(Math.Abs(playerPos - x1), Math.Abs(playerPos - x0));
        
        player.transform.position = new Vector3(-playerPos, 0);
        playerPos = player.transform.position.x;
        x0 = zenChanBehaviour.transform.position.x;
        yield return new WaitForSeconds(0.1f);
        x1 = zenChanBehaviour.transform.position.x;
        Assert.Less(Math.Abs(playerPos - x1), Math.Abs(playerPos - x0));
        yield return null;
    }
}
