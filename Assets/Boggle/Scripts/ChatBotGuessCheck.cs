using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChatBotGuessCheck : MonoBehaviour
{
    public int gridSize = 4; // Adjust the size of the grid as needed
    private char[,] grid;
    public GuessManager gm;
    
    // Example list of words to search for
    [HideInInspector]
    public List<string> wordsToFind;

    public List<string> correctGuesses = new List<string>();
    public List<string> wrongGuesses = new List<string>();
    
    [SerializeField]
    private TextAsset jsonFile;

    
    [HideInInspector]
    public List<string> englishWords;
    [HideInInspector]
    public List<string> englishWordsLessThanTwoLetters;
    void Awake()
    {
        if (jsonFile == null)
        {
            Debug.LogError("Please assign a JSON file in the inspector.");
            return;
        }

        LoadWordList();
    }

    void LoadWordList()
    {
        try
        {
            englishWords = new List<string>(jsonFile.text.ToUpper().Split('\n'));

            for (int i = englishWords.Count - 1; i >= 0; i--)
            {
                string cleanedWord = RemoveNonLetters(englishWords[i]);

                if (cleanedWord.Length < 3 || cleanedWord.Length > 12)
                {
                    englishWords.RemoveAt(i);
                }
                else
                {
                    englishWords[i] = cleanedWord;
                }
            }

            Debug.Log($"Loaded {englishWords.Count} words from the JSON file.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading the JSON file: {e.Message}");
        }
    }

    string RemoveNonLetters(string input)
    {
        char[] charArray = input.ToCharArray();
        for (int i = 0; i < charArray.Length; i++)
        {
            if (!char.IsLetter(charArray[i]))
            {
                charArray[i] = ' ';
            }
        }
        return new string(charArray).Replace(" ", "");
    }

    public void BeginSearch()
    {
        GenerateGrid();
        PrintGrid();

        foreach (string word in wordsToFind)
        {
            if (SearchWord(word))
            {
                Debug.Log($"Found word: {word}");
                if (englishWords.Contains(word))
                {
                    correctGuesses.Add(word);
                }
                else
                {
                    Debug.Log($"Word not real: {word}");
                    wrongGuesses.Add(word);
                }
                
            }
            else
            {
                Debug.Log($"Word not found: {word}");
                wrongGuesses.Add(word);
            }
        }
    }

    void GenerateGrid()
    {
        grid = new char[gridSize, gridSize];

        // Populate the grid with random letters (for demonstration purposes)
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                //set local grid to main grid
                grid[i, j] = Convert.ToChar(gm.m_boardLetters[i, j][0].ToString().ToUpper());
            }
        }
    }

    void PrintGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            string row = "";
            for (int j = 0; j < gridSize; j++)
            {
                row += grid[i, j] + " ";
            }
            Debug.Log(row);
        }
    }

    bool SearchWord(string word)
    {
        // Iterate through each cell in the grid
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // If the current letter matches the first letter of the word, start searching
                if (grid[i, j] == word[0])
                {
                    // Call the recursive search function
                    if (SearchWordRecursive(word, i, j, 0))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    bool SearchWordRecursive(string word, int row, int col, int index)
    {
        // Base case: If the entire word has been matched
        if (index == word.Length)
        {
            return true;
        }

        // Check if the current position is within bounds and the letter matches
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize && grid[row, col] == word[index])
        {
            // Mark the current cell as visited to avoid revisiting it
            char original = grid[row, col];
            grid[row, col] = '\0'; // Using a placeholder character

            // Recursively search in all 8 adjacent cells
            bool found = SearchWordRecursive(word, row - 1, col, index + 1) ||
                         SearchWordRecursive(word, row + 1, col, index + 1) ||
                         SearchWordRecursive(word, row, col - 1, index + 1) ||
                         SearchWordRecursive(word, row, col + 1, index + 1) ||
                         SearchWordRecursive(word, row - 1, col - 1, index + 1) ||
                         SearchWordRecursive(word, row - 1, col + 1, index + 1) ||
                         SearchWordRecursive(word, row + 1, col - 1, index + 1) ||
                         SearchWordRecursive(word, row + 1, col + 1, index + 1);

            // Restore the original character for backtracking
            grid[row, col] = original;

            return found;
        }

        return false;
    }
}