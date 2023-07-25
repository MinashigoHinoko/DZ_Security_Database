namespace Festival_Manager
{
    internal class cEquipment
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Position { get; set; }
        public override string ToString()
        {
            return $"{ID} - {Name}, {Color} - {Position}";  // or just "{ID}" if you want
        }

        public override bool Equals(object obj)
        {
            // If the object is null, return false
            if (obj == null)
            {
                return false;
            }

            // If the object cannot be cast to cEquipment, return false
            cEquipment otherEquipmentID = obj as cEquipment;
            if (otherEquipmentID == null)
            {
                return false;
            }

            // Return true if the ID fields match
            return ID.Equals(otherEquipmentID.ID);
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
