namespace Festival_Manager
{
    internal class cRadioEquipment
    {
        public string ID { get; set; }
        public string Permanent { get; set; }
        public int Battery { get; set; }
        public string Radio { get; set; }
        public string CamouflageHeadset { get; set; }
        public string Razor { get; set; }
        public string MickeyMouse { get; set; }
        public string Consumables { get; set; }
        public string Others { get; set; }

        public override string ToString()
        {
            List<string> equipment = new List<string>();
            if (Radio == "true") equipment.Add("Funkgerät");
            if (CamouflageHeadset == "true") equipment.Add("Tarn-Headset");
            if (Razor == "true") equipment.Add("Rasierer");
            if (MickeyMouse == "true") equipment.Add("Mikie Maus");

            return $"{ID} beinhaltet: {string.Join(", ", equipment)} und {Battery} Batterien";
        }

        public override bool Equals(object obj)
        {
            // If the object is null, return false
            if (obj == null)
            {
                return false;
            }

            // If the object cannot be cast to cRadioEquipment, return false
            cRadioEquipment otherEquipment = obj as cRadioEquipment;
            if (otherEquipment == null)
            {
                return false;
            }

            // Return true if the ID fields match
            return ID.Equals(otherEquipment.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}

