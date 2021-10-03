using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using School_Manager.Models;

namespace School_Manager.Service
{
    public class ScoreService
    {
        Student _std;
        //1 scoreType have score      
        managerSchoolEntities _db;
        public ScoreService(Student std, managerSchoolEntities db)
        {
            _std = std;

            _db = db;
        }

        public StudentSubjectScore CalculatorScore()
        {
            var StudentSubjectScore = new StudentSubjectScore();
            StudentSubjectScore.student = _std;

            var scoreBySubjs = _db.Students.Find(_std.ID).Scores.GroupBy(sb => sb.SubjectID);
            List<ScoreCalculator> listScoreCalculator = new List<ScoreCalculator>();
            StudentSubjectScore.scoreCalculators = listScoreCalculator;//get Id Scores in this
            foreach (var scoreSubject in scoreBySubjs)
            {
                Console.WriteLine(scoreSubject);
                //int? scoreId = scoreSubject.Key;
                //Score Idscore = _db.Scores.Find(scoreId ?? 1);
                //get id subject
                int? subjectID = scoreSubject.Key;
                Subject subject = _db.Subjects.Find(subjectID ?? 1);//get id subject
                List<Score> firstScore = new List<Score>();
                List<Score> secondScore = new List<Score>();
                List<Score> thirdScore = new List<Score>();
                foreach (var sc in scoreSubject)
                    //scoreSubject get id of score
                {

                    if(sc.ScoreTypeID ==1 || sc.ScoreTypeID == 2)
                    {
                        firstScore.Add(sc);
                    } 
                    if(sc.ScoreTypeID == 3)
                    {
                        secondScore.Add(sc);
                    }
                    if(sc.ScoreTypeID == 4)
                    {
                        thirdScore.Add(sc);
                    }
                   
                }

                //tinh diem trung binh
                int count = firstScore.Count + secondScore.Count * 2 + thirdScore.Count * 3;

                double sumscore = (firstScore.Select(x => x.ScoreNumber).Sum() + secondScore.Select(x => x.ScoreNumber).Sum() * 2 + thirdScore.Select(x => x.ScoreNumber).Sum() * 3);
                double finalsc = Math.Round(sumscore / count, 2);
                ScoreCalculator subjectSc = new ScoreCalculator();
                //subjectSc.Score = scoreSubject
                subjectSc.finalScore = finalsc;
                subjectSc.subject = subject;
                subjectSc.firstScore = firstScore;
                subjectSc.secondScore = secondScore;
                subjectSc.thirdScore = thirdScore;
                listScoreCalculator.Add(subjectSc);

            }


            return StudentSubjectScore;
        }

    }
}