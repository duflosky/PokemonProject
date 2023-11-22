using System;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "New TypesRelations", menuName = "TypesRelations", order = 0)]
    public class TypesRelationSO : ScriptableObject
    {
        public RelationType[] relations;

        public Enums.RelationType GetRelation(Enums.Types attackType,Enums.Types defenseType)
        {
            foreach (var relation in relations)
            {
                if (relation.attackType == attackType && relation.defenseType == defenseType) return relation.relation;
            }
            return Enums.RelationType.Normal;
        }
    }

    [Serializable]
    public struct RelationType
    {
    public Enums.Types attackType;
    public Enums.RelationType relation;
    public Enums.Types defenseType;
    }
}