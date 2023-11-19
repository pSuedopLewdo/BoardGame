using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boggle/CreateBoggleLetter", fileName = "newBoggleLetter")]
public class BoggleLetterBlueprint : ScriptableObject
{
    public List<char> DieFace = new List<char>(6); //Direction / COORDINATE
    public List<char> FaceLetter = new List<char>(6);

    public char faceUpLetter;

    public Dictionary<char, char> DieProperties = new Dictionary<char, char>(6);


    public void SetDieProperties()
    {
        for (int i = 0; i < DieFace.Count; i++)
        {
            DieProperties.Add(DieFace[i], FaceLetter[i]);
            Debug.Log(DieFace[i] + " " + FaceLetter[i]);
        }
    }

    public void SetRandomDieFace()
    {
        faceUpLetter = FaceLetter[Random.Range(0, DieFace.Count)];
    }
    
}