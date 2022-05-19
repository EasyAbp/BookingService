using System;

namespace EasyAbp.BookingService.AssetOccupancies;

public class BookablePeriod : IEquatable<BookablePeriod>
{
    public TimeSpan StartingTime { get; set; }

    public TimeSpan EndingTime { get; set; }

    public bool Divisible { get; set; }

    public bool IsIntersected(TimeSpan startingTime, TimeSpan endingTime)
    {
        return !(startingTime >= endingTime || endingTime <= StartingTime);
    }

    public void Merge(TimeSpan startingTime, TimeSpan endingTime)
    {
        if (startingTime < StartingTime
            && StartingTime < endingTime
            && endingTime <= EndingTime)
        {
            StartingTime = startingTime;
            EndingTime = EndingTime;
        }
        else if (StartingTime <= startingTime
                 && startingTime < EndingTime
                 && EndingTime < endingTime)
        {
            StartingTime = StartingTime;
            EndingTime = endingTime - StartingTime;
        }
        else
        {
            StartingTime = StartingTime < startingTime ? StartingTime : startingTime;
            EndingTime = EndingTime > endingTime ? EndingTime : endingTime;
        }
    }

    public bool Equals(BookablePeriod other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return StartingTime.Equals(other.StartingTime) && EndingTime.Equals(other.EndingTime) &&
               Divisible == other.Divisible;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((BookablePeriod)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = StartingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ EndingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ Divisible.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(BookablePeriod left, BookablePeriod right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BookablePeriod left, BookablePeriod right)
    {
        return !Equals(left, right);
    }
}