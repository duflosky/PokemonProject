using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "New Capacity", menuName = "Pokemon/Capacity", order = 0)]
    public class CapacitySO : ScriptableObject
    {
        public string name;
        [TextArea] private string description;
        
        public int maxPP;
        public Enums.Types attackType;

        public CapacityInstance GetInstance() => new CapacityInstance(this);
    }

    public class CapacityInstance
    {
        public CapacitySO so;

        public int maxPP;
        public int currentPP;
        
        public CapacityInstance(CapacitySO _so)
        {
            so = _so;
            maxPP = _so.maxPP;
            currentPP = maxPP;
        }
    }
}