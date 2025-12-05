using UnityEngine;

public class TogglePhoneScript : MonoBehaviour
{
public GameObject imageUnchecked;   // PhoneDown
    public GameObject imageChecked;     // PhoneUp
    public GameObject extraObject;      // The 3rd object you want to toggle
    public GameObject extraObject2; 
    public GameObject LeaveButton;

    public void SelectChecked()
    {
        imageUnchecked.SetActive(false);  // Hide PhoneDown

        imageChecked.SetActive(true);     // Show PhoneUp
        extraObject.SetActive(true);      // Show the 3rd object
        extraObject2.SetActive(false);
        LeaveButton.SetActive(true);
    }

    public void SelectUnchecked()
    {
        imageUnchecked.SetActive(true);   // Show PhoneDown
        imageChecked.SetActive(false);    // Hide PhoneUp
        extraObject.SetActive(false);     // Hide the 3rd object
        LeaveButton.SetActive(true);
        extraObject2.SetActive(true);
    }
}
