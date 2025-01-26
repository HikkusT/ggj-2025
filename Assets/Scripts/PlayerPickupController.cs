using System;
using NaughtyAttributes;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerPickupController : MonoBehaviour
    {
        [SerializeField] private Transform _slot;
        [SerializeField] private GameObject _bubbleTeaView;
        [SerializeField] private GameObject _interectionIcon;
        [SerializeField] private GameObject _deleteTooltip;
        
        [Header("Debug")]
        [SerializeField, ShowIf(nameof(HasBubbleTea))] private Color _color;
        [SerializeField, ShowIf(nameof(HasBubbleTea))] private AudioClip _clip;
        [SerializeField, ShowIf(nameof(HasBubbleTea))] private float _bubbleQuantity;
        
        Interactable _currentInteractable;
        Pickable _grabbedPickable;
        BubbleTea _bubbleTea;
        
        public bool HasBubbleTea => _bubbleTea != null;

        private void Start()
        {
            _color = Color.black;
            _clip = null;
            _bubbleQuantity = 0;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_grabbedPickable != null)
                {
                    Destroy(_grabbedPickable.gameObject);
                    _grabbedPickable = null;
                    _deleteTooltip.SetActive(false);
                }

                if (_currentInteractable != null &&
                    _currentInteractable.TryGetComponent(out CauldronController cauldronController))
                {
                    cauldronController.Flush();
                }
            }
            
            if (Input.GetMouseButtonDown(0) && _currentInteractable != null)
            {
                if (_grabbedPickable == null && _bubbleTea == null && _currentInteractable.TryGetComponent(out Pickable pickable))
                {
                    _currentInteractable.TogglePickUpEffect(false);
                    
                    Pickable grabbedPickable = pickable.PickObject();
                    grabbedPickable.transform.parent = _slot;
                    grabbedPickable.transform.localPosition = Vector3.zero;
                    grabbedPickable.transform.localRotation = Quaternion.identity;
                    
                    // Vector3 size = grabbedPickable.GetComponent<Renderer>().bounds.size;
                    // _slot.localScale = (0.5f / Mathf.Max(size.x, Mathf.Max(size.y, size.z))) * Vector3.one;

                    _currentInteractable = null;
                    _interectionIcon.SetActive(false);
                    _grabbedPickable = grabbedPickable;
                    _deleteTooltip.SetActive(true);
                }
                else if (_grabbedPickable != null && _currentInteractable.TryGetComponent(out CauldronController cauldronController))
                {
                    cauldronController.AddIngredient(_grabbedPickable.Ingredient);
                    Destroy(_grabbedPickable.gameObject);
                    _grabbedPickable = null;
                    _deleteTooltip.SetActive(false);
                }
                else if (_bubbleTea == null && _currentInteractable.TryGetComponent(out CauldronController cauldronController2))
                {
                    BubbleTea bubbleTea = cauldronController2.FinishCooking();
                    if (bubbleTea != null)
                    {
                        _bubbleTeaView.SetActive(true);
                        _bubbleTea = bubbleTea;
                        
                        _color = _bubbleTea.Color;
                        _clip = _bubbleTea.AudioClip;
                        _bubbleQuantity = _bubbleTea.BubbleQtt;
                    }
                }
                else if (_bubbleTea != null && _currentInteractable.TryGetComponent(out BubbleTeaDeliver bubbleTeaDeliver))
                {
                    bubbleTeaDeliver.Deliver(_bubbleTea);
                    _bubbleTeaView.SetActive(false);
                    _bubbleTea = null;
                    
                    _color = Color.black;
                    _clip = null;
                    _bubbleQuantity = 0;
                }
            }
            
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 5f)) return;

            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                if (_currentInteractable == interactable) return;

                if (_currentInteractable != null)
                {
                    _currentInteractable.TogglePickUpEffect(false);
                }
                
                interactable.TogglePickUpEffect(true);
                _currentInteractable = interactable;
                _interectionIcon.SetActive(true);
            }
            else if (_currentInteractable != null)
            {
                _currentInteractable.TogglePickUpEffect(false);
                _currentInteractable = null;
                _interectionIcon.SetActive(false);
            }
        }
    }
}