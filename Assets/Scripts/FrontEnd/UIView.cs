using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIView : MonoBehaviour
{
    [SerializeField]protected UIType type;
    private float value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public UIType GetUIType()
    {
        return type;
    }


    public void SetValue(float value)
    {
        this.value = value;
        switch (type) { 
            case UIType.Text:
                transform.GetComponent<TextMeshProUGUI>().text = value.ToString();
                break;
            case UIType.Slider:
                transform.GetComponent<Slider>().value = value;
                break;

                
        }

    }
}
