using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace Fusion_
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Title = "Fusion_";
                string title = @"
   ####    #####   #####    #######  ######   ##  ##             ####    ######
  ##  ##  ##   ##   ## ##    ##   #   ##  ##  ##  ##            ##  ##   ##  ##
 ##       ##   ##   ##  ##   ## #     ##  ##   ####                 ##       ##
 ##       ##   ##   ##  ##   ####     #####     ##                ###       ##
 ##       ##   ##   ##  ##   ## #     ## ##    ####                 ##     ##
  ##  ##  ##   ##   ## ##    ##   #   ##  ##  ##  ##            ##  ##     ##
   ####    #####   #####    #######  #### ##  ##  ##             ####      ##
        
                                                                 ";

                Console.WriteLine(title);

                Console.ForegroundColor = ConsoleColor.White;
            
                Console.Write("\nXML dosyalarının yer aldığı dizinini yazınız (örnek C:/dosya/):");
                string yol = Console.ReadLine()+"\\";        //kullanıcının girdiği yolun sonuna \ ekliyoruz
                
                
                Console.Write("\n Kaç XML dosyası birleştirilecek?:");
                int num = Convert.ToInt32(Console.ReadLine());

                int dosyaSayisi = Directory.GetFiles(yol, "*.*", SearchOption.AllDirectories).Length; //dosya sayısı alma

                if (dosyaSayisi<num)   //dosya sayısı kontrol
                {
                    Console.WriteLine("bu kadar dosyanız yok");
                    Environment.Exit(0);  //program kapatma


                }

                var c = 1;
                var path = yol;
                Directory.GetFiles(path, "*.xml").ToList().ForEach(p =>
                {
                    File.Copy(p, Path.Combine(path+"1"+ "("+(c++)+")" + ".xml"), true);



                    GC.Collect();  //işlem bitse bile GC(garbage collector) işlemi hafızada tuttuğu için diğer işleme geçemiyoruz fakat bunu yaparsak işlemi GC üzerinden atmış oluruz
                    GC.WaitForPendingFinalizers();

                });
        
                XmlTextReader reader = new XmlTextReader(yol + "1(1).xml");  //xml okuyucu tanımlama ve xml dosyasını ekleme

                DataSet datset = new DataSet();  //dataset yapısı oluşturma
                datset.ReadXml(reader);    //xml okutuyorrum

                int i = 2;

                 while(i <= num)
                {
                    object[] xmltutucu = new object[] { yol, "1(", Convert.ToInt32(i), ").xml" }; 
                    XmlTextReader reader2 = new XmlTextReader(string.Concat(xmltutucu)); //concat strleri birleştirir 
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(reader2);
                    datset.Merge(dataSet);  //Belirtilen DataSet ve şemasını geçerli DataSetile birleştirir.
                    i++;

                }
                datset.WriteXml(yol + "Merge.xml");
                Console.WriteLine("işlem tamam!");
            }
            catch (Exception exception)  //bir hata oluşması durumunda 
            {
                Console.Write(exception.Message);
            }
            Console.Read();
        }
    

    }
}
