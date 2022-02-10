using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using ConsoleApp1.Helpers;
using ConsoleApp1.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace test
{
    class Scenario
    {
        public static object ScenarioID { get; private set; }
        public static MongoClient client =
            new MongoClient(
                "mongodb://RailmaxWeb:Op5K4HM1A6or@dcxrmxpoc04:27017,dcxrmxpoc05:27017,dcxrmxpoc06:27017/admin?replicaSet=MainReplicaSet&ssl=false");
        //"mongodb://127.0.0.1:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
        //"mongodb://admin:p%40ssw0rd@host.docker.internal:27017");

        private static async Task Main(string[] args)
        {
            //args = new string[4];
            //args[0] = "50000";
            //args[1] = "1";
            //args[2] = "100";

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonClassMap.RegisterClassMap<Schedule>(map =>
            {
                map.AutoMap();
                map.MapIdProperty(x => x.Id);
            });
            BsonClassMap.RegisterClassMap<Movement>(map =>
            {
                map.AutoMap();
            });

            Console.WriteLine(" args:- 1.Count,2. ScenarioID ,3. PageSize");

            List<Tuple<double, double, double, double, double, double, double>> observation =
                new List<Tuple<double, double, double, double, double, double, double>>();
            var inputList = new List<int>() {3}; 
            Console.WriteLine("Running 1 times.........................");
            for (int i = 0; i < 1 ; i++)
            {
                Console.WriteLine("Running iteration number " + (i + 1));

                double res1 = 0, res2 = 0, res3 = 0, res4 = 0, res5 = 0, res6 = 0, res7 = 0;
                for (int j = 0; j < inputList.Count; j++)
                { 
                    var input = inputList[j];
                    ; switch (input)
                    {
                        case 1:
                            int count = Convert.ToInt32(args[0]);
                            int scenarioid  = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling SAVEScheduleActual");
                            res1 = SAVEScheduleActual(count, scenarioid);

                            break;
                        case 2:
                            int scenarioid1 = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling GETScheduleActualwithoutjoin");
                            res2 = GETScheduleActualwithoutjoin(scenarioid1);
                            break;
                        case 3:
                            int scenarioid3 = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling GETScheduleActual");
                            res3 = GETScheduleActual(scenarioid3);
                            break;
                        case 4:
                            int scenarioid4 = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling UPDActualMovement");
                            res4 = UPDActualMovement(scenarioid4);
                            break;
                        case 5:
                            int scenarioid5 = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling DELScheduleActual");
                            res5 = DELScheduleActual(scenarioid5);
                            break;
                        case 6:
                            int scenarioid2 = Convert.ToInt32(args[1]);
                            int pageSize = Convert.ToInt32(args[2]);
                            
                            Console.WriteLine("Calling GETScheduleActualpagination");
                            res6 = GETScheduleActualpagination(scenarioid2,pageSize);
                            break;
                        case 7:
                            int count1 = Convert.ToInt32(args[0]);
                            int scenarioid6 = Convert.ToInt32(args[1]);
                            Console.WriteLine("Calling SAVEScheduleActualwithoutbatch");
                            res7 = SAVEScheduleActualwithoutbatch(count1,scenarioid6);
                            break;

                        default:
                            break;
                    }
                    Console.WriteLine();
                    Console.WriteLine();


                }
                var r = Tuple.Create(res1, res2, res3, res4, res5, res6, res7);
                observation.Add(r);
            }
            Console.WriteLine();
            Console.WriteLine();
            for(int i = 0; i < observation.Count; i++)
            {
                Console.WriteLine(observation[i].Item1 + "| " + observation[i].Item2 + "| " + observation[i].Item3 + "| " + observation[i].Item4 + "| " + observation[i].Item5 + "| " + observation[i].Item6 + "| " + observation[i].Item7);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Best and worst out of 1 run by a single client is :");
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Operation                        Min            |           Max            ");
            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("1. SaveScheduleActual " +observation.Select(x=>x.Item1).Min() + "          |        " + observation.Select(x => x.Item1).Max());
            Console.WriteLine("2. GETScheduleActualwithoutjoin " + observation.Select(x => x.Item2).Min() + "          |        " + observation.Select(x => x.Item2).Max());
            Console.WriteLine("3. GETScheduleActual " + observation.Select(x => x.Item3).Min() + "          |        " + observation.Select(x => x.Item3).Max());
            Console.WriteLine("4. UPDActualMovement " + observation.Select(x => x.Item4).Min() + "          |        " + observation.Select(x => x.Item4).Max());
            Console.WriteLine("5. DELScheduleActual " + observation.Select(x => x.Item5).Min() + "          |        " + observation.Select(x => x.Item5).Max());
            Console.WriteLine("6. GETScheduleActualpagination " + observation.Select(x => x.Item6).Min() + "          |        " + observation.Select(x => x.Item6).Max());
            Console.WriteLine("7. SAVEScheduleActualwithoutbatch " + observation.Select(x => x.Item7).Min() + "          |        " + observation.Select(x => x.Item7).Max());

            Console.ReadLine();


        }


        private static double SAVEScheduleActual( int count, int scenarioid)
        {
            try
            {  IMongoDatabase db = client.GetDatabase("Schedule_Dummy");

                List<Schedule> scheduleList = new List<Schedule>();

               // for (int i = 1; i < 3; i++)
                //{   
                var list = ScheduleHelper.CreateSchedules(count, scenarioid);
                    scheduleList.AddRange(list);
                //}
                var bsonScheduleList = scheduleList.Select(s => s.ToBsonDocument()).ToList();
                var batchSize = 5000;
                var noOfBatches = (count / batchSize) + (count % batchSize == 0 ? 0 : 1);
                var source = Enumerable.Range(1, noOfBatches).ToArray();

                var stopWatch = new Stopwatch();

                stopWatch.Start();
                var collection = db.GetCollection<BsonDocument>("ScheduleActual");


                bsonScheduleList.ParallelForEachAsync(
                    async item =>
                    {
                        try
                        {
                            await collection.InsertOneAsync(item);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error");
                        }
                    },noOfBatches).Wait();

                stopWatch.Stop();
                Console.WriteLine($"Time elapsed in milliseconds to write {stopWatch.Elapsed.TotalMilliseconds}");
                return stopWatch.Elapsed.TotalMilliseconds;
          
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }

            return 0;
        }

        //private static double ReadWriteScheduleActual( int count, scenarioid)
        //{
        //    var sa = SAVEScheduleActual(count, scenarioid);
        //    var sa1 = GETScheduleActual();
        //   // DropCollection();
        //    return sa + sa1;
        //}
        private static double SAVEScheduleActualwithoutbatch(int count,int scenarioid)
        {
            try
            {
                IMongoDatabase db = client.GetDatabase("Schedule_Dummy");

                List<Schedule> scheduleList = new List<Schedule>();
   
                var list = ScheduleHelper.CreateSchedules(count, scenarioid);
                scheduleList.AddRange(list);
                var bsonScheduleList = scheduleList.Select(s => s.ToBsonDocument()).ToList();
                var stopWatch = new Stopwatch();

                stopWatch.Start();
                var collection = db.GetCollection<BsonDocument>("ScheduleActual");

                bsonScheduleList.ParallelForEachAsync(
                    async item =>
                    {
                        try
                        {
                            await collection.InsertOneAsync(item);

                        }
                        catch (Exception ex)
                        {
                        }
                    }, Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0))).Wait();
                stopWatch.Stop();
                Console.WriteLine($"Time elapsed in milliseconds to write {stopWatch.Elapsed.TotalMilliseconds}");
                return stopWatch.Elapsed.TotalMilliseconds;

            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }

            return 0;
        }


        private static double GETScheduleActual(int scenarioid)
        {
            IMongoDatabase db = client.GetDatabase("Schedule_Dummy");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var collection = db.GetCollection<BsonDocument>("ScheduleActual");          

            var lookup = new BsonDocument("$lookup",
                             new BsonDocument("from", "Scenario")
                                         .Add("localField", "ScenarioID")
                                         .Add("foreignField", "ScenarioID")
                                         .Add("as", "ScheduleActualCollection"));

            var match = new BsonDocument("$match", new BsonDocument("ScenarioID", scenarioid));

            var pipeline = new[] { match, lookup };
            var option = new AggregateOptions() { AllowDiskUse = true };
            var ddTask = collection.AggregateAsync<BsonDocument>(pipeline, option).Result;

            

            //int countOfRead = 0;
            var enumerator = ddTask.ToList();// ToEnumerable().GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    countOfRead++;
            //}
            stopWatch.Stop();
            Console.WriteLine($"Record count is  {enumerator.Count}");
            Console.WriteLine($"Time elapsed in milliseconds to read {stopWatch.Elapsed.TotalMilliseconds}");
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        private static double GETScheduleActualwithoutjoin(int scenarioid)
        {
            IMongoDatabase db = client.GetDatabase("Schedule_Dummy");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var collection = db.GetCollection<BsonDocument>("ScheduleActual");
           

            var filter = Builders<BsonDocument>.Filter.Eq("ScenarioID", scenarioid);

             var res = collection.FindAsync(filter).Result;
           
            stopWatch.Stop();

            //int countOfRead = 0;

            var enumerator = res.ToList();//ToEnumerable().GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    countOfRead++;
            //}
            Console.WriteLine($"Record count is  {enumerator.Count}");

            Console.WriteLine($"Time elapsed in milliseconds to read {stopWatch.Elapsed.TotalMilliseconds}");
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        private static double GETScheduleActualpagination(int scenarioid,int pageSize)
        {
            IMongoDatabase db = client.GetDatabase("Schedule_Dummy");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var collection = db.GetCollection<BsonDocument>("ScheduleActual");

            int page = 1;
      
            List<BsonDocument> data = new List<BsonDocument>();
            var filter = Builders<BsonDocument>.Filter.Eq("ScenarioID", scenarioid);
            int dCount =0;
            while (true)
            {                
                var res = collection.Find(filter).Limit(pageSize).ToListAsync().Result;                
                Console.WriteLine($"Record count for pagination  is  {res.Count}");
                stopWatch.Stop();
                Console.WriteLine($"Time elapsed in milliseconds to read {stopWatch.Elapsed.TotalMilliseconds}");
                dCount = res.Count + dCount;
                if (dCount == 500 || res.Count < pageSize)
                    break;
                page++;
            }
            
            Console.WriteLine($"Record count is  {dCount}");

            
            return stopWatch.Elapsed.TotalMilliseconds;

        }
        private static double UPDActualMovement(int scenarioid)
        {
            IMongoDatabase db = client.GetDatabase("Schedule_Dummy");

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var collection = db.GetCollection<BsonDocument>("ScheduleActual");

            var filter = Builders<BsonDocument>.Filter.Eq("ScenarioID", scenarioid); //& (Builders<BsonDocument>.Filter.Eq("State", "Inactive") | Builders<BsonDocument>.Filter.Eq("State", "Active"));

            var update = Builders<BsonDocument>.Update.Set("Origin", "TLO5-Updated");

            var updateTask = collection.UpdateManyAsync(filter, update);
            updateTask.Wait(CancellationToken.None);
            stopWatch.Stop();
            var uResult = updateTask.Result.IsAcknowledged ? updateTask.Result.ModifiedCount : 0;
            Console.WriteLine($"Updated count is  {uResult}");

            Console.WriteLine($"Time elapsed in milliseconds to write {stopWatch.Elapsed.TotalMilliseconds}");
            return stopWatch.Elapsed.TotalMilliseconds;
        }
        private static double DELScheduleActual(int scenarioid)
        {
            IMongoDatabase db = client.GetDatabase("Schedule_Dummy");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var collection = db.GetCollection<BsonDocument>("ScheduleActual");

            var filter = Builders<BsonDocument>.Filter.Eq("ScenarioID", scenarioid);

            var delTask = collection.DeleteManyAsync(filter);
            delTask.Wait();
            stopWatch.Stop();
            delTask.Wait(CancellationToken.None);
            stopWatch.Stop();
            var dResult = delTask.Result.IsAcknowledged ? delTask.Result.DeletedCount : 0;
            Console.WriteLine($"deleted count is  {dResult}");
            Console.WriteLine($"Time elapsed in milliseconds to delete {stopWatch.Elapsed.TotalMilliseconds}");
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        //private static void DropCollection()
        //{
        //    IMongoDatabase db = client.GetDatabase("Schedule_Dummy");

        //    db.DropCollection("ScheduleActual");
        //}
    }
}
