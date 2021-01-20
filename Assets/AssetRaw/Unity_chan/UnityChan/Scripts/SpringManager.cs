//
//弹簧管理器，处理头发等的弹动效果
//
using UnityEngine;

namespace UnityChan
{
	public class SpringManager : MonoBehaviour
	{
		public float dynamicRatio = 1.0f;//柔软度，0-1之间，骨骼当前方向到目标方向的平滑度

		public float			stiffnessForce;//刚度力,整体硬度，越小像丝带，越大像钢筋
		public AnimationCurve	stiffnessCurve;//硬度变化曲线
		public float			dragForce;//力衰减，越大越丝滑，越小像多节棍
		public AnimationCurve	dragCurve;
		public SpringBone[] springBones;

		void Start ()
		{
			UpdateParameters ();
		}
	
		void Update ()
		{
            dynamicRatio = Mathf.Clamp(dynamicRatio, 0, 1);
            UpdateParameters();
        }
	
		private void FixedUpdate ()
		{
			//Kobayashi
			if (dynamicRatio != 0.0f) {
				for (int i = 0; i < springBones.Length; i++) {
					if (dynamicRatio > springBones [i].threshold) {
						springBones [i].UpdateSpring ();
					}
				}
			}
		}

		private void UpdateParameters ()
		{
			UpdateParameter ("stiffnessForce", stiffnessForce, stiffnessCurve);
			UpdateParameter ("dragForce", dragForce, dragCurve);
		}
	

		private void UpdateParameter (string fieldName, float baseValue, AnimationCurve curve)
		{
			var start = curve.keys [0].time;
			var end = curve.keys [curve.length - 1].time;
			//var step	= (end - start) / (springBones.Length - 1);
		
			//以反射获取字段，修改
			var prop = springBones [0].GetType ().GetField (fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		
			for (int i = 0; i < springBones.Length; i++) {
				//Kobayashi
				if (springBones [i].isUseEachBoneForceSettings) {
					var scale = curve.Evaluate(start + (end - start));// * i / (springBones.Length - 1));
					prop.SetValue (springBones [i], baseValue * scale);
				}
			}
		}
	}
}