using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SO
{
    
    [CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon/Pokemon", order = 0)]
    public class PokemonSO : ScriptableObject
    {
        
        public string name;
        [TextArea] public string description;
        [Space] 
        public Enums.Types type;
        public Enums.Types secondType;
        [Header("Stats")] 
        public int baseMaxHp;
        public int baseAttack;
        public int baseDefense;
        public int baseAttackSpe;
        public int baseDefenseSpe;
        public int baseSpeed;
        [Header("Gain Stats")]
        public int HpGain = 3;
        public int attackGain = 2;
        public int defenseGain = 1;
        public int attackSpeGain = 2;
        public int defenseSpeGain = 2;
        public int speedGain = 2;
        [Space] 
        public LevelCapacity[] learnCapacities;
        [Header("ProgressCurves")] 
        public AnimationCurve experienceCurve = AnimationCurve.Linear(1,15,100,10000);

        [Header("Sprites")] 
        public Sprite facePokemonSprite;
        public Sprite backPokemonSprite;
        public Sprite[] animationFrames;

        public PokemonInstance GetInstance() => new PokemonInstance(this);
        public bool ContainType(Enums.Types _type) => _type == type || _type == secondType;
    }

    [Serializable]
    public struct LevelCapacity
    {
        public CapacitySO capacity;
        public int level;
    }

    public class PokemonInstance
    {
        public PokemonSO so;
        
        public string Name;
        public string description;

        public int maxHp;
        public int attack;
        public int defense;
        public int attackSpe;
        public int defenseSpe;
        public int speed;
        
        //Dynamic values
        public int level;
        public int totalExpNeed;
        public int currentExp;
        
        public int currentHp;

        public CapacityInstance[] capacities;
        
        public PokemonInstance(PokemonSO _so)
        {
            so = _so;
            Name = _so.name;
            description = _so.description;

            maxHp = _so.baseMaxHp;
            attack = _so.baseAttack;
            defense = _so.baseDefense;
            attackSpe = _so.baseAttackSpe;
            defenseSpe = _so.baseDefenseSpe;
            speed = _so.baseSpeed;

            capacities = new CapacityInstance[4];
            
            currentHp = maxHp;
            level = 1;
            SetupCapacity();
        }
        public PokemonInstance(PokemonSO _so, int _level) : this(_so)
        {
            for (int i = level; i < _level; i++)
            {
                LevelUpStats();
            }
            level = _level;
            totalExpNeed = (int)so.experienceCurve.Evaluate(level);
            SetupCapacity();
        }

        void SetupCapacity()
        {
            var capacityToSet = so.learnCapacities.Where(capa => capa.level <= level).ToArray();
            for (int i = 0; i < capacityToSet.Length; i++)
            {
                if(capacities[3] != null)break;
                if(!IsCapacityLearn(capacityToSet[i].capacity))AddCapacity(capacityToSet[i].capacity.GetInstance());
            }
        }

        public int GetCapacityAmount()
        {
            int count = 0;

            foreach (var capacity in capacities)
            {
                if (capacity != null) count++;
            }
            return count;
        }
        bool IsCapacityLearn(CapacitySO _capacity)
        {
            if (capacities == null) return false;
            return capacities.Any(capacity => capacity?.so == _capacity);
        }

        void AddCapacity(CapacityInstance capacity, int index = Int32.MaxValue)
        {
            if (index == Int32.MaxValue)
            {
                if (capacities[3] != null) return;
                for (int i = 0; i < capacities.Length; i++)
                {
                    if(capacities[i] != null)continue;
                    capacities[i] = capacity;
                    break;
                }
            }
            else
            {
                if (capacities[index] != null) return;
                capacities[index] = capacity;
            }
        }

        public void LevelUp()
        {
            level++;
            currentExp = 0;
            totalExpNeed = (int)so.experienceCurve.Evaluate(level);
            LevelUpStats();
        }
        
        private void LevelUpStats()
        {
            maxHp += so.HpGain;
            currentHp += so.HpGain;
            attack += so.attackGain;
            defense += so.defenseGain;
            attackSpe += so.attackSpeGain;
            defenseSpe += so.defenseSpeGain;
            speed += so.speedGain;
        }
        
    }
}