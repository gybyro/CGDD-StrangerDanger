using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Button getInfoButton;

    private void Start()
    {
        infoText.text = "Tap to view delivery info";
        getInfoButton.onClick.AddListener(OnGetInfoClicked);
    }

    private void OnGetInfoClicked()
    {
        GameManager.Instance.GenerateNextCustomer();
        infoText.text = GameManager.Instance.GetPhoneDescription();

        // Make the button disappear after first click
        getInfoButton.gameObject.SetActive(false);
        infoText.gameObject.SetActive(true);
        
    }
}
