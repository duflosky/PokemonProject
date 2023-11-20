using SO;
using TMPro;
using UnityEngine;

namespace UI.Fight
{
    public class PokemonInfoBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Lifebar lifebar;
        [SerializeField] private ExperienceBar expBar;

        public void InitBox(PokemonInstance pokemon)
        {
            nameText.text = pokemon.name;
            levelText.text = $"{pokemon.level}";
            if (hpText) hpText.text = $"{pokemon.currentHp} / {pokemon.maxHp}";
            lifebar.InitBar(pokemon.currentHp, pokemon.maxHp);
            if(expBar)expBar.InitBar(pokemon.currentExp, pokemon.totalExpNeed);
        }
    }
}