using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Guest
{
    [SerializeField] public Sprite guestImage;
    [SerializeField] public Sprite talkBubbleImage;
    [SerializeField] public TextGroup textGroup;
}

[CreateAssetMenu(menuName = "ScriptableObj/GuestGroup", fileName = "GuestGroup")]
public class GuestGroup : ScriptableObject
{
    [SerializeField] public List<Guest> Guests;
}
