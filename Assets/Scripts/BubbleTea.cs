    using UnityEngine;

    public class BubbleTea
    {
        public readonly Color Color;
        public readonly AudioClip AudioClip;
        public readonly float BubbleQtt;

        public BubbleTea(Color color, AudioClip audioClip, float bubbleQtt)
        {
            Color = color;
            AudioClip = audioClip;
            BubbleQtt = bubbleQtt;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public bool IsCorrect(Recipe recipe)
        {
            Debug.Log(Color == recipe.Color && AudioClip == recipe.AudioClip && BubbleQtt >= recipe.BubbleQttStart && BubbleQtt <= recipe.BubbleQttEnd);
            Debug.Log(recipe.Color);
            Debug.Log(recipe.AudioClip);
            Debug.Log(recipe.BubbleQttStart);
            Debug.Log(recipe.BubbleQttEnd);
            Debug.Log("---------------");
            
            Debug.Log(Color);
            Debug.Log(AudioClip);
            Debug.Log(BubbleQtt);
            return Color == recipe.Color && AudioClip == recipe.AudioClip && BubbleQtt >= recipe.BubbleQttStart && BubbleQtt <= recipe.BubbleQttEnd;
        }
    }