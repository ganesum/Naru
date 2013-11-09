using System;

namespace Naru.WPF.Wizard
{
    public class WizardStepIdentifier : IEquatable<WizardStepIdentifier>
    {
        public int Ordinal { get; private set; }

        public string Name { get; private set; }

        public WizardStepIdentifier(int ordinal, string name)
        {
            Name = name;
            Ordinal = ordinal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WizardStepIdentifier) obj);
        }
        public bool Equals(WizardStepIdentifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Ordinal == other.Ordinal && string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Ordinal * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(WizardStepIdentifier left, WizardStepIdentifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WizardStepIdentifier left, WizardStepIdentifier right)
        {
            return !Equals(left, right);
        }
    }
}