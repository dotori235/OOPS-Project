using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logTxt;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int _maxLines = 50;
    private readonly Queue<string> _lines = new Queue<string>();
    private static DebugLog _instance;
    public static DebugLog Instance { get => _instance; private set => _instance = value; }

    private void Awake()
    {
        _instance = this;
    }

    // Keep only the most recent _maxLines entries so the TMP text can't grow
    // without bound (vertex limit / GC) over a long play session.
    public void Print(string s)
    {
        _lines.Enqueue(s);
        while (_lines.Count > _maxLines) _lines.Dequeue();
        _logTxt.text = string.Join("\n\n", _lines);
        _scrollRect.verticalNormalizedPosition = 0.0f;
    }
}
