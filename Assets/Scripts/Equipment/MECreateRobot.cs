using System;
using UnityEngine;

namespace Equipment
{
    public class MECreateRobot : MonoBehaviour
    {
        private void OnEnable()
        {
            Events.AddListener(Events.M_CREATE_ROBOT, CreateRobot);
        }

        private void Update()
        {
            
        }

        public void CreateRobot(object[] args)
        {
            //确定是本基地建造才执行
            if (args[0] == gameObject.name)
            {
                /*根据传入模板（args[1]）建造车的底盘*/

                //建造完成后发出事件
                Events.Invoke(Events.F_ROBOT_CREATED, new object[] { gameObject.name });
            }
        }
    }
    
}