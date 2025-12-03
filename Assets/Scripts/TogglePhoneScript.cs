using UnityEngine;

public class TogglePhoneScript : MonoBehaviour
{
public GameObject imageUnchecked;   // PhoneDown
    public GameObject imageChecked;     // PhoneUp
    public GameObject extraObject;      // The 3rd object you want to toggle

    public void SelectChecked()
    {
        imageUnchecked.SetActive(false);  // Hide PhoneDown

        imageChecked.SetActive(true);     // Show PhoneUp
        extraObject.SetActive(true);      // Show the 3rd object
    }

    public void SelectUnchecked()
    {
        imageUnchecked.SetActive(true);   // Show PhoneDown
        imageChecked.SetActive(false);    // Hide PhoneUp
        extraObject.SetActive(false);     // Hide the 3rd object
    }
}
