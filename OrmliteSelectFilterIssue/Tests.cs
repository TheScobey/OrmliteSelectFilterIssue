using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.OrmLite;

namespace OrmLiteIssueWithAliases
{
    [TestFixture]
    public class Tests
    {
        public OrmLiteConnectionFactory factory;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            factory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);

            using (var db = factory.OpenDbConnection())
            {
                db.DropAndCreateTable<FirstTable>();
                db.DropAndCreateTable<SecondTable>();

                var a = new FirstTable()
                {
                    Deleted = false
                };

                db.Save(a);

                db.Insert(new SecondTable()
                {
                    FirstTableId = a.Id,
                    Deleted = false
                });
            }
            
        }
        
        [Test]
        public void Test_Aliases()
        {
            OrmLiteConfig.SqlExpressionSelectFilter = q =>
            {
                if (q.ModelDef.ModelType.HasInterface(typeof(IJoinFilter)))
                {
                    q.LeftJoin<IJoinFilter, FirstTable>((f, j) => f.FirstTableId == j.Id)
                        .Where<FirstTable>(j => j.Deleted != true);
                }
            };

            using (var db = factory.OpenDbConnection())
            {
                var q = db.From<SecondTable>()
                    .Where<SecondTable>(x => x.Id == 1);
                
                var result = db.Select(q);
            }

            OrmLiteConfig.SqlExpressionSelectFilter = null;
        }
    }
}