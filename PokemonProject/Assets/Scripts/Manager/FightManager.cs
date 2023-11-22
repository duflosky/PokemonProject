using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SO;
using UI;
using UI.Fight;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class FightManager : MonoBehaviour
    {
        public static FightManager Instance;

        private UIFight ui;

        [SerializeField] private SpriteRenderer enemySpriteRenderer;
        [SerializeField] private SpriteRenderer allySpriteRenderer;
        [Space]
        [SerializeField]private PokemonSO enemyDebug;
        [Space,SerializeField] private int enemyLevel = 1;

        [Header("Param")]
        [SerializeField] private TypesRelationSO typesRelations;
        [SerializeField] float enterFightEaseDuration = 0.2f;
        
        [HideInInspector]public List<PokemonInstance> enemyPokemons;

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

        public void LaunchFight(PokemonInstance enemyPokemon) => LaunchFight(new List<PokemonInstance>() { enemyPokemon });
        public async void LaunchFight(List<PokemonInstance> enemyPokemonTeam)
        {
            enemyPokemons = enemyPokemonTeam;
            await GameManager.Instance.GoToScene(enterFightEaseDuration);
            SetPokemon(GameManager.Instance.team[0], true);
            SetPokemon(enemyPokemons[0], false);
        }

        void SetPokemon(PokemonInstance pokemon, bool ally)
        {
            if (ally)
            {
                allySpriteRenderer.sprite = pokemon.so.backPokemonSprite;
                currentAllyPokemon = pokemon;
            }
            else
            {
                enemySpriteRenderer.sprite = pokemon.so.facePokemonSprite;
                currentEnemyPokemon = pokemon;
            }
            ui.DisplayPokemonInfo(pokemon,ally);
        }


        public async void ProcessTurn(int capacityUseIndex)
        {
            ui.StartTurn();
            
            var allyCapacityUse = currentAllyPokemon.capacities[capacityUseIndex];
            var enemyCapacityUse = currentEnemyPokemon.capacities[Random.Range(0, currentEnemyPokemon.GetCapacityAmount())];
            allyCapacityUse.currentPP--;
            enemyCapacityUse.currentPP--;

            var allyFirst = currentAllyPokemon.speed > currentEnemyPokemon.speed;
            var firstPokemon = allyFirst ? currentAllyPokemon : currentEnemyPokemon;
            var secondPokemon = !allyFirst ? currentAllyPokemon : currentEnemyPokemon;
            
            await ProcessAttack(allyCapacityUse.so, firstPokemon, secondPokemon, allyFirst);
            await ProcessAttack(allyCapacityUse.so, secondPokemon, firstPokemon, !allyFirst);
            
            /*if (currentAllyPokemon.speed > currentEnemyPokemon.speed)
            {
                await ProcessAttack(allyCapacityUse.so, currentAllyPokemon, currentEnemyPokemon, true);
                await ProcessAttack(allyCapacityUse.so, currentEnemyPokemon, currentAllyPokemon, false);
            }
            else
            {
                await ProcessAttack(allyCapacityUse.so, currentAllyPokemon, currentEnemyPokemon, true);
                await ProcessAttack(allyCapacityUse.so, currentEnemyPokemon, currentAllyPokemon, false);
            }*/

            ui.EndTurn();
        }

        async Task ProcessAttack(CapacitySO capacity, PokemonInstance attacker, PokemonInstance defender, bool allyAttack)
        {
            var attackMessage = $"{attacker.so.name} utilise {capacity.name} !";
            await ui.LogMessage(attackMessage);
            
            //TODO animation d'attaque

            var damageDeal = CalculateDamage(capacity, attacker, defender);
            Debug.Log($"Deal {damageDeal} damage");
            defender.currentHp -= damageDeal;
            if (defender.currentHp < 0) defender.currentHp = 0;

            await ui.UpdateLife(allyAttack);

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
    }
}