using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteController : MonoBehaviour
{
    public Image portraitImage;
    public Animator portraitAnimator; // has SlideIn + SlideOut animations
    public CharacterPortraitDatabase portraitDB;

    public void ShowCharacter(string characterID)
    {
        portraitImage.sprite = portraitDB.GetDefaultPortrait(characterID);
        portraitAnimator.Play("SlideIn");
    }

    public void HideCharacter()
    {
        portraitAnimator.Play("SlideOut");
    }

    public void ChangeExpression(string characterID, string emotion)
    {
        portraitImage.sprite = portraitDB.GetPortrait(characterID, emotion);
    }
}