using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoneForTheDayScript : MonoBehaviour
{
    public string sceneToLoad = "CarScene";
    
    public Button nextSceneButton;     // UI button

    [Header("Text")]
    public TMP_Text doneTodayText;
    public TMP_Text currentDayMoneyText;
    public TMP_Text comment;

    [Header("Audio")]
    public AudioSource audioSource;
    

    private int moneyToday;

    private void Awake()
    {
        doneTodayText.gameObject.SetActive(false);
        currentDayMoneyText.gameObject.SetActive(false);
        comment.gameObject.SetActive(false);
        nextSceneButton.gameObject.SetActive(false);
    }



    private void Start()
    {
        if (nextSceneButton != null)
            nextSceneButton.gameObject.SetActive(false);

        moneyToday = Money_Manager.Instance.currentDayMoney;
        SetMoneyText();
        setComment();
        GameManager.Instance.AdvanceTime();

        StartCoroutine(TextApyr());
    }
    private void SetMoneyText()
    {
        currentDayMoneyText.text = "You got $" + moneyToday + "!";
    }
    private void setComment()
    {
        if (moneyToday < 15) comment.text = "YIKES!";
        else if (moneyToday > 25) comment.text = "Guess I can finally afford toppings on my own pizza...";
        else comment.text = "I wish I was around back in 1987. At least then I could have had quirky animatronics with my subpar pizza and existential dread.";
    }
    

    private IEnumerator TextApyr()
    {  
        yield return new WaitForSeconds(0.2f);
        doneTodayText.gameObject.SetActive(true);
        PlayFromResources(audioSource, "snd_curtgunshot");

        yield return new WaitForSeconds(0.5f);
        currentDayMoneyText.gameObject.SetActive(true);
        PlayFromResources(audioSource, "snd_curtgunshot");

        yield return new WaitForSeconds(0.5f);
        comment.gameObject.SetActive(true);
        PlayFromResources(audioSource, "snd_curtgunshot");

        yield return new WaitForSeconds(1f);
        nextSceneButton.gameObject.SetActive(true);
        PlayFromResources(audioSource, "snd_curtgunshot");
        

    }

        // ===================== AUDIO =====================
    private void PlayFromResources(AudioSource cam, string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
            cam.PlayOneShot(clip);
    }
    private void PlayLoop(AudioSource source, string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
    }
}
