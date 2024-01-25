using System.Collections.Generic;
using System.Threading.Tasks;
using Objects;
using SO;
using UI;
using UI.Fight;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager
{
    public class FightManager : MonoBehaviour
    {
        public static FightManager Instance;

        private UIFight ui;

        [SerializeField] private PokemonSprite enemyPokemon;
        [SerializeField] private PokemonSprite allyPokemon;
        [Space]
        [SerializeField]private PokemonSO enemyDebug;
        [Space,SerializeField] private int enemyLevel = 1;

        [Header("Param")]
        [SerializeField] private TypesRelationSO typesRelations;
        [SerializeField] float enterFightEaseDuration = 0.2f;
        
        [HideInInspector]public List<PokemonInstance> enemyPokemons;

        private bool inFight;

        public PokemonInstance currentAllyPokemon { get; private set; }
        private PokemonInstance currentEnemyPokemon;
        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            ui = UIManager.Instance.uiFight;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                var enemy = new PokemonInstance(enemyDebug, enemyLevel);
                
                LaunchFight(enemy);
            }
        }

        public void LaunchFight(PokemonInstance enemyPokemon) => LaunchFight(new List<PokemonInstance> { enemyPokemon });
        public async void LaunchFight(List<PokemonInstance> enemyPokemonTeam)
        {
            inFight = true;
            GameManager.Instance.player.IsInFight = true;
            enemyPokemons = enemyPokemonTeam;
            await GameManager.Instance.GoToScene(enterFightEaseDuration);
            ui.StartFight();
            SetPokemon(GameManager.Instance.team[0], true);
            SetPokemon(enemyPokemons[0], false);
        }

        void SetPokemon(PokemonInstance pokemon, bool ally)
        {
            if (ally)
            {
                allyPokemon.spriteRenderer.sprite = pokemon.so.backPokemonSprite;
                allyPokemon.StartJiggle();
                currentAllyPokemon = pokemon;
            }
            else
            {
                enemyPokemon.spriteRenderer.sprite = pokemon.so.facePokemonSprite;
                currentEnemyPokemon = pokemon;
            }
            ui.DisplayPokemonInfo(pokemon,ally);
        }


        public async void ProcessTurn(int capacityUseIndex)
        {
            allyPokemon.StopJiggle();
            ui.StartTurn();
            
            var allyCapacityUse = currentAllyPokemon.capacities[capacityUseIndex];
            var enemyCapacityUse = currentEnemyPokemon.capacities[Random.Range(0, currentEnemyPokemon.GetCapacityAmount())];
            allyCapacityUse.currentPP--;
            enemyCapacityUse.currentPP--;

            var allyFirst = currentAllyPokemon.speed > currentEnemyPokemon.speed;
            var firstPokemon = allyFirst ? currentAllyPokemon : currentEnemyPokemon;
            var secondPokemon = !allyFirst ? currentAllyPokemon : currentEnemyPokemon;
            
            await ProcessAttack((allyFirst? allyCapacityUse : enemyCapacityUse).so, firstPokemon, secondPokemon, allyFirst);
           
            if(secondPokemon.currentHp>0)await ProcessAttack((allyFirst?enemyCapacityUse :allyCapacityUse).so, secondPokemon, firstPokemon, !allyFirst);

            if (currentAllyPokemon.currentHp <= 0)
            {
                await ProcessAllyKO();
            }

            if (currentEnemyPokemon.currentHp <= 0)
            {
                await ProcessEnemyKO();
            }
            if(!inFight)return;
            
            ui.EndTurn();
            allyPokemon.StartJiggle();
        }

        async Task ProcessAttack(CapacitySO capacity, PokemonInstance attacker, PokemonInstance defender, bool allyAttack)
        {
            var attackMessage = $"{attacker.so.name} utilise {capacity.name} !";
            await ui.LogMessage(attackMessage);

            if(attacker == currentAllyPokemon) await allyPokemon.Attack();
            else await enemyPokemon.UseEffect();

            var damageDeal = CalculateDamage(capacity, attacker, defender);
            Debug.Log($"Deal {damageDeal} damage");
            defender.currentHp -= damageDeal;
            if (defender.currentHp < 0) defender.currentHp = 0;

            await ui.UpdateLife(allyAttack);

        }
        
        async Task ProcessAllyKO()
        {
            var koMessage = $"{currentAllyPokemon.Name} est KO !";
            var endMessage = "Vous fuyez le combat . . .";
            await ui.LogMessage(koMessage, true);
            await ui.LogMessage(endMessage);
            QuitFight();
            currentAllyPokemon.currentHp = currentAllyPokemon.maxHp;
        }

        async Task ProcessEnemyKO()
        {
            var expGain = CalculateGainExp(currentEnemyPokemon);
            var koMesssage = $"{currentEnemyPokemon.Name} est KO!";
            var expMesssage = $"{currentAllyPokemon.Name} a gagné {expGain} points EXP.!";
            
            await ui.LogMessage(koMesssage, true);
            await ui.LogMessage(expMesssage, true);
            currentAllyPokemon.currentExp += expGain;
            await ui.UpdateExperience(currentAllyPokemon.currentExp);
            if (currentAllyPokemon.currentExp >= currentAllyPokemon.totalExpNeed) currentAllyPokemon.LevelUp();
            var levelUpMessage = $"{currentAllyPokemon.Name} monte au N. {currentAllyPokemon.level} !";
            await ui.LogMessage(levelUpMessage, true);

            QuitFight();
        }

        private void QuitFight()
        {
            inFight = false;
            GameManager.Instance.player.IsInFight = false;
            GameManager.Instance.GoToScene(enterFightEaseDuration, false);
        }

        int CalculateDamage(CapacitySO capacity, PokemonInstance attacker, PokemonInstance defender)
        {
            //Merci https://www.pokepedia.fr/Calcul_des_dégâts
            float damage = (attacker.level * 0.4f +2);
            damage = damage *(capacity.specialCapacity ? attacker.attack : attacker.attackSpe) * capacity.power;
            damage = (damage / (capacity.specialCapacity ? defender.defense : defender.defenseSpe))/50;
            damage += 2;
            if (attacker.so.ContainType(capacity.attackType)) damage *= 1.5f;
            float typeFactor = 1;
            switch (typesRelations.GetRelation(capacity.attackType, defender.so.type))
            {
                case Enums.RelationType.Normal:
                    typeFactor *= 1f;
                    break;
                case Enums.RelationType.Strong:
                    typeFactor *= 2f;
                    break;
                case Enums.RelationType.Weak:
                    typeFactor *= 0.5f;
                    break;
                case Enums.RelationType.NoEffect:
                    typeFactor *= 0;
                    break;
            }
            switch (typesRelations.GetRelation(capacity.attackType, defender.so.secondType))
            {
                case Enums.RelationType.Normal:
                    typeFactor *= 1f;
                    break;
                case Enums.RelationType.Strong:
                    typeFactor *= 2f;
                    break;
                case Enums.RelationType.Weak:
                    typeFactor *= 0.5f;
                    break;
                case Enums.RelationType.NoEffect:
                    typeFactor *= 0;
                    break;
            }
            damage *= typeFactor;

            switch (typeFactor)
            {
                case 0.5f:
                    Debug.Log("Attack Not very effective");
                    break;
                
                case 1f:break;
                
                case 2:
                case 4:
                    Debug.Log("Attack very effective");
                    break;
                
                default: Debug.Log($"Type factor {typeFactor}");
                    break;
            }
            
            return (int)damage;
        }
        
        private int CalculateGainExp(PokemonInstance pokemon)
        {
            return 150 * pokemon.level;
        }
    }
}