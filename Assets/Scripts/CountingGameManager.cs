using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountingGameManager : MonoBehaviour
{
    public GameObject[] deers;

    public int totalSet;
    public TMPro.TextMeshProUGUI setText;

    [Space]
    public int currentCount;
    public int currentSet;
    public int countAnswer;
    public int countRight;
    public int countWrong;


    [Space]
    public TMPro.TextMeshProUGUI counter;
    public TMPro.TextMeshProUGUI rightCountText;
    public TMPro.TextMeshProUGUI wrongCountText;
    public AudioSource buttonSound;

    [Space]
    public UnityEvent onStart;
    public UnityEvent onRight;
    public UnityEvent onWrong;
    public UnityEvent onEnd;

    private void Start()
    {
        SetButtonSounds();
    }

    public void SetButtonSounds()
    {
        Button[] btns = GameObject.FindObjectsOfType<Button>(true);
        foreach (Button b in btns)
        {
            b.onClick.AddListener(delegate
            {
                buttonSound.Play();
            });
        }
    }

    public void AddCount()
    {
        countAnswer++;
        countAnswer = Mathf.Clamp(countAnswer, 0, deers.Length);
        counter.text = countAnswer.ToString();
    }

    public void SubCount()
    {
        countAnswer--;
        countAnswer = Mathf.Clamp(countAnswer, 0, deers.Length);
        counter.text = countAnswer.ToString();
    }

    public void StartGame()
    {
        if (currentSet >= totalSet)
        {
            onEnd.Invoke();
            currentSet = 0;
            return;
        }

        currentSet++;
        countAnswer = 0;
        setText.text = $"SET {currentSet}/{totalSet}";

        int randomCount = Random.Range(5, deers.Length);
        currentCount = randomCount;

        foreach (GameObject go in deers)
        {
            go.SetActive(false);
        }


        for(int i=0; i<deers.Length; i++)
        {
            deers[i].SetActive(i < randomCount);
        }

        onStart.Invoke();
    }

    public void SubmitCount()
    {
        if(countAnswer == currentCount)
        {
            countRight++;
            onRight.Invoke();
        }
        else
        {
            countWrong++;
            onWrong.Invoke();
        }

        rightCountText.text = countRight.ToString();
        wrongCountText.text = countWrong.ToString();
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
