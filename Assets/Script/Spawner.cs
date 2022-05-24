using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Spawner : MonoBehaviour
{

    GameObject player;


    void Start()
    {
        // Player ==============================================
        player = GameObject.FindGameObjectWithTag("Player");

    }
    public void PlayerReSpawn()
    {
        GameObject newPlayer = Instantiate(Resources.Load("Prefab/Player"), new Vector2(transform.localPosition.x, transform.localPosition.y), Quaternion.identity) as GameObject;

        newPlayer.GetComponent<Status>().StatInit(player.GetComponent<Status>());
        newPlayer.GetComponent<Player>().ChangeDragon(player.GetComponent<Player>().ChangeMode);
        Destroy(player);
        player = newPlayer;
        if (GameObject.Find("Constraints").GetComponent<PlayerConstraints>().JumpTwice)
            player.GetComponent<Move>().JumpMaxCount = 2;
        else
            player.GetComponent<Move>().JumpMaxCount = 1;
        GameObject.Find("CAM").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }


}
