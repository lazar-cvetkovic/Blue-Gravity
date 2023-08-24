using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ButtonSprites", menuName = "ScriptableObjects/ButtonSprites")]
public class ButtonSpritesSO : ScriptableObject
{
    public Sprite NormalSprite;
    public Sprite SelectedSprite;
    public Sprite PressedSprite;
    public Material SelectedShader;
}