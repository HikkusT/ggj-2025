using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public Color Color;
    public AudioClip AudioClip;
    public float BubbleQttStart;
    public float BubbleQttEnd;
    public string RecipeName;
}