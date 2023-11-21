using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "New Object", menuName = "Pokemon/Object", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public Enums.ObjectType type;

        public string name;
        [TextArea] public string description;
    }

    public class ItemStack
    {
        public ItemSO so;
        public int amount;
        
        public ItemStack(ItemSO _so, int _amount = 1)
        {
            so = _so;
            amount = _amount;
        }
    }
}