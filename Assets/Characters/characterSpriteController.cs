using UnityEngine;
using UnityEngine.UI;

public class CharacterSpriteController : MonoBehaviour
{
    public Image portraitImage;
    public Animator portraitAnimator; // has SlideIn + SlideOut animations
    public CharacterSpriteDatabase spriteDB;

    public void ShowCharacter(string characterID)
    {
        portraitImage.sprite = spriteDB.GetDefaultPortrait(characterID);
        portraitAnimator.Play("SlideIn");
    }

    public void HideCharacter()
    {
        portraitAnimator.Play("SlideOut");
    }

    public void ChangeExpression(string characterID, string emotion)
    {
        portraitImage.sprite = spriteDB.GetPortrait(characterID, emotion);
    }
}