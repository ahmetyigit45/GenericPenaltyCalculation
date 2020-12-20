using System;
using System.Collections.Generic;
using System.Text;
using LibraryBusiness;
using System.Configuration;
using System.Xml;

namespace LibraryApplication
{
    public class Program{
        static void Main(string[] args){
            if (args == null || args.Length != 3) {
                PrintUsage();
                return;
            }
            String fee = "";
            DateTime DateStart=new DateTime();
            DateTime DateEnd=new DateTime();
            string CountryCode="";
            XmlElement CountryElement = null;

            try
            {
                /* Description,
                 * please implement PenaltyFeeCalculator class
                 * feel free to make any changes (if necessary) 
                 * to PenaltyFeeCalculator class method and constructor signatures,
                 * as well as here this very portion of the main method
                 * You should not need to change any other methods in this class.
                 */
                

                Console.WriteLine("Lütfen Tarihi: Gün/Ay/Yýl olarak girin!");
                Console.WriteLine("--------------------------------------");

                
                //tarih kontrolü
                int DateControl = 0;
                while (DateControl<=0)
                {
                    Console.Write("Baþlangýç tarihini giriniz: ");
                    DateStart = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Bitiþ tarihini giriniz: ");
                    DateEnd = Convert.ToDateTime(Console.ReadLine());
                    if (DateStart>DateEnd)
                    {
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Baþlangýç Tarihi Bitiþ tarihinden büyük olamaz Yeniden Tarih Gir");
                    }
                    else
                    {
                        while (DateControl<=0)
                        {
                            Console.Write("Türkiye için TRY, Birleþik Arap Emirlikleri için AED  yazýnýz:");
                            CountryCode = Console.ReadLine();
                            XmlNodeList CountryNodeList = (ConfigurationManager.GetSection("LibrarySetting") as XmlElement).GetElementsByTagName("Country");
                            foreach (XmlElement CountryCurrency in CountryNodeList)
                            {
                                if (CountryCode.Equals(CountryCurrency.GetAttribute("Currency")))
                                {
                                    DateControl++;//þart saðlandý döngü tamalanmasý ve bir daha girmemesi gerekiyor burda DateControl++ yapýyoruz
                                    CountryElement = CountryCurrency;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Girilen ülke kodu App.COnfig içerisinde yer almamaktadýr");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("\n-------------------------------------------------\n");
                Console.WriteLine("Baþlangýç Tarihi: {0} \n" +
                    "Bitiþ Tarihi: {1} \n" +
                    "Ükle Kodu: {2}", DateStart.ToShortDateString(), DateEnd.ToShortDateString(), CountryCode);
                Console.WriteLine("\n-------------------------------------------------\n");
                fee = PenaltyFeeCalculator.PenaltyFee(DateStart, DateEnd, CountryCode, CountryElement).ToString();
            }
            catch (Exception e) {
                PrintErrorMessage(e);
            }
            PrintResultMessage(fee);
           
        }
        private static void PrintUsage(){
            Console.WriteLine("Please provide these parameters (without brackets) : <CountryCode> <DateStart> <DateEnd>");
            PrintAnyKeyMessage();
            Console.ReadKey();
        }
        private static void PrintAnyKeyMessage(){
            Console.WriteLine("Press any key to continue");
        }
        private static void PrintResultMessage(string fee){
            Console.WriteLine("Penalty Fee is {0}", fee);
            PrintAnyKeyMessage();
            Console.ReadKey();
        }
        private static void PrintErrorMessage(Exception e) {
            Console.WriteLine("Exception : " + e.Message);
            Console.WriteLine("Stacktrace : ");
            Console.WriteLine(e.StackTrace);
            PrintAnyKeyMessage();
            Console.ReadKey();
        }
    }

}
