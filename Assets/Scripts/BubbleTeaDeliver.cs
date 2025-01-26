using Cysharp.Threading.Tasks;
using UnityEngine;

public class BubbleTeaDeliver : MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;

    public void Deliver(BubbleTea bubbleTea)
    {
        orderManager.CompleteOrder().Forget();
        // if (bubbleTea.IsCorrect(orderManager.CurrentOrder.Recipe))
        // {
        //     orderManager.CompleteOrder().Forget();
        // }
    }
}
