using DevelopHelpers;
using System;
using System.Collections.Generic;
using TestModel;


namespace QueueTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"F:\Project\MyTest\MyTest\OutputTest\bin\Debug\Test.xml";
            string bnpath = @"F:\Project\MyTest\MyTest\OutputTest\bin\Debug\bnTest.txt";
            string jpath = @"F:\Project\MyTest\MyTest\OutputTest\bin\Debug\jTest.txt";
            double[] data = new double[] {10.2,15.3,4,51.2,5,4,0,-5.3,5,8};
            List<int> list = new List<int> { 6, 2, 48, 8, 6, 7, 4, };
            data.BubbleSort();
            list.InsertSort();
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            People p = new People();
            p.Name = "李静";
            p.Sex = "女";
            var t =  ConvertHelpers.CreateInstanceByBase<Teacher,People>(p);
            ///启动队列
            BusinessInfoHelper.Instance.start(StartThread);
            BusinessInfoHelper.Instance.AddQueue("胡大帅3", "666666676666");
            BusinessInfoHelper.Instance.AddQueue("胡大帅","6666666666");
            BusinessInfoHelper.Instance.AddQueue("胡大帅2", "66664666666");

            List<SerializationModel> modellist = new List<SerializationModel>()
            {
                new SerializationModel() { name="just fly",passWord="flyboy"},
                new SerializationModel() { name="just fly1",passWord="flyboy1"},
                new SerializationModel() { name="just fly2",passWord="flyboy2"}
            };
            List<SerializationModel> nmodellist = new List<SerializationModel>();
            foreach (var item in modellist)
            {
                nmodellist.Add(BinarySerializationHelper.DeepClone(item));
            }
            modellist[1].name = "hu";
            SerializationModel model = BinarySerializationHelper.DeepClone(new SerializationModel() { name="just fly",passWord="flyboy"});

            XmlSerializationHelper.SerializeToXml<List<SerializationModel>>(modellist,path);
            var li= XmlSerializationHelper.DeSerializeFromXml<List<SerializationModel>>(path , true);
            var ss = XmlSerializationHelper.SerializeToXml<List<SerializationModel>>(modellist);
            var sss = XmlSerializationHelper.DeSerializeFromXml<List<SerializationModel>>(ss);
            
            var bt = BinarySerializationHelper.FormatterObjectBytes(modellist);
            BinarySerializationHelper.BinaryFileSave(bnpath, modellist);

            var json = JsonSerializationHelper.SerializeToJson(modellist);
            JsonSerializationHelper.SerializeToJson(modellist, jpath);

            var dejson = JsonSerializationHelper.DeSerializeFromJson<List<SerializationModel>>(json);
            var pdejson = JsonSerializationHelper.DeSerializeFromJson<List<SerializationModel>>(jpath,true);
            Console.Read();
        }

        private static void StartThread()
        {
            //throw new NotImplementedException();
        }
    }
}
