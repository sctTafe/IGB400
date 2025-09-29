using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {

    /// <summary>
    /// Refs:
    /// Bit shit unity learning example.... https://learn.unity.com/tutorial/lists-and-dictionaries# 
    /// </summary>
    public class Projectiles_Pool : MonoBehaviour
    {

        [System.Serializable]
        public class ProPool
        {
            public string tag_OfPool;
            public GameObject prefab_OfPoolGameObjects;
            public int size_OfPool;
        }

        #region Singleton Of Projectiles_Pool   //Need to Ask Mark about this to confirm
        public static Projectiles_Pool Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion


        public List<ProPool> pools_List;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (ProPool pool_current in pools_List)
            {

                // for each 'pool' made, a queue of game objects is made 
                Queue<GameObject> objectPool_currentQueue = new Queue<GameObject>();

                // fills the pool with game objects
                for (int i = 0; i < pool_current.size_OfPool; i++)
                {
                    GameObject obj = Instantiate(pool_current.prefab_OfPoolGameObjects);
                    obj.SetActive(false);
                    objectPool_currentQueue.Enqueue(obj);  // 'Enqueue' adds the game object to the end of the pool queue
                }

                // adds the pool to the dictionary
                poolDictionary.Add(pool_current.tag_OfPool, objectPool_currentQueue);
            }

        }

        /// <summary>
        /// Equivalent to instantiating, but instead calls it from the pool; Remember to 'Enqueue' when finished with the object!
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public GameObject ProjectileDequeueFromPool(string tag, Vector3 position, Quaternion rotation)
        {

            if (!poolDictionary.ContainsKey(tag)) { Debug.LogWarning("Projectiles_Pool: Pool with tag " + tag + " doesn't exist"); return null; }

            RetryDequeued:

            if (poolDictionary[tag].Count > 0)
            {
                //If the are objects in the pool use them: Else...
                GameObject dequeuedObject = poolDictionary[tag].Dequeue();  // gets the gameobject from the front of the queue
                dequeuedObject.transform.position = position;
                dequeuedObject.transform.rotation = rotation;
                dequeuedObject.SetActive(true);
                //poolDictionary[tag].Enqueue(dequeuedObject);  //Adds it back to the pool (this means objects will disapear from the game if it comes back to this object without it been removed, may be best to only add it back only on destruction)
                return dequeuedObject;  // retuns it to the point it was spawned from
            } else
            {
                //if the are no object left in the pool add an extra... (this could be an issue if somthing is not working correctly)

                // Find the matching 'prefab' to the list, by comparing 'tag', then add an extra game object
               
                foreach (ProPool pool_current in pools_List){
                    if (pool_current.tag_OfPool == tag)
                    {
                        GameObject obj = Instantiate(pool_current.prefab_OfPoolGameObjects);
                        obj.SetActive(false);
                        poolDictionary[tag].Enqueue(obj);
                    }
                }

                //leave this Debug Log in!
                Debug.Log(":::Notification::: Added Additional Projectile to: '" + tag + "' poollist, in 'Projectiles_Pool'");

                // might not please Mark.... but its a nice lazy way, and lazy is good right???
                goto RetryDequeued;
            }
        }

        public void AddToPool(GameObject gameObject, string ProjectilePooltag)
        {
            //Debug.Log(":::OP::: ObjectPool: '" + ProjectilePooltag + "', Current Count = " + poolDictionary[ProjectilePooltag].Count);
            poolDictionary[ProjectilePooltag].Enqueue(gameObject);
            
        }


        private void Test(string tag)
        {
            GameObject prefabMatch; 
            foreach (ProPool pool_current in pools_List)
            {
                if(pool_current.tag_OfPool == tag){
                    prefabMatch = pool_current.prefab_OfPoolGameObjects;
                }
            }
            
        }

    }
}
