using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] Ingredient _ingredient;
        
        public Ingredient Ingredient => _ingredient;
        
        public Pickable PickObject()
        {
            GameObject clone = Instantiate(gameObject);
            Destroy(clone.GetComponent<Interactable>());
            Destroy(clone.GetComponent<Collider>());

            GetComponent<Outline>().enabled = false;
            
            return clone.GetComponent<Pickable>();
        }
    }
}