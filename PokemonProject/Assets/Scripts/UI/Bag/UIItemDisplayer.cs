using SO;
using TMPro;
using UnityEngine;

namespace UI.Bag
{
    public class UIItemDisplayer : MonoBehaviour
    {
        [SerializeField] private UIBag parent;
        [Space]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI amountText;

        private bool isEnd;

        public void SetItem(ItemStack item)
        {
            isEnd = false;
            nameText.text = item.so.name.ToUpper();
            amountText.text = item.so.type == Enums.ObjectType.KeyItems? "" : $"x {item.amount}";
        }

        public void SetEnd()
        {
            nameText.text = "SORTIR";
            amountText.text = "";
            isEnd = true;
        }

        public void ButtonAction()
        {
            if(isEnd) parent.GoBackMenu();
        }
    }
}