using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class PooledParticalsActivation_Singleton : Singleton<PooledParticalsActivation_Singleton>
    {

        // The Projectile Pool Instance
        private Projectiles_Pool projectiles_Pool;

        private void Awake()
        {
            projectiles_Pool = Projectiles_Pool.Instance;   // calling a 'singleton' version of the class that is running  :::: check with Mark that is is correct & the correct way of doing it ::::

        }


        public GameObject fn_DequeueParticalEffect_ByTag(string tag_PooledTagName, Vector3 position, Quaternion rotation)
        {
            GameObject go = new GameObject(tag_PooledTagName);

            if (projectiles_Pool != null)
            {
                GameObject projectileGO = projectiles_Pool.ProjectileDequeueFromPool(tag_PooledTagName, position, rotation);
                go = projectileGO;
            } 
            else
            {
                projectiles_Pool = Projectiles_Pool.Instance;
                if (projectiles_Pool != null)
                {
                    GameObject projectileGO = projectiles_Pool.ProjectileDequeueFromPool(tag_PooledTagName, position, rotation);
                    go = projectileGO;
                }
            }

            return go;
        }

        









    }
}