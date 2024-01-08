using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "New Capacity", menuName = "Pokemon/Capacity", order = 0)]
    public class CapacitySO : ScriptableObject
    {
        public string name;
        [TextArea] private string description;

        public bool attack = true;
        public Enums.Types attackType = Enums.Types.Normal;
        public bool specialCapacity; //Attack ou Attack spe
        public int maxPP = 20;
        public int power = 30;
        public int precision = 100;
        [Header("Effect")] 
        public bool selfEffect;
        public Enums.StatType statAffect;
        public bool isBonus;

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