using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Test.Model
{
   public class User2
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }             //// RIMARY KEY  必须优先 AUTOINCREMENT

        public string Name { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDay { get; set; }

        public DateTime CreateTime { get; set; }


        public DateTime UpdateTime { get; set; }

        [NonWrite]
        public string Status { get; set; }

        public bool Check()
        {
            return true;
        }
    }
}
