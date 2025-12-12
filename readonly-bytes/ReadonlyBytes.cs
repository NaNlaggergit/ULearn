using System;
using System.Collections;
using System.Collections.Generic;

public class ReadonlyBytes : IEquatable<ReadonlyBytes>, IEnumerable<byte>
{
    private readonly byte[] _bytes;
    private readonly int _hashCode;

    public ReadonlyBytes()
    {
        _bytes = Array.Empty<byte>();
        _hashCode = ComputeHash(_bytes);
    }

    public ReadonlyBytes(params byte[] bytes)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));

        _bytes = new byte[bytes.Length];
        Array.Copy(bytes, _bytes, bytes.Length);

        _hashCode = ComputeHash(_bytes);
    }

    public int Length => _bytes.Length;

    public byte this[int index] => _bytes[index];

    public byte[] ToArray()
    {
        var arr = new byte[_bytes.Length];
        Array.Copy(_bytes, arr, _bytes.Length);
        return arr;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        for (int i = 0; i < _bytes.Length; i++)
            yield return _bytes[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object obj)
    {
        if (obj is null) return false;

        if (obj.GetType() != typeof(ReadonlyBytes))
            return false;

        return Equals((ReadonlyBytes)obj);
    }

    public bool Equals(ReadonlyBytes other)
    {
        if (other is null) return false;

        if (other.GetType() != typeof(ReadonlyBytes))
            return false;

        if (ReferenceEquals(this, other)) return true;

        if (_bytes.Length != other._bytes.Length)
            return false;

        for (int i = 0; i < _bytes.Length; i++)
            if (_bytes[i] != other._bytes[i])
                return false;

        return true;
    }

    public override int GetHashCode() => _hashCode;

    private static int ComputeHash(byte[] data)
    {
        unchecked
        {
            const uint offset = 2166136261u;
            const uint prime = 16777619u;

            uint hash = offset;

            for (int i = 0; i < data.Length; i++)
            {
                hash ^= data[i];
                hash *= prime;
            }

            return (int)hash;
        }
    }
    public override string ToString()
    {
        if (_bytes.Length == 0)
            return "[]";

        return "[" + string.Join(", ", _bytes) + "]";
    }

    public static bool operator ==(ReadonlyBytes a, ReadonlyBytes b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        if (a.GetType() != typeof(ReadonlyBytes) || b.GetType() != typeof(ReadonlyBytes))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ReadonlyBytes a, ReadonlyBytes b)
        => !(a == b);
}
