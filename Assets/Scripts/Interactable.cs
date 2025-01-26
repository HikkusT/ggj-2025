using UnityEngine;

// [RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [SerializeField] Outline _outline;
    
    public void TogglePickUpEffect(bool isOn)
    {
        _outline.OutlineMode = isOn ? Outline.Mode.OutlineAll : Outline.Mode.OutlineHidden;
        _outline.enabled = isOn;
    }
}
