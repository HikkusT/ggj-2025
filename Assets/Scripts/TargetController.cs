using UnityEngine;
using UnityEngine.UI;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject _interectIndicator;
    [SerializeField] private Image _targetImage;

    // Update is called once per frame
    void Update()
    {
        if (_interectIndicator.activeSelf)
        {
            _targetImage.enabled = false;
        }
        else
        {
            _targetImage.enabled = true;
        }
    }
}
