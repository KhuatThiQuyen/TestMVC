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
    public class ScoreUpdateService
    {
        managerSchoolEntities _db;
        public ScoreUpdateService( managerSchoolEntities db)
        {
            _db = db;
        }
        public void UpdateScore(int ID, int ScoreNumber)
        {
            //var ScoreToUpdate = new ScoreToUpdate();
            //ScoreToUpdate.id = _scoreID;
            var editScore = _db.Scores.SingleOrDefault(sc => sc.ID == ID);
            if(editScore != null)
            {
                editScore.ScoreNumber = ScoreNumber;
                _db.SaveChanges();
            }
        }

    }
}