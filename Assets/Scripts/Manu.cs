using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manu : MonoBehaviour
{
    public void GotoRusa()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GotoPanah()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
