using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    public class SetPlayerPosition_StartRespawn : MonoBehaviour
    {
        [SerializeField] Transform player_transfrom;
        [SerializeField] List<GameObject> inActiveDrones;
        private int listPositionIndex;

        private void Start()
        {
            listPositionIndex = 0;
            fnc_movePlayerToNextDrone(); //Called at the start of the game
        }

        public void fnc_movePlayerToNextDrone()
        {
            if (inActiveDrones.Count > 0)
            {
                if ((listPositionIndex) < inActiveDrones.Count)
                {
                    Transform transform = inActiveDrones[listPositionIndex].transform;
                    player_transfrom.position = transform.position;
                    player_transfrom.rotation = transform.rotation;
                    Destroy(inActiveDrones[listPositionIndex]);
                    listPositionIndex++;
                }

            }
        }


    }
}