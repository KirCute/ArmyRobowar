using Model.Inventory;
using UnityEngine;

namespace System
{
    public class MECreatePickable : MonoBehaviour
    {
        //创造掉落物
        private void OnEnable()
        {
            Events.AddListener(Events.F_CREATE_ITEM, CreatePickable);
        }

        private void OnDisable()
        {
            Events.RemoveListener(Events.F_CREATE_ITEM, CreatePickable);
        }
        
        void CreatePickable(object[] args)
        {
            if (Summary.team.teamColor == (int)args[0])
            {
                for (int i = 0; i < Summary.team.robots[(int)args[1]].equippedComponents.Length; i++)
                {
                    SensorItemAdapter itemAdapter =new SensorItemAdapter(Summary.team.robots[(int)args[1]].equippedComponents[i]);
                }
            }
        }
    }
}