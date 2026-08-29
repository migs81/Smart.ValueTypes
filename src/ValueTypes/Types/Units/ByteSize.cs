using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Units
{
    /// <summary>
    /// Value type for representing and calculating data sizes (e.g., bytes, KB, MB, GB).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    public readonly record struct ByteSize : IValueType<ulong, ByteSize>
    {
        #region fields

        private readonly ulong _value;
        
        #endregion

        #region properties

        public decimal Bits => _value * 8m;
        public decimal Kibibits => _value * 8m / 1024m;
        public decimal Mebibits => _value * 8m / (1024m * 1024m);
        public decimal Gibibits => _value * 8m / (1024m * 1024m * 1024m);
        public decimal Tebibits => _value * 8m / (1024m * 1024m * 1024m * 1024m);
        
        public decimal Kilobits => _value * 8m / 1000m;
        public decimal Megabits => _value * 8m / (1000m * 1000m);
        public decimal Gigabits => _value * 8m / (1000m * 1000m * 1000m);
        public decimal Terabits => _value * 8m / (1000m * 1000m * 1000m * 1000m);
        
        public decimal Kibibytes => _value / 1024m;
        public decimal Mebibytes => _value / (1024m * 1024m);
        public decimal Gibibytes => _value / (1024m * 1024m * 1024m);
        public decimal Tebibytes => _value / (1024m * 1024m * 1024m * 1024m);
        
        public decimal Kilobytes => _value / 1000m;
        public decimal Megabytes => _value / (1000m * 1000m);
        public decimal Gigabytes => _value / (1000m * 1000m * 1000m);
        public decimal Terabytes => _value / (1000m * 1000m * 1000m * 1000m);
        
        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteSize"/> struct.
        /// </summary>
        /// <param name="value"></param>
        public ByteSize(ulong value) => _value = value;

        #endregion
        
        #region operator

        public static bool operator ==(ByteSize left, ulong right) => left.Equals(right);
        public static bool operator !=(ByteSize left, ulong right) => !left.Equals(right);

        public static implicit operator ulong(ByteSize byteSize) => byteSize._value;
        public static implicit operator ByteSize(ulong value) => new(value);

        #endregion

        #region public methods

        public static ByteSize From(uint value) => new(value);
        
        public static ByteSize From(ulong value) => new(value);

        public static ByteSize FromKibibytes(uint value) => new(value * 1024UL);                        
        public static ByteSize FromMebibytes(uint value) => new(value * 1024UL * 1024UL);                
        public static ByteSize FromGibibytes(uint value) => new(value * 1024UL * 1024UL * 1024UL);        
        public static ByteSize FromTebibytes(uint value) => new(value * 1024UL * 1024UL * 1024UL * 1024UL);
        
        public static ByteSize FromKilobytes(uint value) => new(value * 1000UL);                            
        public static ByteSize FromMegabytes(uint value) => new(value * 1000UL * 1000UL);                    
        public static ByteSize FromGigabytes(uint value) => new(value * 1000UL * 1000UL * 1000UL);            
        public static ByteSize FromTerabytes(uint value) => new(value * 1000UL * 1000UL * 1000UL * 1000UL);
        
        #endregion
    }
}
