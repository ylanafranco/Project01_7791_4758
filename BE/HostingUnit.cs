using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    public static class Tools
    {
        public static T[] Flatten<T>(this T[,] arr) 
        {
            //int rows = arr.GetLength(0); 
            //int columns = arr.GetLength(1); 
            //T[] arrFlattened = new T[rows * columns]; 
            //for (int j = 0; j < columns; j++) 
            //{ 
            //    for (int i = 0; i < rows; i++) 
            //    { 
            //        var test = arr[i, j]; 
            //        arrFlattened[i + j * rows] = arr[i, j]; 
            //    } 
            //} 
            //return arrFlattened; 
            int rows = arr.GetLength(0); 
            int columns = arr.GetLength(1); 
            T[] arrFlattened = new T[rows * columns]; 
            for (int j = 0; j < columns; j++) 
            { 
                for (int i = 0; i < rows; i++) 
                { var test = arr[i, j]; 
                    arrFlattened[i + j * rows] = arr[i, j]; 
                } 
            }
            return arrFlattened;
        }

        public static T[,] Expand<T>(this T[] arr, int rows) 
        {
            //int length = arr.GetLength(0); 
            //int columns = length / rows; 
            //T[,] arrExpanded = new T[rows, columns]; 
            //for (int j = 0; j < rows; j++) 
            //{ 
            //    for (int i = 0; i < columns; i++) 
            //    { 
            //        arrExpanded[i, j] = arr[i + j * rows]; 
            //    } 
            //} 
            //return arrExpanded; 

            int length = arr.GetLength(0); 
            int columns = length / rows; 
            T[,] arrExpanded = new T[rows, columns]; 
            for (int i = 0; i < rows; i++) 
            { 
                for (int j = 0; j < columns; j++) 
                { arrExpanded[i, j] = arr[i + j * rows]; 
                } 
            }
            return arrExpanded;
        }
    }

    


    [Serializable]
    public class HostingUnit : Enumeration
    {
        private long HostingUnitkey;
        public long HostingUnitKey
        {
            get { return HostingUnitkey; }
            set { HostingUnitkey = value; }
        }

        private Host owner;
        public Host Owner { get { return owner; } set { owner = value; } }

        private string HostingUnitname;
        public string HostingUnitName { get { return HostingUnitname; } set { HostingUnitname = value; } }

        private Area areaa;
        public Area Areaa { get { return areaa; } set { areaa = value; } }

        private Type typee;
        public Type Typee { get { return typee; } set { typee = value; } }

        private int room;
        public int Room { get { return room; } set { room = value; } }

        
        private bool[,] diary = new bool[12,31];
        [XmlIgnore]
        public bool[,] Diary
        {
            get { return diary; }
            set { diary = new bool[12, 31]; }
        }

        [XmlArray("Diary")]
        public bool[] DiaryTo
        {
            get { return diary.Flatten(); }
            set { diary = value.Expand(12) ;}         
        } 




        public List<string> Uris { get; set; }

        private int bed;
        public int Bed { get { return bed; } set { bed = value; } }


        public bool Pool { get; set; }
        public bool Spa { get; set; }
        public bool SportRoom { get; set; }
        public bool Parking { get; set; }
        public bool KidsClub { get; set; }
        public bool PetsAccepted { get; set; }
        public bool WifiIncluded { get; set; }
        public bool HandicapAccess { get; set; }
        public bool FoodIncluded { get; set; }
        public bool AirConditionner { get; set; }

        private double pricePerNight;
        public double PricePerNight { get { return pricePerNight; } set { pricePerNight = value; } }

        

        public override string ToString()
        {
            string s = ("Hosting Unit Details : " +
                "\nHosting Unit Key : " + HostingUnitKey +
                "\n Hosting Unit Name : " + HostingUnitName
                + "\nOwner : \n" + Owner.ToString()
                + "\nType : " + typee
                + "\nArea : " + areaa
                + "\nNumber of rooms :" + room
                + "\nNumber of beds :" + bed
                + "\nPrice per night :" + pricePerNight);
              //  + "\n pool :" + Pool.ToString());
            return s;
        }



    }
}
