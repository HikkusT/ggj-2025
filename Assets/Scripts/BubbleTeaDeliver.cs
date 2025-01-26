using Cysharp.Threading.Tasks;
using UnityEngine;

public class BubbleTeaDeliver : MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioSource audioSource;

    public void Deliver(BubbleTea bubbleTea)
    {
        if (bubbleTea.IsCorrect(orderManager.CurrentOrder.Recipe))
        {
            audioSource.PlayOneShot(correctSound);
            orderManager.CompleteOrder().Forget();
        }
        else
        {
            orderManager.FailOrder();
            audioSource.PlayOneShot(failSound);
        }
    }
}
