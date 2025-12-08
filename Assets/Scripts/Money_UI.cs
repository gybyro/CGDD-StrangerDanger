using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Money_UI : MonoBehaviour
{
    private TMP_Text MoneyText;

    private void Awake()
    {
        MoneyText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        MoneyText.text = "$" + Money_Manager.Instance.money;
    }
}