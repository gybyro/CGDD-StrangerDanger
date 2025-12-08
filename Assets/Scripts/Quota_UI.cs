using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Quota_UI : MonoBehaviour
{
    private TMP_Text QuotaText;

    private void Awake()
    {
        QuotaText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        QuotaText.text = "$" + Money_Manager.Instance.quota;
    }
}