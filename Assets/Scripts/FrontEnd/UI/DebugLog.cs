using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DebugLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logTxt;
    [SerializeField] private ScrollRect _scrollRect;
    private static DebugLog _instance;
    public static DebugLog Instance { get => _instance; private set => _instance = value;  }

    private void Awake()
    {
        _instance = this;
    }

    public void Print(string s)
    {
        _logTxt.text += s+"\n\n";
        _scrollRect.verticalNormalizedPosition = 0.0f;

    }
}
