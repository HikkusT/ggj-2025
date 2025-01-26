    using UnityEngine;

    public class BubbleTea
    {
        private Color _color;
        private AudioClip _audioClip;
        private float _bubbleQtt;

        public bool IsCorrect(Recipe recipe)
        {
            return _color == recipe.Color && _audioClip == recipe.AudioClip && _bubbleQtt >= recipe.BubbleQttStart && _bubbleQtt <= recipe.BubbleQttEnd;
        }
    }