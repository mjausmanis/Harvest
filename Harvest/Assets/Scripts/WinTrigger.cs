
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    private GameObject player;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.gameObject;
            StartCoroutine(handleWin());
        }
    }

    IEnumerator handleWin() {
        Transform winScreen = player.transform.Find("PlayerHUD/GameWon");

        winScreen.gameObject.SetActive(true);
        Time.timeScale = 0.3f;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
