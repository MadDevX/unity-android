using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ScorePanelTracker : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private PrefabManager _prefabManager;

    private List<ScoreListEntry> _entries = new List<ScoreListEntry>();

    [Inject]
    public void Construct(ScoreManager scoreManager, PrefabManager prefabManager)
    {
        _scoreManager = scoreManager;
        _prefabManager = prefabManager;
    }

    private void Awake()
    {
        _scoreManager.OnDictChanged += ResetEntries;
    }

    private void OnDestroy()
    {
        _scoreManager.OnDictChanged -= ResetEntries;
    }

    private void ResetEntries()
    {
        ClearEntries();
        var scoreList = _scoreManager.GetScoreList();
        scoreList.Sort((t1, t2) => { return t1.Value.CompareTo(t2.Value); });

        foreach (var scoreData in scoreList)
        {
            AddEntry(scoreData.Key, scoreData.Value);
        }
    }

    private void AddEntry(NetworkInstanceId netId, int score)
    {
        var scoreEntry = Instantiate(_prefabManager.scoreListEntry, transform);
        scoreEntry.Init(netId, score, false); //Detect local player
        _entries.Add(scoreEntry);
    }

    private void ClearEntries()
    {
        foreach(var entry in _entries)
        {
            if (entry != null)
            {
                Destroy(entry.gameObject);
            }
        }

        _entries.Clear();
    }
}
