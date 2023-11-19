using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;


public class BoggleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] p_lettersGO;

    //[SerializeField] private List<string> p_alphabet; //Test alphabet
    public List<string> gameLetters;
    
    private GameObject[,]  p_boggleLetterGameObjects = new GameObject[4,4];
    private BoggleLetterBlueprint[,] p_allGameLetterSO = new BoggleLetterBlueprint[4,4];
    public List<BoggleLetterBlueprint> m_listGameLetterSO;
    public string[,]  m_boggleLetterStrings = new string[4,4];
    
    public void ShuffleScriptableObjectsList()
    {
        int n = m_listGameLetterSO.Count;

        // Iterate through the list from the end to the beginning
        for (int i = n - 1; i > 0; i--)
        {
            // Generate a random index between 0 and i (inclusive)
            int randomIndex = Random.Range(0, i + 1);

            // Swap the elements at randomIndex and i
            (m_listGameLetterSO[i], m_listGameLetterSO[randomIndex]) = (m_listGameLetterSO[randomIndex], m_listGameLetterSO[i]);
        }
    }
    public void GameLettersReset()
    {
        ShuffleScriptableObjectsList();
        
        foreach (var obj in m_listGameLetterSO)
        {
            obj.SetRandomDieFace();
        }
        LoopObjects();
    }
    
    // Start is called before the first frame update
    private void Awake()
    {
        GetAllLetters();
        //GenerateGameLetters();
    }

    private void GenerateGameLetters()
    {
        #region TestAlphabet
        /*
        
        p_alphabet = new List<string> {"a", "b", "c", "d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        var tempAlphabet = p_alphabet;
        
        var n = 26;
        for (int i = 0; i < 16; i++)
        {
            var rn = Random.Range(0, n);
            gameLetters.Add(tempAlphabet[rn]);

            tempAlphabet.RemoveAt(rn);
            n--;
        }
        */
        #endregion
        //TODO make the game letters the faces of all the SO's

        for (int x = 0; x < p_allGameLetterSO.GetLength(0); x++)
        {
            for (int y = 0; y < p_allGameLetterSO.GetLength(1); y++)
            {
                gameLetters.Add(p_allGameLetterSO[x, y].faceUpLetter.ToString());
            }
        }
        
        
        //TODO Sets the string[,]
        for (int x = 0; x < m_boggleLetterStrings.GetLength(0); x++)
        {
            for (int y = 0; y < m_boggleLetterStrings.GetLength(1); y++)
            {
                //TODO add the letters in the list to the 2D array
                m_boggleLetterStrings[x, y] = p_allGameLetterSO[x, y].faceUpLetter.ToString();
                //TODO a propper boggle system with 16* 6 numbers, more playable with dupes and more vowels
            }
        }
        
        Debug.Log(gameLetters);
    }

    void Start()
    {
        //DebugMatrix();
        //DebugLetterObjects();
        //TODO Loops through the letters and will perform the EachLetter Function
        GameLettersReset();
        LoopObjects();
    }

    private void GetAllLetters()
    {
        p_lettersGO = GameObject.FindGameObjectsWithTag("BoggleLetter");
    }

    private void LoopObjects()
    {
        for (int x = 0; x < m_boggleLetterStrings.GetLength(0); x++)
        {
            for (int y = 0; y < m_boggleLetterStrings.GetLength(1); y++)
            {
                //The action performed onto each of the letters
                EachLetter(x, y);
            }
        }
    }

    private void EachLetter(int x, int y)
    {
        p_allGameLetterSO[x, y] = m_listGameLetterSO[x * 4 + y];
        
        m_boggleLetterStrings[x, y] = p_allGameLetterSO[x, y].faceUpLetter.ToString();
        
        gameLetters.Add(p_allGameLetterSO[x, y].faceUpLetter.ToString());
        
        #region Naming the GameObjects
        foreach (var l in p_lettersGO)
        {
            var n = x + ", " + y;
            if (l.name == n)
            {
                p_boggleLetterGameObjects[x, y] = l;
            }
        }
        #endregion

        #region Filling the Text
        p_boggleLetterGameObjects[x, y].GetComponent<Text>().text = m_boggleLetterStrings[x, y];
        #endregion
    }

    public void DebugLetterObjects()
    {
        StringBuilder sb = new StringBuilder();
        var counter = 0;
        
        for (int x = 0; x < m_boggleLetterStrings.GetLength(0); x++)
        {
            for (int y = 0; y < m_boggleLetterStrings.GetLength(1); y++)
            {
                foreach (var l in p_lettersGO)
                {
                    var n = x + ", " + y;
                    if (l.name == n)
                    {
                        p_boggleLetterGameObjects[x, y] = l;
                    }
                }
            }
        }
        for (int x = 0; x < p_boggleLetterGameObjects.GetLength(0); x++)
        {
            for (int y = 0; y < p_boggleLetterGameObjects.GetLength(1); y++)
            {
                if (counter % 4 == 0)
                {
                    sb.Append("\n");
                }
                counter++;
                sb.Append(p_boggleLetterGameObjects[x, y].name + ", ");
                p_boggleLetterGameObjects[x, y].GetComponent<Text>().text = "| " + p_boggleLetterGameObjects[x, y].name;
            }
        }
        sb.AppendLine();
        Debug.Log(sb.ToString());
    }

    public void DebugMatrix()
    {
        StringBuilder sb = new StringBuilder();
        var counter = 0;

        for (int x = 0; x < m_boggleLetterStrings.GetLength(0); x++)
        {
            for (int y = 0; y < m_boggleLetterStrings.GetLength(1); y++)
            {
                if (counter % 4 == 0)
                {
                    sb.Append("\n");
                }
                counter++;
                sb.Append(x + ", " + y + " | ");
            }
        }
        sb.AppendLine();
        Debug.Log(sb.ToString());
    }
}
