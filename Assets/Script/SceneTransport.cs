using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class SceneTransport : MonoBehaviour
{
    public string NextScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            GameManager.Instance.LoadScene(NextScene);
    }
}
