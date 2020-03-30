using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data object that can generate a number of dancer names
/// 
/// TODO:
///     Needs to generate the number of requested <b>unique</b> character names for the dancers in our battle
/// </summary>
//[CreateAssetMenu(menuName = "Battle Objects/Character Name Generator")]
//[System.Serializable]
public class CharacterNameGenerator : ScriptableObject
{
    [Header("Possible first names")]
    [SerializeField]
    public List<string> firstNames;
    [Header("Possible last names")]
    [SerializeField]
    public List<string> lastNames;
    [Header("Possible nicknames")]
    [SerializeField]
    public List<string> nicknames;
    [Header("Possible adjectives to describe the character")]
    [SerializeField]
    public List<string> descriptors;

    public CharacterName[] GenerateNames(int namesNeeded)
    {
        CharacterName[] names = new CharacterName[namesNeeded];

        for (int i = 0; i < namesNeeded; i++)
        {
            string first = firstNames[Random.Range(0, firstNames.Count)];
            string last = lastNames[Random.Range(0, lastNames.Count)];
            last.Remove;
            object p = Debug.Log(last);
            string nick = nicknames[Random.Range(0, nicknames.Count)];
            string des = descriptors[Random.Range(0, descriptors.Count)];

            names[i] = new CharacterName(first, last, nick, des);
        }

        return names;
    }
}