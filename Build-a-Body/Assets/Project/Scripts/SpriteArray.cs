using UnityEngine;

[CreateAssetMenu(fileName = "New Asset Array", menuName = "2D/Sprite Array", order = 999)]
public class SpriteArray : ScriptableObject
{

    public Sprite[] sprites;
    public float animationFps = 30;
    public bool lingerOnLastSprite = false;
    public bool loop;
    public Color uiImageColor;

}
