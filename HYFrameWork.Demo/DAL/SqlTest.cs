using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using HYFrameWork.Test.Model;
using System.Data;
using System.Data.SqlClient;
using HYFrameWork.DAL.SqlServer;
using System.Diagnostics;
using System.Data.Common;
using System.Linq;

namespace HYFrameWork.Test
{
    /// <summary>
    /// sqlite 查询单元测试
    /// </summary>
   public class SqlTest:IDALTest
    {
        /// <summary>
        /// 仓储对象
        /// </summary>
        public IRepository<User> Repository { get; set; }
        /// <summary>
        /// 仓储对象 2
        /// </summary>
        public IRepository<User2> Repository2 { get; set; }
        public SqlTest()
        {
            Repository = new SqlServerRepository<User>(new SqlConnection("SqlserverConStr".ValueOfAppSetting()));
            Repository2 = new SqlServerRepository<User2>(new SqlConnection("SqlserverConStr".ValueOfAppSetting()));
        }

        #region 增加
        public void AddTest()
        {
            //var obj = Repository.Add(new User()
            //{
            //    Name = "测试",
            //    UserName = "Add",
            //    PassWord = "Add",
            //    Sex = true,
            //    BirthDay = DateTime.Now,
            //    CreateTime = DateTime.Now,
            //    UpdateTime = DateTime.Now
            //});
            //INSERT INTO [User] (Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime) 
            //SELECT @Name,@UserName,@PassWord,@Sex,@BirthDay,@CreateTime,@UpdateTime 
            var u = new User()
            {
                Name = "测试",
                UserName = "Add",
                PassWord = "Add",
                Sex = true,
                BirthDay = DateTime.Now,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var u2 = new User2()
            {
                Name = "测试2",
                UserName = "Add2",
                PassWord = "Add2",
                Sex = true,
                BirthDay = DateTime.Now,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
        }

        public void AddIfNotExistsTest()
        {
            var obj = Repository.AddIfNotExists(new User()
            {
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
            //		SELECT TOP 1 Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime,Status 
            //		FROM [User] 
            //		WHERE (UserName = @P0))
        }
        public void AddListTest()
        {
            List<User> us = new List<User>();
            for (int i = 0; i < 10000; i++)
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
            Repository.AddList(us);
        }

        #endregion

        #region 查询

        //查询字段
        public void SelectTest()
        {
            var obj = Repository.Get(where => where.Name == "测试");
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User]  WHERE (Name = @P0)
            var obj2 = Repository.Get(where => where.Name == "测试", order => order.Desc(o => o.Id));
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime FROM [User] WHERE (Name == @P0) ORDER BY ID DESC 

            var obj3 = Repository.GetEx(where => where.UserName == "AddList0", u => new User());
            //SELECT Id,Name,UserName,PassWord,Sex,BirthDay,CreateTime,UpdateTime,Status FROM [User]  WHERE (UserName = @P0) 

            var obj4 = Repository.GetEx(where => where.UserName == "AddList0", order => order.Desc(o => o.Id), u => new User() { Id = 0, Name = "" });
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

            //var obj15 = Repository.SelectIn(null,u=>new User(),c=>c.Id,new int[] {1,2,3 },10);
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

            var obj6 = Repository.Get(where => where.Name.Contains("测"));
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
            User user = new User() { Id = 1 };
            var obj0 = Repository.Update(c => c.Id > user.Id, o => new User() {  Sex = true, Name = "测试", CreateTime = DateTime.Now });
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

            var obj2 = Repository.Update(c => c.Id > 0,
                o => new User()
                {
                    Name = "测试",
                    UserName = "Update",
                    PassWord = "Update",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            //UPDATE [User] SET Name=@Name,UserName=@UserName,PassWord=@PassWord,Sex=@Sex,BirthDay=@BirthDay,CreateTime=@CreateTime,UpdateTime=@UpdateTime WHERE Id=@Id

            var obj3 = Repository.UpdateSelect<User>(w => w.Id > 0,
                o => new User()
                {
                    Name = "测试",
                    UserName = "UpdateSelect",
                    PassWord = "UpdateSelect",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, null, 10);
            //UPDATE TOP(10) [User] WITH(UPDLOCK, READPAST) SET Sex = @Sex, Name = @Name OUTPUT INSERTED.Id AS Id, INSERTED.Name AS Name, INSERTED.UserName AS UserName, INSERTED.PassWord AS PassWord, INSERTED.Sex AS Sex, INSERTED.BirthDay AS BirthDay, INSERTED.CreateTime AS CreateTime, INSERTED.UpdateTime AS UpdateTime WHERE (Id > @P0)

            using (var tran = new UnitOfWork().BeginTransaction())
            {
                Repository.Update(new User()
                {
                    Id = 2,
                    Name = "测试",
                    UserName = "UpdateTran1",
                    PassWord = "UpdateTran1",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, tran);

                Repository.Update(w =>
                                w.Id >= 0 &&
                                w.Name == "测试",
                                u => new User()
                                {
                                    Name = "测试",
                                    UserName = "UpdateTran2",
                                    PassWord = "UpdateTran2",
                                    Sex = true,
                                    BirthDay = DateTime.Now,
                                    CreateTime = DateTime.Now,
                                    UpdateTime = DateTime.Now
                                }, tran);
                tran.Commit();
            }
        }
        #endregion

        #region 删除
        public void DeleteTest()
        {
            Repository.Delete(new User() { Id = 1 });          //DELETE FROM [User] WHERE Id=@Id
            Repository.Delete(w => w.Id > 0);                  //DELETE FROM [User]  WHERE (Id > @P0)

        }
        #endregion

        #region 事务

        public void LoacalTransactionTest()
        {
            List<User> us = new List<User>();
            for (int i = 0; i < 1; i++)
            {
                us.Add(new User()
                {
                    Name = "测试",
                    UserName = "AddList_LoacalTransaction" + i,
                    PassWord = "AddList_LoacalTransaction" + i,
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

            using (var unit = new UnitOfWork())
            {
                var tran = unit.BeginTransaction();
                Repository2.Add(new User2()
                {
                    Name = "测试",
                    UserName = "Exists_LoacalTran",
                    PassWord = "Exists_LoacalTran",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, tran);
                var id = tran.Commit(TransactionType.LocalDistribute);
                if (id > 0)
                {
                    var tran2 = unit.BeginTransaction();
                    Repository.Add(new User()
                    {
                        Name = "Add_LoacalDisTran",
                        UserName = "Add_LoacalDisTran",
                        PassWord = "Add_LoacalDisTran",
                        Sex = true,
                        BirthDay = DateTime.Now,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    },tran2);
                    var id2 = tran2.Commit();
                    if (id2 > 0)
                    {
                        Repository.Update(w => w.UserName == "Add_LoacalDisTran", u => new User()
                        {
                            Name = "测试分布式事务修改",
                            UserName = "Add_LoacalDisTran测试分布式事务修改",
                            PassWord = "Add_LoacalDisTran测试分布式事务修改",
                            Sex = true,
                            BirthDay = DateTime.Now,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        }, unit.BeginTransaction());
                    }
                }
                var effected=unit.Commit();
            }
        }
        //分布式事务
        public void DistributedTransactionTest()
        {
            using (var unit = new UnitOfWork(TransactionType.Distribute))
            {
                var tran = unit.BeginTransaction();
                Repository2.Add(new User2()
                {
                    Name = "测试",
                    UserName = "Exists_LoacalTran",
                    PassWord = "Exists_LoacalTran",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, tran);
                Repository.Add(new User()
                {
                    Name = "测试分布式事务增加",
                    UserName = "Add_LoacalDisTran测试分布式事务增加",
                    PassWord = "Add_LoacalDisTran测试分布式事务增加",
                    Sex = true,
                    BirthDay = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                }, tran);
                var effected = unit.Commit();
            }
        }

        #endregion

        #region 存储过程

        public void StoreProcedureTest()
        {

            var obj=Repository.QueryStoreProcedure<User>("UP_GetRecordByPage", new DbParameter[] {
                new SqlParameter("@tblName","User"),
                new SqlParameter("@fldName","Id"),
                new SqlParameter("@PageIndex","3"),
                new SqlParameter("@strWhere","Id>0"),
            });

            var obj2 = Repository.StoreProcedure("UP_GetRecordByPage",new DbParameter[] {
                new SqlParameter("@tblName","User"),
                new SqlParameter("@fldName","Id"),
                new SqlParameter("@PageIndex","3"),
                new SqlParameter("@strWhere","Id>0"),
            });
        }


        #endregion
    }
}
