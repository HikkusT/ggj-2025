using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] private List<IngredientEffect> _ingredientEffects;
    
    public List<IngredientEffect> IngredientEffects => _ingredientEffects;
}

[Serializable]
public class IngredientEffect
{
    [SerializeField] private float _time;
    
    [SerializeField] private bool _affectsColor;
    [SerializeField, ShowIf(nameof(_affectsColor))] private Color _color;
    
    [SerializeField] private bool _affectsBubbles;
    [SerializeField, ShowIf(nameof(_affectsBubbles))] private AnimationCurve _bubblesIntensity;

    [SerializeField] private bool _affectsSound;
    [SerializeField, ShowIf(nameof(_affectsSound))] private AudioClip _sound;
    
    public float Time => _time;
    
    public Color? Color => _affectsColor ? _color : null;
    [CanBeNull] public AudioClip Sound => _affectsSound ? _sound : null;

    [CanBeNull] public AnimationCurve BubblesIntensity => _affectsBubbles ? _bubblesIntensity : null;
}
