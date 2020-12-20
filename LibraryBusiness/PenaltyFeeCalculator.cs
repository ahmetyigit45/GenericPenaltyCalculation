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
            //Country altýndaki attribute leri çekiyoruz
            double DailyPenaltyFee = Convert.ToDouble(CountryElement.GetAttribute("DailyPenaltyFee"));
            int PenaltyAppliesAfter = Convert.ToInt32(CountryElement.GetAttribute("PenaltyAppliesAfter"));

            //WeekendSetting altýnda  Weekend'in indislerini alýp Weekend dizisine atýyoruz.
            XmlNodeList WeekendNodeList = (CountryElement.GetElementsByTagName("WeekendSetting").Item(0) as XmlElement).GetElementsByTagName("Weekend");
            int[] Weekend = new int[WeekendNodeList.Count];
            int ind = 0;
            foreach (XmlElement WeekendElement in WeekendNodeList)
            {
                Weekend[ind++] = Convert.ToInt32(WeekendElement.GetAttribute("Day"));
            }
            //weekend altýnda bulunan Holiday Date'lerini  alýp Holidays dizisine atýyoruz.
            XmlNodeList HolidayNodeList = (CountryElement.GetElementsByTagName("HolidaySetting").Item(0) as XmlElement).GetElementsByTagName("Holiday");
            DateTime[] Holidays = new DateTime[HolidayNodeList.Count];
            ind = 0;
            foreach (XmlElement HolidayElement in HolidayNodeList)
            {
                Holidays[ind++] = DateTime.ParseExact(HolidayElement.GetAttribute("Date"), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
            }


            //      örnek tarih kontrol: 20/11/2009 - 7/12/2009 
            //                      x   x   x
            //20  21   22   23  24  25  26  27  28  29  30  1   2   3   4   5   6   7
            //5    6    0   1   2   3   4   5   6   0   1   2   3   4   5   6   0   1   TR
            //              1   2                       3   4   5   6   7           8   TR için toplam 8 gün ceza 3 gün düþersek toplamda 5 gün ceza
            //                      x   x   x
            //20  21   22   23  24  25  26  27  28  29  30  1   2   3   4   5   6   7
            //5    6    0   1   2   3   4   5   6   0   1   2   3   4   5   6   0   1   AED
            //          1   2   3                   4   5   6   7   8           9   10  AED için toplam 10 gün ceza 4 gün düþersek toplamda 6 gün ceza 



            //Ceza Gününü Hesaplamasý için gerekli Fonksiyon
            int DayCount = DateCalculator(DateStart, DateEnd, Weekend, Holidays);


            //bu satýrda ülke bazlý olarak gecikme affý olan günü toplam günden çýkartýyoruz.
            int PenaltyDayCount = DayCount > PenaltyAppliesAfter ? DayCount - PenaltyAppliesAfter : 0;
            Console.WriteLine("Cezalý gün sayýsý: {0}", PenaltyDayCount);

            /*eðer PenaltyDayCount 0'a eþit ise ceza yemeden kitabý teslim etmiþ olmakta
            eðer deðilse xlm den çektiðimiz ceza tutarý PenaltyDatCount ile çarpýlýr ve ceza hesaplanýr*/
            double SumFee = 0;
            if (PenaltyDayCount == 0)
            {
                Console.WriteLine("Kitap zamanýnda ya da af süresinde teslim edilmiþtir ceza yoktur.");
            }
            else
            {
                //Console.WriteLine("Ceza: " + (PenaltyDayCount * DailyPenaltyFee));
                SumFee = PenaltyDayCount * DailyPenaltyFee;
            }
            return SumFee;
        }

        //Burada Baþlangýç ve Bitiþ tarihi arasýnda Haftasonu ve Bayramlara denk gelen tarihleri çýkartýyoruz.
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
