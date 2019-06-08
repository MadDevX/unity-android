using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class ScorePanelTracker : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private PrefabManager _prefabManager;
    private ServiceProvider _serviceProvider;

    private List<ScoreListEntry> _entries = new List<ScoreListEntry>();

    private Coroutine _cor = null;

    [Inject]
    public void Construct(ScoreManager scoreManager, PrefabManager prefabManager, ServiceProvider serviceProvider)
    {
        _scoreManager = scoreManager;
        _prefabManager = prefabManager;
        _serviceProvider = serviceProvider;
    }

    private void OnEnable()
    {
        _cor = StartCoroutine(ResetEntriesCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(_cor);
        _cor = null;
    }

    private bool ResetEntriesImmediate()
    {
        ClearEntries();
        var scoreList = _scoreManager.GetScoreList();
        scoreList.Sort((t1, t2) => { return t1.Value.CompareTo(t2.Value); });
        bool success = true;
        foreach (var scoreData in scoreList)
        {
            if (success)
            {
                success = AddEntry(scoreData.Key, scoreData.Value);
            }
            else break;
        }
        return success;
    }

    private IEnumerator ResetEntriesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            ResetEntriesImmediate();
        }
    }

    private bool AddEntry(NetworkInstanceId netId, int score)
    {
        var scoreEntry = Instantiate(_prefabManager.scoreListEntry, transform);
        var result = scoreEntry.Init(netId, score, _serviceProvider.allPlayers);
        _entries.Add(scoreEntry);
        return result;
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
