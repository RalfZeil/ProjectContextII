//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class CardStructure : Structure
//{
//    [SerializeField] private GameObject createdCardPrefab;
//    private Transform createdCard;
//    protected override bool CanActivate()
//    {
//        return (createdCard == null || createdCard.GetComponent<Card>().wasPlayed);
//    }

//    protected override void Activate()
//    {
//        createdCard = Instantiate(createdCardPrefab).transform;
//        createdCard.position = transform.position;
//    }
//}
