using System;
namespace KalenderJawa
{
    public class Weton : IEquatable<Weton>
    {
        public Hari Hari { get; set; }
        public Pasaran Pasaran { get; set; }

        public bool Equals(Weton other)
        {
            return (other.Hari == Hari && other.Pasaran == Pasaran);
        }
    }
}
