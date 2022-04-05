using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Spawner : MonoBehaviour
{
    [SerializeField]
    public float PlayerSpawnPosX;
    [SerializeField]
    public float PlayerSpawnPosY;



    const float RandTerm = 5f;
    const float RandSpawnMinXRange = 0f;
    const float RandSpawnMaxXRange = 52f;
    const float FixTerm = 15f;
    [SerializeField]
    public List<float> Xpos;
    [SerializeField]
    public List<float> Ypos;
    int FixSpawnSequence = 0;
    List<GameObject> Enemys = new List<GameObject>();
    void Start()
    {
        // Player ==============================================



        // Enemy  ==============================================


        if (Enemys.Count == 0) {
            Enemys.Add(Resources.Load("Prefab/Slime") as GameObject);
            Enemys.Add(Resources.Load("Prefab/Warrior") as GameObject);
        }
        // Random Spawn
        //for (int i = 0; i < Enemys.Count; i++)
        //    StartCoroutine(RandomSpawn(Enemys[i], RandTerm));

        // Fixed Spawn
        for (int i = 0; i < Enemys.Count; i++)
            StartCoroutine(FixedSpawn(Enemys[i], FixTerm));

    }

    IEnumerator RandomSpawn(GameObject Enemy, float Term)
    {
        Instantiate(Enemy, new Vector2(Random.Range(RandSpawnMinXRange, RandSpawnMaxXRange), 0), Quaternion.identity);
        yield return new WaitForSeconds(Term);
        StartCoroutine(RandomSpawn(Enemy, Term));
    }
    IEnumerator FixedSpawn(GameObject Enemy, float Term)
    {
        Instantiate(Enemy, new Vector2(Xpos[FixSpawnSequence], Ypos[FixSpawnSequence]), Quaternion.identity);
        yield return new WaitForSeconds(Term);
        StartCoroutine(RandomSpawn(Enemy, Term));
        FixSpawnSequence++;
        if (FixSpawnSequence >= Xpos.Count)
            FixSpawnSequence = 0;
    }
    public void PlayerReSpawn(Status stat)
    {
        GameObject Player = Instantiate(Resources.Load("Prefab/Player") as GameObject, new Vector2(PlayerSpawnPosX, PlayerSpawnPosY), Quaternion.identity);
        Player.GetComponent<Status>().StatInit(stat);
        Player.GetComponent<Player>().CheckEvent();
        GameObject.Find("CAM").GetComponent <CinemachineVirtualCamera>().Follow = Player.transform;

    }


}
