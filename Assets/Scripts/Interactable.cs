using System;
using NaughtyAttributes;
using UnityEngine;

// [RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [SerializeField] Outline _outline;
    [SerializeField] bool _isoutlineAlwaysOn;
    [SerializeField, ShowIf(nameof(_isoutlineAlwaysOn))] Color _baseColor;
    [SerializeField, ShowIf(nameof(_isoutlineAlwaysOn))] Color _targetedColor;
    
    public void TogglePickUpEffect(bool isOn)
    {
        if (!_isoutlineAlwaysOn)
        {
            _outline.OutlineMode = isOn ? Outline.Mode.OutlineAll : Outline.Mode.OutlineHidden;
            _outline.enabled = isOn;
        }
        else
        {
            if (isOn)
            {
                _outline.OutlineColor = _targetedColor;
                _outline.OutlineWidth = 20f;
            }
            else
            {
                _outline.OutlineColor = _baseColor;
                _outline.OutlineWidth = 5f;
            }
        }
    }

    private void OnEnable()
    {
        if (_isoutlineAlwaysOn)
        {
            _outline.enabled = true;
            _outline.OutlineMode = Outline.Mode.OutlineAll;
            _outline.OutlineColor = _baseColor;
            _outline.OutlineWidth = 5f;
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    void OnDisable()
    {
        if (_isoutlineAlwaysOn)
        {
            _outline.enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
