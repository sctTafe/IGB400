using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Enemy_PS_EffectsSpawn : MonoBehaviour
    {
        [SerializeField] bool DebugON = false;
        [SerializeField] Transform pointAtWhichToSpawnEffects;
        [SerializeField] GameObject Fire_PS_Prefab;
        [SerializeField] GameObject Smoke_PS_Prefab;
        [SerializeField] GameObject Explosion_PS_Prefab;

        private bool fireHasBeenSpawned;
        private bool smokeHasBeebnSpawned;
        private bool explosionHasBeebnSpawned;


        private void Start()
        {
            fireHasBeenSpawned = false;
            smokeHasBeebnSpawned = false;
            explosionHasBeebnSpawned = false;
        }

        public void fnc_SpawnFire_PS()
        {
            if (fireHasBeenSpawned == false)
            {
                if (Fire_PS_Prefab != null)
                {
                    if (DebugON) Debug.Log("::Enemy_PS_EffectsSpawn:: fnc_SpawnFire_PS; fireHasBeenSpawned ");
                    Instantiate(Fire_PS_Prefab, pointAtWhichToSpawnEffects.position, Quaternion.Euler(-90, 0, 0), this.gameObject.transform);
                    fireHasBeenSpawned = true;
                }
            }
        }

        public void fnc_SpawnSmoke_PS()
        {
            if (smokeHasBeebnSpawned == false)
            {
                if (Smoke_PS_Prefab != null)
                {
                    if (DebugON) Debug.Log("::Enemy_PS_EffectsSpawn:: fnc_SpawnSmoke_PS; smokeHasBeebnSpawned ");
                    Instantiate(Smoke_PS_Prefab, pointAtWhichToSpawnEffects.position, Quaternion.Euler(-90, 0, 0), this.gameObject.transform);
                    smokeHasBeebnSpawned = true;
                }
            }
        }

        public void fnc_SpawnExplosion_PS()
        {
            if (explosionHasBeebnSpawned == false)
            {
                if(Explosion_PS_Prefab != null)
                {
                    if (DebugON) Debug.Log("::Enemy_PS_EffectsSpawn:: fnc_SpawnExplosion_PS; explosionHasBeebnSpawned ");
                    GameObject ps = Instantiate(Explosion_PS_Prefab, pointAtWhichToSpawnEffects.position, Quaternion.Euler(-90, 0, 0));
                    Destroy(ps, 5);
                    explosionHasBeebnSpawned = true;

                }

            }
        }


    }
}
