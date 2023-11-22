using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SO;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject gameScene;
        [SerializeField] private GameObject fightScene;
        public PokemonInstance[] team { get; set; }= new PokemonInstance[6];
        public Dictionary<Enums.ObjectType, List<ItemStack>> inventory = new();

        [SerializeField]private PokemonSO[] debugStartPokemon;
        [SerializeField] private ItemSO[] debugStartItem;
        private void Awake()
        {
            if (Instance != null) DestroyImmediate(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            gameScene.SetActive(true);
            fightScene.SetActive(false);
            
            foreach (var objectType in Enum.GetValues(typeof(Enums.ObjectType)))
            {
                inventory.Add((Enums.ObjectType)objectType, new());
            }

            foreach (var item in debugStartItem)
            {
                inventory[item.type].Add(new ItemStack(item, Random.Range(1,15)));
            }
            
            for (int i = 0; i < debugStartPokemon.Length; i++)
            {
                team[i] = new PokemonInstance(debugStartPokemon[i], 5);
            }
        }


        public async Task GoToScene(float easeDuration, bool fightScene = true)
        {
            await UIScene.Instance.EaseIn(easeDuration);
            UIManager.Instance.OpenMenu(Enums.UIMenus.FightMenu, true);
            gameScene.SetActive(!fightScene);
            this.fightScene.SetActive(fightScene);
            await UIScene.Instance.EaseOut(easeDuration);
        }
    }
}