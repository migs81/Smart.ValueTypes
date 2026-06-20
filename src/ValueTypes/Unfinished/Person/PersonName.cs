using System;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Unfinished.Person
{
    /// <summary>
    /// Value type for email addresses.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, EmailAddress&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="FormatException"></exception>
    public readonly record struct PersonName : IValueType<string, string, string, PersonName>
    {
        #region properties

        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }

        #endregion

        #region constructor

        public PersonName()
        {
            FirstName = "";
            MiddleName = "";
            LastName = "";
        }
        public PersonName(string firstName, string lastName) : this(firstName, "", lastName)
        {
        }
        public PersonName(string firstName, string middleName, string lastName)
        {
            _ = firstName ?? throw new ArgumentNullException(nameof(firstName));
            _ = middleName ?? throw new ArgumentNullException(nameof(middleName));

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;

        }
        private PersonName(ref string firstName, ref string middleName, ref string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }

        #endregion

        #region operator

        public static bool operator ==(PersonName left, string right) => left.Equals(right);
        public static bool operator !=(PersonName left, string right) => !left.Equals(right);

        public static implicit operator string(PersonName name) => $"{name.FirstName} {name.MiddleName} {name.LastName}";

        #endregion

        #region public methods

        public static PersonName From(string firstName, string lastName) => new(firstName, lastName);
        public static PersonName From(string firstName, string middleName, string lastName) => new(firstName, middleName, lastName);
        public static bool TryFrom(string firstName, string lastName, out PersonName? output)
        {
            try
            {
                if (firstName is not null && lastName is not null)
                {
                    string empty = "";
                    output = new PersonName(ref firstName, ref empty, ref lastName);
                    return true;
                }

                output = null;
                return true;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }
        public static bool TryFrom(string firstName, string middleName, string lastName, out PersonName? output)
        {
            try
            {
                if (firstName is not null && middleName is not null && lastName is not null)
                {
                    output = new PersonName(ref firstName, ref middleName, ref lastName);
                    return true;
                }

                output = null;
                return true;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }

        public bool Equals(string firstName, string middleName, string lastName)
            => FirstName.Equals(firstName)
            && MiddleName.Equals(middleName)
            && LastName.Equals(lastName);

        #endregion
    }
}
