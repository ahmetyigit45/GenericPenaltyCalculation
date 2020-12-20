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
                

                Console.WriteLine("L�tfen Tarihi: G�n/Ay/Y�l olarak girin!");
                Console.WriteLine("--------------------------------------");

                
                //tarih kontrol�
                int DateControl = 0;
                while (DateControl<=0)
                {
                    Console.Write("Ba�lang�� tarihini giriniz: ");
                    DateStart = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Biti� tarihini giriniz: ");
                    DateEnd = Convert.ToDateTime(Console.ReadLine());
                    if (DateStart>DateEnd)
                    {
                        Console.WriteLine("-----------------------------------------------------------------");
                        Console.WriteLine("Ba�lang�� Tarihi Biti� tarihinden b�y�k olamaz Yeniden Tarih Gir");
                    }
                    else
                    {
                        while (DateControl<=0)
                        {
                            Console.Write("T�rkiye i�in TRY, Birle�ik Arap Emirlikleri i�in AED  yaz�n�z:");
                            CountryCode = Console.ReadLine();
                            XmlNodeList CountryNodeList = (ConfigurationManager.GetSection("LibrarySetting") as XmlElement).GetElementsByTagName("Country");
                            foreach (XmlElement CountryCurrency in CountryNodeList)
                            {
                                if (CountryCode.Equals(CountryCurrency.GetAttribute("Currency")))
                                {
                                    DateControl++;//�art sa�land� d�ng� tamalanmas� ve bir daha girmemesi gerekiyor burda DateControl++ yap�yoruz
                                    CountryElement = CountryCurrency;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Girilen �lke kodu App.COnfig i�erisinde yer almamaktad�r");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("\n-------------------------------------------------\n");
                Console.WriteLine("Ba�lang�� Tarihi: {0} \n" +
                    "Biti� Tarihi: {1} \n" +
                    "�kle Kodu: {2}", DateStart.ToShortDateString(), DateEnd.ToShortDateString(), CountryCode);
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
