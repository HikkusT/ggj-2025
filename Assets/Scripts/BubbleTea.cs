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

        public bool IsCorrect(Recipe recipe)
        {
            return Color == recipe.Color && AudioClip == recipe.AudioClip && BubbleQtt >= recipe.BubbleQttStart && BubbleQtt <= recipe.BubbleQttEnd;
        }
    }