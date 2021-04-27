using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public static class QuizManager
{
    public static List<Question> questions;
    private static CultureInfo deCulture = new CultureInfo("de-DE");
    private static int currentIndex;
    private static System.Random rnd;

    static QuizManager()
    {
        //not working i dont know why - seems to be the same as in graphgenerator
        //calllig in gamemanager now
        //CreateQuestionList();
        currentIndex = 0;
        rnd = new System.Random();
        
    }

    public static void CreateQuestionList()
    {
        TextAsset questionsFile = ResourceLoader.QuizQuestionsText();
        if (questionsFile == null)
            throw new FileNotFoundException("Question resource do not exist");


        string input = questionsFile.text;
        string[] lines = input.Split('\n');
        questions = new List<Question>();

        for (int row = 1; row < lines.Length - 1; row++)
        {
            string[] columns = lines[row].Split(';');
            questions.Add(
                new Question(
                    row, //index
                    columns[1], //question
                    columns[2], //option a
                    columns[3], //option b
                    columns[4], //option c
                    columns[5], //option d
                    Convert.ToChar(columns[6]), //solution
                    columns[7] //description
                )
            ); 
        }

        RandomizeQuestions();
    }

    private static void RandomizeQuestions()
    {
        int pos = 0;
        int cnt = 0;
        List<Question> random = new List<Question>();
        while(cnt < questions.Count)
        {
            pos = rnd.Next(0, questions.Count);

            if(!random.Contains(questions[pos]))
            {
                random.Add(questions[pos]);
                cnt++;
            }
        }

        questions = random;

    }

    public static Question NextQuestion()
    {
        currentIndex++;
        if(currentIndex > questions.Count-1) {
            RandomizeQuestions();
            currentIndex=0;
        }
        return questions[currentIndex];
    }
}
