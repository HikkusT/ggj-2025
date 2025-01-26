using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerPickupController : MonoBehaviour
    {
        [SerializeField] private Transform _slot;
        [SerializeField] private GameObject _bubbleTeaView;
        
        Interactable _currentInteractable;
        Pickable _grabbedPickable;
        BubbleTea _bubbleTea;
        
        void Update()
        {
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
                    _grabbedPickable = grabbedPickable;
                }
                else if (_grabbedPickable != null && _currentInteractable.TryGetComponent(out CauldronController cauldronController))
                {
                    cauldronController.AddIngredient(_grabbedPickable.Ingredient);
                    Destroy(_grabbedPickable.gameObject);
                    _grabbedPickable = null;
                }
                else if (_bubbleTea == null && _currentInteractable.TryGetComponent(out CauldronController cauldronController2))
                {
                    BubbleTea bubbleTea = cauldronController2.FinishCooking();
                    if (bubbleTea != null)
                    {
                        _bubbleTeaView.SetActive(true);
                        _bubbleTea = bubbleTea;
                    }
                }
                else if (_bubbleTea != null && _currentInteractable.TryGetComponent(out BubbleTeaDeliver bubbleTeaDeliver))
                {
                    bubbleTeaDeliver.Deliver(_bubbleTea);
                    _bubbleTeaView.SetActive(false);
                    _bubbleTea = null;
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
            }
            else if (_currentInteractable != null)
            {
                _currentInteractable.TogglePickUpEffect(false);
                _currentInteractable = null;
            }
        }
    }
}