using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Manager.Models
{
    public class ScoreCalculator
    {
        public Score Score { get; set; }
        public Subject subject { get; set; }
        public List<Score> firstScore { get; set; }
        public List<Score> secondScore { get; set; }
        public List<Score> thirdScore { get; set; }
        public double finalScore { get; set; } // custom get method to return finalscore = 1 x fiS + 2 x seS + 3 x thS
    }
}