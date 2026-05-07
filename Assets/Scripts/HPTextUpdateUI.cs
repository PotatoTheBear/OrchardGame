using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPTextUpdateUI : MonoBehaviour
{
    GameObject parentObj;
    Slider parentSlider;
    TextMeshProUGUI text;
    int currentValue => Mathf.RoundToInt(parentSlider.value);
    int maxValue => Mathf.RoundToInt(parentSlider.maxValue);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        parentObj = transform.parent.gameObject;
        parentSlider = parentObj.GetComponent<Slider>();
        parentSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        StartCoroutine(FruitTree.WaitForValueChange(() => maxValue, delegate { OnValueChanged(); }));
        yield return null;
        OnValueChanged();
    }

    void OnValueChanged()
    {
        text.text = $"{currentValue}/{maxValue} " + text.text.Split(" ")[1];
    }
}
