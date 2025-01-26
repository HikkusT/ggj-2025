using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Pulsable : MonoBehaviour
{
    public Image uiElement; // O elemento de UI que vai piscar
    public Color flashColor = Color.red; // A cor do "piscar"
    public float flashDuration = 0.5f; // Tempo para alternar as cores
    public int flashCount = 3; // Quantas vezes vai piscar

    private Color originalColor; // Para restaurar a cor original

    void Start()
    {
        if (uiElement != null)
        {
            originalColor = uiElement.color; // Salva a cor original
        }
    }

    public void FlashUI()
    {
        if (uiElement == null) return;

        // Faz o elemento piscar `flashCount` vezes entre a cor original e o flashColor
        uiElement.DOColor(flashColor, flashDuration / 2)
            .SetLoops(flashCount * 2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                uiElement.color = originalColor; // Restaura a cor original ao final
            });
    }
}