﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharSelectionUI : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void Start()
    {
        selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        characters[selectedCharacter].SetActive(true);
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Character Number", selectedCharacter);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    public void NextCharacters()
    //Takes an array filled with all our character so we can do more than just the planned four
    //Then uses mod so while the player is cycling through it doesn't create an oout of bounds issue
    {
        //Makes the character selected to not being visable in the scene
        characters[selectedCharacter].SetActive(false);

        //Moves to the next charcter in the array
        selectedCharacter = (selectedCharacter + 1) % characters.Length;

        //Makes the charcter selected visable in the scene
        characters[selectedCharacter].SetActive(true);

        //Sets the int and saves it so it can be used in other scenes. 
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);

        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Character Number", selectedCharacter);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }

    public void PreviousCharacter()
    //Same thing as the next char, but has the loop go the other way
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht.Add("Character Number", selectedCharacter);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }
}
