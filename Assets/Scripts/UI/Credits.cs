using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject credits;
    public GameObject endMessage;

    void Awake()
    {
        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(5);
        endMessage.SetActive(false);
        credits.SetActive(true);
        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(20);
        SceneManager.LoadScene("Main Menu");
    }
}
