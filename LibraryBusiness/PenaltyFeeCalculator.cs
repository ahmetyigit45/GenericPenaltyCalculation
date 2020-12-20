using System;
using System.Collections.Generic;
using System.Text;
using LibraryConfigUtilities;
using System.Configuration;
using System.Xml;


namespace LibraryBusiness
{
    /* Description,
     * settingList member holds configuration parameters stored in the App.config file, 
     * please explore the properties and methods in the Country class to get a better understanding.
     * 
     * Please implement this class accordingly to accomplish requirements.
     * Feel free to add any parameters, methods, class members, etc. if necessary
     */
    public class PenaltyFeeCalculator{
        
        private List<Country> settingList = new LibrarySetting().LibrarySettingList;
        
        public PenaltyFeeCalculator() {
            
        }
        public static double PenaltyFee(DateTime DateStart, DateTime DateEnd, String CountryCode, XmlElement CountryElement)
        {
            //Country alt�ndaki attribute leri �ekiyoruz
            double DailyPenaltyFee = Convert.ToDouble(CountryElement.GetAttribute("DailyPenaltyFee"));
            int PenaltyAppliesAfter = Convert.ToInt32(CountryElement.GetAttribute("PenaltyAppliesAfter"));

            //WeekendSetting alt�nda  Weekend'in indislerini al�p Weekend dizisine at�yoruz.
            XmlNodeList WeekendNodeList = (CountryElement.GetElementsByTagName("WeekendSetting").Item(0) as XmlElement).GetElementsByTagName("Weekend");
            int[] Weekend = new int[WeekendNodeList.Count];
            int ind = 0;
            foreach (XmlElement WeekendElement in WeekendNodeList)
            {
                Weekend[ind++] = Convert.ToInt32(WeekendElement.GetAttribute("Day"));
            }
            //weekend alt�nda bulunan Holiday Date'lerini  al�p Holidays dizisine at�yoruz.
            XmlNodeList HolidayNodeList = (CountryElement.GetElementsByTagName("HolidaySetting").Item(0) as XmlElement).GetElementsByTagName("Holiday");
            DateTime[] Holidays = new DateTime[HolidayNodeList.Count];
            ind = 0;
            foreach (XmlElement HolidayElement in HolidayNodeList)
            {
                Holidays[ind++] = DateTime.ParseExact(HolidayElement.GetAttribute("Date"), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
            }


            //      �rnek tarih kontrol: 20/11/2009 - 7/12/2009 
            //                      x   x   x
            //20  21   22   23  24  25  26  27  28  29  30  1   2   3   4   5   6   7
            //5    6    0   1   2   3   4   5   6   0   1   2   3   4   5   6   0   1   TR
            //              1   2                       3   4   5   6   7           8   TR i�in toplam 8 g�n ceza 3 g�n d��ersek toplamda 5 g�n ceza
            //                      x   x   x
            //20  21   22   23  24  25  26  27  28  29  30  1   2   3   4   5   6   7
            //5    6    0   1   2   3   4   5   6   0   1   2   3   4   5   6   0   1   AED
            //          1   2   3                   4   5   6   7   8           9   10  AED i�in toplam 10 g�n ceza 4 g�n d��ersek toplamda 6 g�n ceza 



            //Ceza G�n�n� Hesaplamas� i�in gerekli Fonksiyon
            int DayCount = DateCalculator(DateStart, DateEnd, Weekend, Holidays);


            //bu sat�rda �lke bazl� olarak gecikme aff� olan g�n� toplam g�nden ��kart�yoruz.
            int PenaltyDayCount = DayCount > PenaltyAppliesAfter ? DayCount - PenaltyAppliesAfter : 0;
            Console.WriteLine("Cezal� g�n say�s�: {0}", PenaltyDayCount);

            /*e�er PenaltyDayCount 0'a e�it ise ceza yemeden kitab� teslim etmi� olmakta
            e�er de�ilse xlm den �ekti�imiz ceza tutar� PenaltyDatCount ile �arp�l�r ve ceza hesaplan�r*/
            double SumFee = 0;
            if (PenaltyDayCount == 0)
            {
                Console.WriteLine("Kitap zaman�nda ya da af s�resinde teslim edilmi�tir ceza yoktur.");
            }
            else
            {
                //Console.WriteLine("Ceza: " + (PenaltyDayCount * DailyPenaltyFee));
                SumFee = PenaltyDayCount * DailyPenaltyFee;
            }
            return SumFee;
        }

        //Burada Ba�lang�� ve Biti� tarihi aras�nda Haftasonu ve Bayramlara denk gelen tarihleri ��kart�yoruz.
        public static int DateCalculator(DateTime DateStart, DateTime DateEnd, int[] Weekend, DateTime[] Holidays)
        {
            DateTime CurrentDate = DateStart.AddDays(1);
            int DayCount = 0;
            while (CurrentDate <= DateEnd)
            {
                if (Array.IndexOf(Weekend, (int)CurrentDate.DayOfWeek) < 0 && Array.IndexOf(Holidays, CurrentDate) < 0)
                {
                    DayCount++;
                }
                CurrentDate = CurrentDate.AddDays(1);
            }
            return DayCount;
        }



        public String Calculate(){
            return "not implemented yet";
        }
    }
}
