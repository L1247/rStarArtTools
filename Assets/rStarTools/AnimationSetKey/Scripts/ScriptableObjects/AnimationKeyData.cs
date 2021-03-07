using UnityEngine;

namespace rStarTools.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AnimationKeyData" , menuName = "rStarTool/AnimationKeyData" , order = 0)]
    public class AnimationKeyData : ScriptableObject
    {
        [SerializeField]
        private float minValue;
        [SerializeField]
        private float maxValue;

        [SerializeField]
        public string propertyName;

        public float GetValue()
        {
            return Random.Range(minValue,maxValue);
        }
    }
}