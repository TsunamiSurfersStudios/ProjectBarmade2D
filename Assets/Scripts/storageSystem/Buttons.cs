using UnityEngine;

namespace storageSystem
{
    public class Buttons : MonoBehaviour
    {
        private 
            void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SpawnDrink()
        {
            GameObject drink = GameObject.Find("Drink");
            Instantiate(drink);
        
        }
    

    }
}
