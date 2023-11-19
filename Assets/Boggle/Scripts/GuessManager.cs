using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GuessManager : MonoBehaviour
{
    public Text guessTextBox;
    public Text correctTextBox, wrongTextBox;
    public InputField guessInputField;
    public BoggleManager bm;
    public ChatBotGuessCheck boxBotGuessCheck;
    public bool isEnded;

    private int _wordLimit = 999;

    public string[,] m_boardLetters;
    
    public List<string> guesses = new List<string>();

    private void Start()
    {
        guessInputField.ActivateInputField();
        isEnded = false;
    }

    private void SetBoardLetters()
    {
        m_boardLetters = bm.m_boggleLetterStrings;
    }

    private void Update()
    {
        var upper = guessInputField.text.ToUpper();
        guessInputField.text = upper;
    }

    private void FixedUpdate()
    {
        SetBoardLetters();
    }
    
    public void SubmitGuess()
    {
        if (isEnded)
        {
            guessInputField.readOnly = true;
            return;
        }
        var w = guessInputField.text;
        var b = "";
        guesses.Add(w.ToUpper());
        Debug.Log(b + " is submitted");
        b += guesses.Aggregate(b, (current, t) => current + ("\n" + t.ToUpper()));

        guessTextBox.text = b;
        guessInputField.text = "";
        guessInputField.ActivateInputField();
        
        if (guesses.Count >= _wordLimit)
        {
            EndOfGame();
            Debug.LogWarning("WordLimit Reached \n" + _wordLimit);
        }
    }

    public void ChangeTextAfterGuess()
    {
        var i = boxBotGuessCheck.correctGuesses;
        var j = "";

        for (int k = 0; k < i.Count; k++)
        {
            j += i[k] + " \n ";
        }
        
        correctTextBox.text = j;
        
        var w = boxBotGuessCheck.wrongGuesses;
        var a = "";

        for (int k = 0; k < w.Count; k++)
        {
            a += w[k] + " \n ";
        }
        
        wrongTextBox.text = a;
        
    }

    public void EndOfGame()
    {
        isEnded = true;
        boxBotGuessCheck.wordsToFind = guesses;
        boxBotGuessCheck.BeginSearch();
        
        ChangeTextAfterGuess();
    }

    public void Reset()
    {
        correctTextBox.text = "";
        wrongTextBox.text = "";
        guessTextBox.text = "";
        guesses.Clear();
        bm.gameLetters.Clear();
        isEnded = false;
        bm.GameLettersReset();
        
    }


    /*
    
    public void SetWrongAnswer()
    {
        Debug.Log(guess + " is the wrong answer");
        guessInputField.text = "";
        guess = "";
    }

    public void SetCorrectGuess()
    {
        var i = guess;
        var t = "";
        guesses.Add(i);
        Debug.Log(i + " is the correct answer");
        guesses.Add(i);
        for (int j = 0; j < guesses.Count; j++)
        {
            t += "\n" + guesses[j];
        }

        guessTextBox.text = t;
        guessInputField.text = "";
    }

    
     
     
    void InitializeGrid(string s)
    {
        grid = new int[rows, columns];

        // Initialize the grid with some values (for demonstration purposes)
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                grid[i, j] = i * columns + j;
                if (p_boardLetters[i, j] == s[0].ToString())
                {
                    FindAdjacentPositions(i, j);
                }
            }
        }
    }

    void FindAdjacentPositions(int row, int column)
    {
        Debug.Log($"Adjacent positions to ({row}, {column}):");

        if (p_boardLetters[row - 1, column] != null)
        {
            // Check top
            CheckPosition(row - 1, column);
        }
        
        if (p_boardLetters[row + 1, column] != null)
        // Check bottom
        CheckPosition(row + 1, column);

        if (p_boardLetters[row - 1, column] != null)
        // Check left
        CheckPosition(row - 1, column);

        if (p_boardLetters[row, column + 1] != null)
        // Check right
        CheckPosition(row, column + 1);

        if (p_boardLetters[row - 1, column - 1] != null)
        // Check top-left
        CheckPosition(row - 1, column - 1);

        if (p_boardLetters[row - 1, column + 1] != null)
        // Check top-right
        CheckPosition(row - 1, column + 1);

        if (p_boardLetters[row + 1, column - 1] != null)
        // Check bottom-left
        CheckPosition(row + 1, column - 1);

        if (p_boardLetters[row + 1, column + 1] != null)
        // Check bottom-right
        CheckPosition(row + 1, column + 1);
    }

    void CheckPosition(int row, int column)
    {
        if (row >= 0 && row < rows && column >= 0 && column < columns)
        {
            Debug.Log($"({row}, {column}): {grid[row, column]}");
        }
    }
    
    
    //row One of the Top Touching Letters
    

    
    private bool CheckGuess(string s)
    {
        var temp2D = p_boardLetters;

        for (int x = 0; x < temp2D.GetLength(0); x++)
        {
            for (int y = 0; y < temp2D.GetLength(1); y++)
            {
                //check if the first letter exists on the board
                //check the | -x, +y | x, +y | +x, +y | 
                //          | -x,  y | x,  y | +x,  y |
                //          | -x, -y | x, -y | +x, -y |

                if (p_boardLetters[x, y] == s[0].ToString())
                {
                    //the first letter does exist
                    //checking for all the letters
                    for (int j = 0; j < s.Length; j++)
                    {
                        //0 row
                        for (int rZ = 0; rZ < 3; rZ++)
                        {
                            var newX = x - 1;
                            var newY = y + 1;
                            if (p_boardLetters[newX, newY] == s[j].ToString())
                            {
                                Debug.Log("found letter: " + s[j] + "at: " + newX + ", " + newY);
                            }
                            else
                            {
                                return false;
                            }
                            Debug.Log("didnt find: " + s[j] + "at: " + newX + ", " + newY);
                            newX++;
                            if (rZ == 2)
                            {
                                //1 row
                                for (int rO = 0; rO < 3; rO++)
                                {
                            
                                    newX = x - 2;
                                    newY = y - 1;
                                    
                                    if (p_boardLetters[newX, newY] == s[j].ToString())
                                    {
                                        Debug.Log("found letter: " + s[j] + "at: " + newX + ", " + newY);
                                    }else
                                    {
                                        return false;
                                    }

                                    if (rO == 2)
                                    {
                                        //2 row
                                        for (int rT = 0; rT < 3; rT++)
                                        {
                                            newX = x - 2;
                                            newY = y - 1;
                                    
                                            if (p_boardLetters[newX, newY] == s[j].ToString())
                                            {
                                                Debug.Log("found letter: " + s[j] + "at: " + newX + ", " + newY);
                                            }else
                                            {
                                                return false;
                                            }
                                            //what to do if word is right'
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
        }

        return false;
    }
    */
}
