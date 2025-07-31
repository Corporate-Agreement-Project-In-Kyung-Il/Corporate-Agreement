using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Lobby,
    Statge1,
    Statge2,
    Statge3,
    Statge4,
    Statge5,
}

[System.Serializable]
public class BGMData
{
    public GameState state;
    public AudioClip bgmClip;
}

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [SerializeField]
    private AudioSource bgmSource;

    [SerializeField]
    private List<BGMData> bgmList;

    private Dictionary<GameState, AudioClip> bgmDict = new Dictionary<GameState, AudioClip>();
    private GameState currentState;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (BGMData data in bgmList)
        {
            if (bgmDict.ContainsKey(data.state) == false)
            {
                bgmDict.Add(data.state, data.bgmClip);
            }
        }

        ChangeBGM(GameState.Lobby);

    }

    public void ChangeBGM(GameState newState)
    {
       
        currentState = newState;
        if (bgmDict.TryGetValue(newState, out AudioClip clip))
        {
            if (bgmSource.clip != clip)
            {
                bgmSource.clip = clip;
                bgmSource.volume = 1f;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }
        else
        {
            Debug.Log($"BGM ¾øÀ½ {newState}");
        }
    }
}
