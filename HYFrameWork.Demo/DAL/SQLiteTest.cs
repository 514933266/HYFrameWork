using HYFrameWork.Core;
using HYFrameWork.DAL.SQLite;
using HYFrameWork.Test.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Test
{
   public class SQLiteTest: IDALTest
    {
        string _dbSource = @"C:\Users\xuhaopeng\Desktop\User.sqlite";
        string _dbSource2 = @"C:\Users\xuhaopeng\Desktop\User2.sqlite";
        IDbConnection _conn { get; set; }
        IDbConnection _conn2 { get; set; }

        /// <summary>
        /// SQLite仓储对象
        /// </summary>
        public IRepository<User> Repository { get; set; }
        /// <summary>
        /// SQLite仓储对象 2
        /// </summary>
        public IRepository<User> Repository2 { get; set; }

        public SQLiteTest()
        {
            SQLiteCommon.CreateDb(_dbSource);
            //SQLiteCommon.CreateDb(_dbSource2);
            Repository= SQLiteCommon.CreateSQLiteRepository<User>(_dbSource);
            //Repository2 = SQLiteCommon.CreateSQLiteRepository<User>(_dbSource2);
            //CreateTableTest();
        }

        #region 表创建

        //约束特性使用：PRIMARY KEY  优先 AUTOINCREMENT 两者不可分开使用
        private void CreateTableTest()
        {
            //SQLiteCommon.CreateTable(Repository);
            //SQLiteCommon.CreateTable(Repository2);
            //CREATE  TABLE IF NOT EXISTS [User] (
            //"Id" INTEGER  PRIMARY KEY AUTOINCREMENT,
            //"Name" TEXT  ,
            //"UserName" TEXT  ,
            //"PassWord" TEXT  ,
            //"Sex" BOOLEAN  ,
            //"BirthDay" DATETIME  ,
            //"CreateTime" DATETIME  ,
            //"UpdateTime" DATETIME  ) 
        }
        #endregion

        #region 增加
        public void AddTest()
        {
            var obj = Repository.Add(new User()
            {
                Name = "测试",
                UserName = "Add",
                PassWord = "Add",
                Sex = true,
                BirthDay = DateTime.Now,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });
            //INSERT INTO [User] (Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime) 
            //SELECT @Name,@UserName,@PassWord,@Sex,@BirthDay,@CreateTime,@UpdateTime 
        }

        public void AddIfNotExistsTest()
        {
            var obj = Repository.AddIfNotExists(new User() {
                Name = "测试",
                UserName = "AddIfNotExists",
                PassWord = "AddIfNotExists",
                Sex = true,
                BirthDay = DateTime.Now,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            }, u => u.UserName == "AddIfNotExists");
            //INSERT INTO [User] (Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime) 
            //SELECT @Name,@UserName,@PassWord,@Sex,@BirthDay,@CreateTime,@UpdateTime 
            //WHERE NOT EXISTS( 
            //SELECT Id, Name, UserName, PassWord, Sex, BirthDay, CreateTime, UpdateTime FROM [User]  WHERE (UserName = @P0)
            //LIMIT 0,1)
        }
        public void AddListTest()
        {
            List<User> us = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                us.Add(new User()
                {
                    Name = "测试",
                    UserName = "AddList" + i,
                    PassWord = "AddList" + i,
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            Repository.AddList(us);                                 //10000条数据 SQLite 5秒 仅适用与100条数据以下
            using (var tran = new UnitOfWork().BeginTransaction())  //10000条数据 SQLite1秒（建议使用该方法
            {
                Repository.AddList(us, tran);
                tran.Commit();
            }
        }

        #endregion

        #region 查询

        //查询字段
        public void SelectTest()
        {
            var obj = Repository.Get(where => where.Name == "测试");
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Name = @P0)

            var obj2 = Repository.Get(where => where.Name == "测试", order => new DbSort<User>().Desc(o => o.Id));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User] WHERE (Name == @P0) ORDER BY ID DESC 

            var obj3 = Repository.GetEx(where => where.UserName == "AddList0", u => new User());
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime,Status FROM [User]  WHERE (UserName = @P0) 

            var obj4 = Repository.GetEx(where => where.UserName == "AddList0", order => new DbSort<User>().Desc(o => o.Id), u => new User() { Id = 0, Name = "" });
            //SELECT Id,Name FROM [User]  WHERE (UserName = @P0)  ORDER BY Id DESC

            var obj5 = Repository.GetList(where => where.UserName.StartsWith("AddList"));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0)

            var obj6 = Repository.GetList(where => where.UserName.StartsWith("AddList"), 10);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0) LIMIT 0,10 

            var obj7 = Repository.GetList(where => where.UserName.StartsWith("AddList"), order => new DbSort<User>().Desc(o => o.Id), 10);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0)  ORDER BY Id DESC LIMIT 0,10 

            var obj8 = Repository.GetListEx(where => where.UserName.StartsWith("AddList"), u => new User());
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime,Status FROM [User]  WHERE (UserName LIKE @P0) 

            var obj9 = Repository.GetListEx<User>(where => where.UserName.StartsWith("AddList"), order => new DbSort<User>().Desc(o => o.Id), null, 10);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0)  ORDER BY Id DESC LIMIT 0,10

            var obj10 = Repository.Count(where => where.UserName.StartsWith("AddList"));
            //SELECT COUNT(1) FROM [User]   WHERE (UserName LIKE @P0) 

            var obj11 = Repository.PageList(where => where.UserName.StartsWith("AddList"), 2, 10);
            //SELECT COUNT(1) FROM [User]  WHERE (UserName LIKE @P0) ;SELECT Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0) LIMIT 10 OFFSET 10*1 

            var obj12 = Repository.PageList(where => where.UserName.StartsWith("AddList"), order => new DbSort<User>().Desc(o => o.Id), 10, 2);
            //SELECT COUNT(1) FROM [User]   WHERE (UserName LIKE @P0) ;SELECT * FROM [User]   WHERE (UserName LIKE @P0)  ORDER BY Id DESC LIMIT 10 OFFSET 10*1 

            var obj13 = Repository.PageListEx(where => where.UserName.StartsWith("AddList"), u => new User(), 10, 3);
            //SELECT COUNT(1) FROM [User]   WHERE (UserName LIKE @P0) ;SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime,Status FROM [User]   WHERE (UserName LIKE @P0) LIMIT 10 OFFSET 10*2 

            var obj14 = Repository.PageListEx<User>(where => where.UserName.StartsWith("AddList"), null, 10, 3);
            //SELECT COUNT(1) FROM [User]  WHERE (UserName LIKE @P0) ;SELECT Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0) LIMIT 3 OFFSET 3*9 
        }

        //Where条件
        public void PredicateTest()
        {
            User user = new User();
            var whereAnd = PredicateBuilder.True<User>().And(o => o.Sex);
            var obj0 = Repository.Get(whereAnd);

            var obj = Repository.Get(where => where.Id > user.Id);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Id > @P0)

            var obj2 = Repository.Get(where => where.Name != "");
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Name != '')

            var obj3 = Repository.Get(where => where.Name == " ");
            //SSELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Name = @P0)

            var obj3_2 = Repository.Get(where => where.Name == "");
            //SELECT  Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE(Name = '')

            var obj4 = Repository.Get(where => where.Sex || !where.Sex);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime  FROM [User]   WHERE ((Sex  =  @P0) OR (Sex  =  @P1))

            var obj4_2 = Repository.Get(where => where.Sex == true || where.Sex == false);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE ((Sex = @P0) OR (Sex = @P1))

            var obj5 = Repository.Get(where => !where.Name.IsNullOrEmpty() || where.Name.IsNullOrEmpty());
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE ((Name IS NOT NULL) OR (Name IS NULL)) 

            var obj5_2 = Repository.Get(where => where.Name == null || where.Name != null);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE ((Name IS  NULL ) OR (Name IS NOT NULL )) 

            var obj6 = Repository.Get(where => where.Name.Contains("2"));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime  FROM [User]   WHERE (Name LIKE @P0 )

            var obj7 = Repository.Get(where => where.Name.EndsWith("2"));
            //SLECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Name LIKE @P0) 

            var obj8 = Repository.Get(where => where.UserName.StartsWith("2"));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (UserName LIKE @P0) 

            var obj9 = Repository.Get(where => where.UpdateTime > DateTime.Now.AddDays(-3));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime  FROM [User]   WHERE (UpdateTime > @P0)

            var obj10 = Repository.Get(where => new DbMethod().DateDiff(DateEnum.Day, where.UpdateTime, DateTime.Now) >= 1);
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE ((julianday(datetime('2017-09-14 11:24:35')) - julianday(datetime(UpdateTime))) >= @P0)

            //var obj10_2 = Repository.Get(where => new DbMethod().CharIndex("测试",where.UpdateTime) > 0);

            List<int> ids = new List<int>() { 11, 22, 33 };
            List<string> names = new List<string>() { "AddList", "22", "33" };
            var arr = new int[] { 11, 22, 33 };
            var strArr = new string[] { "AddList", "22", "33" };

            var obj11 = Repository.Get(where => where.Id.In(ids));
            var obj11_2 = Repository.Get(where => ids.Contains(where.Id));
            var obj11_3 = Repository.Get(where => where.Id.In(arr));
            var obj11_4 = Repository.Get(where => where.Id.In(new[] { 11, 22, 33 }));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime  FROM [User]   WHERE (Id IN (11,22,33))

            var obj11_5 = Repository.Get(where => where.UserName.In(names));
            var obj11_6 = Repository.Get(where => names.Contains(where.UserName));
            var obj11_7 = Repository.Get(where => where.UserName.In(strArr));
            var obj11_8 = Repository.Get(where => where.UserName.In(new[] { "AddList", "22", "33" }));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime  FROM [User]   WHERE (Name IN ('测试','22','33'))


            var obj12 = Repository.GetList(where => where.Id.NotIn(ids));
        }
        #endregion

        #region 修改

        public void UpdateTest()
        {
            var obj = Repository.Update(new User()
            {
                Id = 2,
                Name = "测试",
                UserName = "Update",
                PassWord = "Update",
                Sex = true,
                BirthDay = DateTime.Now,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });
            User user = new User() { Id = 1 };
            var obj2 = Repository.Update(c => c.Id > user.Id, o => new User() { Sex = true, Name = "测试",CreateTime=DateTime.Now });
            //var obj = Repository.Update(new User() { UserName = "测试", Name = "测试姓名" });
            ////UPDATE [User] SET Name=@Name,UserName=@UserName,PassWord=@PassWord,Sex=@Sex,BirthDay=@BirthDay,CreateTime=@CreateTime,UpdateTime=@UpdateTime WHERE Id=@Id

            //var obj2 = Repository.Update(w=>w.Id>=0&&w.UserName=="测试", u => new User() { UserName = "测试2", Name = "测试姓名2" });
            ////UPDATE User SET UserName = @UserName, Name = @Name WHERE ((Id >= @P0) AND (UserName = @P1))
            using (var tran = new UnitOfWork().BeginTransaction())
            {
                Repository.Update(new User() { UserName = "UpdateTran", Name = "测试" }, tran);

                Repository.Update(w => w.Id >= 0 && w.UserName == "UpdateTran", u => new User() { UserName = "测试4", Name = "测试", Sex = false }, tran);
                tran.Commit();
            }
        }
        #endregion

        #region 删除
        public void DeleteTest()
        {
            Repository.Delete(new User() { Id = 1 });          //DELETE FROM [User] WHERE Id=@Id
            Repository.Delete(w => w.Id > 0);                  //DELETE FROM [User]  WHERE (Name = @P0)

        }
        #endregion

        #region 事务

        public void LoacalTransactionTest()
        {
            List<User> us = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                us.Add(new User()
                {
                    Name = "测试",
                    UserName = "AddList_LoacalTransaction"+i,
                    PassWord = "AddList_LoacalTransaction"+i,
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            using (var tran = new UnitOfWork().BeginTransaction())
            {
                Repository.Add(new User()
                {
                    Name = "测试",
                    UserName = "Add_LoacalTransaction",
                    PassWord = "Add_LoacalTransaction",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, tran);

                Repository.AddIfNotExists(new User()
                {
                    Name = "测试",
                    UserName = "AddIfNotExists_LoacalTransaction",
                    PassWord = "AddIfNotExists_LoacalTransaction",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, u => u.UserName == "AddIfNotExists_LoacalTransaction", tran);

                Repository.AddList(us, tran);
                tran.Commit();
            }
        }
        //本地分布式事务
        public void LoacalDisTransactionTest()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                Repository.Add(new User()
                {
                    Name = "测试",
                    UserName = "Add_LoacalDisTransaction",
                    PassWord = "Add_LoacalDisTransaction",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, unitOfWork.BeginTransaction());

                Repository2.AddIfNotExists(new User()
                {
                    Name = "测试",
                    UserName = "AddIfNotExists_LoacalTransaction",
                    PassWord = "AddIfNotExists_LoacalTransaction",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, u => u.UserName == "AddIfNotExists_LoacalTransaction", unitOfWork.BeginTransaction());

             /*   unitOfWork.Commit();*/                               //此种方式，仅回滚出错的操作
                unitOfWork.Commit();//此种方式，回滚所有操作
            }
        }
        //分布式事务
        public void DistributedTransactionTest()
        {
            //预留
        }

        #endregion


        #region 存储过程
        public void StoreProcedureTest()
        {

        }
        #endregion
    }
}
