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
    [SerializeField] private AudioSource _audioSource;
    
    [Header("Transitions")]
    [SerializeField] private float _transitionDurationInMillis;
    
    [Header("Debug")]
    [SerializeField] Ingredient _debugIngredient;
    
    private readonly Dictionary<Ingredient, IngredientTracking> _processingIngredients = new ();
    private Material _liquidMaterial => _liquidRenderer.material;
    float _currentTime = 0;

    private void Start()
    {
        var emission = _bubbleParticles.emission;
        emission.rateOverTime = 0;
        ResetCauldron();
    }

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
                
                tracking.TrackProcessed(effect);
                ApplyEffect(effect, tracking.Cts.Token);
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

    public void Flush()
    {
        foreach ((_, IngredientTracking tracking) in _processingIngredients)
        {
            tracking.Cts.Cancel();
        }
        _processingIngredients.Clear();
        
        ResetCauldron();
    }

    public BubbleTea FinishCooking()
    {
        if (_processingIngredients.Count == 0) return null;
        
        foreach ((_, IngredientTracking tracking) in _processingIngredients)
        {
            tracking.Cts.Cancel();
        }
        _processingIngredients.Clear();

        var emission = _bubbleParticles.emission;
        var result = new BubbleTea(_liquidMaterial.color, _audioSource.clip, emission.rateOverTime.constant);

        ResetCauldron();
        
        return result;
    }

    void ApplyEffect(IngredientEffect effect, CancellationToken ct)
    {
        if (effect.Color != null)
        {
            UpdateColor(effect.Color.Value, ct).Forget();
        }

        if (effect.Sound != null)
        {
            UpdateSound(effect.Sound);
        }
        
        UpdateBubbles(effect.BubblesIntensity, _currentTime, ct).Forget();
    }

    void UpdateSound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.loop = true;
        _audioSource.Play();
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
            var mainModule = _bubbleParticles.main;
            mainModule.startColor = currentColor;
        } while (DateTime.Now - start < TimeSpan.FromMilliseconds(_transitionDurationInMillis));
    }
    
    async UniTask UpdateBubbles(AnimationCurve bubbleCurves, float startedAt, CancellationToken ct)
    {
        float beforeTransitionRate = _bubbleParticles.emission.rateOverTime.constant;
        float target = bubbleCurves?.Evaluate(0) ?? 0f;
        
        DateTime start = DateTime.Now;
        do
        {
            await UniTask.Yield(ct);
            float currentEmissionRate = Mathf.Lerp(beforeTransitionRate, target,
                Mathf.Clamp01((float)(DateTime.Now - start).TotalMilliseconds / _transitionDurationInMillis));
            var emission = _bubbleParticles.emission;
            emission.rateOverTime = currentEmissionRate;
        } while (DateTime.Now - start < TimeSpan.FromMilliseconds(_transitionDurationInMillis));
        
        if (bubbleCurves == null) return;
        
        while (!ct.IsCancellationRequested)
        {
            var emission = _bubbleParticles.emission;
            emission.rateOverTime = bubbleCurves.Evaluate(_currentTime - startedAt);
            await UniTask.Yield(ct);
        }
    }

    public void ResetCauldron()
    {
        _liquidMaterial.color = _initialCauldronColor;
        _cauldronLight.color = _initialCauldronColor;
        var emission = _bubbleParticles.emission;
        emission.rateOverTime = 0;
        var mainModule = _bubbleParticles.main;
        mainModule.startColor = _initialCauldronColor;
        _audioSource.Stop();
        _audioSource.clip = null;
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
