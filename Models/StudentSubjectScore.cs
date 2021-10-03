using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Manager.Models
{
    public class StudentSubjectScore
    {
        public Student student { get; set; }
       
        //public virtual Student ID { get; set; }
        public List<ScoreCalculator> scoreCalculators { get; set; }
    }
    
}