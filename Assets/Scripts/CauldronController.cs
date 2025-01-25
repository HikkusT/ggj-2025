using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    [SerializeField] private Color _initialCauldronColor;
    [SerializeField] private Renderer _liquidRenderer;
    [SerializeField] private Light _cauldronLight;
    [SerializeField] private ParticleSystem _bubbleParticles;
    
    [Header("Transitions")]
    [SerializeField] private float _transitionDurationInMillis;
    
    [Header("Debug")]
    [SerializeField] Ingredient _debugIngredient;
    
    private readonly Dictionary<Ingredient, IngredientTracking> _processingIngredients = new ();
    private Material _liquidMaterial => _liquidRenderer.material;
    float _currentTime = 0;
    
    void Update()
    {
        if (_processingIngredients.Count <= 0) return;
        
        _currentTime += Time.deltaTime;
        foreach ((Ingredient ingredient, IngredientTracking tracking) in _processingIngredients)
        {
            foreach (IngredientEffect effect in ingredient.IngredientEffects)
            {
                if (tracking.ProcessedEffects.Contains(effect)) continue;
                if (_currentTime - tracking.TimeWhenAdded < effect.Time) continue;
                
                ApplyEffect(effect);
                tracking.TrackProcessed(effect);
            }
        }
    }
    
    public void AddIngredient(Ingredient ingredient)
    {
        if (_processingIngredients.ContainsKey(ingredient)) return;
        if (_processingIngredients.Count == 0)
        {
            _currentTime = 0;
        }
        
        _processingIngredients.Add(ingredient, new IngredientTracking(_currentTime));
    }

    void ApplyEffect(IngredientEffect effect)
    {
        if (effect.Color != null)
        {
            UpdateColor(effect.Color.Value, this.GetCancellationTokenOnDestroy()).Forget();
        }

        if (effect.BubblesIntensity != null)
        {
            // UpdateBubbles()
        }
        else
        {
            
        }
    }

    async UniTask UpdateColor(Color targetColor, CancellationToken ct)
    {
        Color beforeTransitionColor = _liquidMaterial.color;
        
        DateTime start = DateTime.Now;
        do
        {
            await UniTask.Yield(ct);
            Color currentColor = Color.Lerp(beforeTransitionColor, targetColor,
                Mathf.Clamp01((float)(DateTime.Now - start).TotalMilliseconds / _transitionDurationInMillis));
            _liquidMaterial.color = currentColor;
            _cauldronLight.color = currentColor;
        } while (DateTime.Now - start < TimeSpan.FromSeconds(_transitionDurationInMillis));
    }
    
    async UniTask UpdateBubbles(AnimationCurve bubbleCurves, float startedAt, CancellationToken ct)
    {
        // float beforeTransitionRate = _bubbleParticles.emission.rateOverTime.constant;
        //
        // DateTime start = DateTime.Now;
        // do
        // {
        //     await UniTask.Yield(ct);
        //     Color currentColor = Color.Lerp(beforeTransitionColor, targetColor,
        //         Mathf.Clamp01((float)(DateTime.Now - start).TotalMilliseconds / _transitionDurationInMillis));
        //     _liquidMaterial.color = currentColor;
        //     _cauldronLight.color = currentColor;
        // } while (DateTime.Now - start < TimeSpan.FromSeconds(_transitionDurationInMillis));
        //
        // while (!ct.IsCancellationRequested)
        // {
        //     _
        // }
    }
    
    
    [Button]
    public void DebugAddIngredient() => AddIngredient(_debugIngredient);

    private class IngredientTracking
    {
        public readonly float TimeWhenAdded;
        private readonly List<IngredientEffect> _processedEffects = new();
        public IReadOnlyList<IngredientEffect> ProcessedEffects => _processedEffects;
        public CancellationTokenSource Cts { get; private set; } = new(); 

        public IngredientTracking(float timeWhenAdded)
        {
            TimeWhenAdded = timeWhenAdded;
        }

        public void TrackProcessed(IngredientEffect effect)
        {
            _processedEffects.Add(effect);
            Cts.Cancel();
            Cts = new CancellationTokenSource();
        }
    }
}
