namespace Festival_Manager
{
    public class cWorker
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ChipNumber { get; set; }
        public bool CheckInState { get; set; }
        public override string ToString()
        {
            return $"{Name} - {ChipNumber} ({Position})";  // or just "{ID}" if you want
        }

        public override bool Equals(object obj)
        {
            // If the object is null, return false
            if (obj == null)
            {
                return false;
            }

            // If the object cannot be cast to cWorker, return false
            cWorker otherWorker = obj as cWorker;
            if (otherWorker == null)
            {
                return false;
            }

            // Return true if the ID fields match
            return ID.Equals(otherWorker.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
