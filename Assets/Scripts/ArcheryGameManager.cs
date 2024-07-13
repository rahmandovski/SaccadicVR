using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArcheryGameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Tutorial,
        Game,
        Result
    }

    public delegate void GameStateTrigger(GameState state);

    public GameState gameState;
    public GameStateTrigger OnGameStateChange;

    [Space]
    public List<GameObject> targets1;
    public List<GameObject> targets2;

    public float gameTime;
    [Space]
    public float time;
    public int score;
    public int currentTargetId;

    [Space]
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI skorText;
    public TMPro.TextMeshProUGUI resultScore;
    public AudioSource buttonSound;
    public Transform hitObject;
    public bool timerStart;

    [Space]
    public UnityEvent onMenu;
    public UnityEvent onTutorial;
    public UnityEvent onPlay;
    public UnityEvent onResult;
    
    public void StartGame()
    {
        timerStart = false;
        time = gameTime;
        score = 0;
        currentTargetId = 1;

        ShffleTargetList(ref targets1);
        ShffleTargetList(ref targets2);

        targets1[0].SetActive(true);
        targets1[0].GetComponentInChildren<TMPro.TextMeshPro>().text = currentTargetId.ToString();

        currentTargetId++;
        targets2[0].SetActive(true);
        targets2[0].GetComponentInChildren<TMPro.TextMeshPro>().text = currentTargetId.ToString();
        ChangeState(GameState.Game);
    }

    void ShffleTargetList(ref List<GameObject> array)
    {
        List<GameObject> t = new List<GameObject>();
        while (array.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, array.Count);
            GameObject tr = array[random];
            t.Add(tr);
            array.Remove(tr);
        }
        array.AddRange(t);
        t.Clear();
    }

    private void Update()
    {
        if(gameState == GameState.Game)
        {
            if(timerStart)
            {
                time -= Time.deltaTime;
            }

            if (time <= 0)
            {
                ChangeState(GameState.Result);
            }
        }


        timeText.text = $"{GetTime(time)}";
        skorText.text = score.ToString();
        resultScore.text = score.ToString();
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        OnGameStateChange?.Invoke(gameState); 

        if (state == GameState.Menu) onMenu.Invoke();
        if (state == GameState.Tutorial) onTutorial.Invoke();
        if (state == GameState.Game) onPlay.Invoke();
        if (state == GameState.Result) onResult.Invoke();
    }

    public void NextTarget(GameObject target)
    {
        timerStart = true;
        currentTargetId++;
        score++;
        NextTargetFromArray(ref targets1.Contains(target) ? ref targets1 : ref targets2, target);
    }

    public void NextTargetFromArray(ref List<GameObject> array, GameObject target)
    {
        int index = array.IndexOf(target);
        array[index].SetActive(false);
        
        index++;
        if (index >= array.Count) index = 0;

        array[index].GetComponentInChildren<TMPro.TextMeshPro>().text = currentTargetId.ToString();
        array[index].SetActive(true);
    }

    public void GoToMenu()
    {
        ChangeState(GameState.Menu);
    }

    public void GoToTutorial()
    {
        ChangeState(GameState.Tutorial);
    }

    public void ResetGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            ) ;
    }

    public void TargetShooted(Vector3 position)
    {
        hitObject.transform.position = position;
        hitObject.gameObject.SetActive(true);
    }

    public string GetTime(float s)
    {
        TimeSpan t = TimeSpan.FromSeconds(s);
        string answer = string.Format("{1:D2}:{2:D2}:{3:D3}",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
        return answer;
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

    private void Start()
    {
        SetButtonSounds();
    }

    public void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
