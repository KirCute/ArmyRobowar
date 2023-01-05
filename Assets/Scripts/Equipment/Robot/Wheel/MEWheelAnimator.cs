using UnityEngine;

namespace Equipment.Robot.Wheel {
    /// <summary>
    /// 根据机器人的运动情况生成轮子的骨骼动画
    /// </summary>
    public class MEWheelAnimator : MonoBehaviour {
        [SerializeField] private float linearEffect = -1.0f;  // 线速度系数，用来调整移动单位距离所转圈数
        [SerializeField] private float angularEffect = -1.0f;  // 角速度系数，用来调整转动单位距离所转圈数，左右轮该数值应当为相反数
        private Transform robot;
        private Vector3 lastPos;  // 上一帧位置
        private Vector3 lastRot;  // 上一帧朝向
        
        private void Awake() {
            robot = GetComponentInParent<Rigidbody>().transform;
            lastPos = robot.position;
            lastRot = robot.localEulerAngles;
        }

        private void Update() {
            var euler = transform.localEulerAngles;
            var pos = robot.position;
            var rot = robot.localEulerAngles;
            euler.y += Vector3.Dot(pos - lastPos, robot.forward) * linearEffect;  // 累了，直接取位移和向前单位向量点乘结果作为向前移动量
            euler.y += (rot.y - lastRot.y) * angularEffect;
            transform.localEulerAngles = euler;
            lastPos = pos;
            lastRot = rot;
        }
    }
}