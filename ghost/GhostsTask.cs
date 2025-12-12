using System;
using System.Collections.Generic;
using System.Text;

namespace hashes;

public class GhostsTask : 
	IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
	IMagic
{
    private List<object> _ghosts = new List<object>();
    public void DoMagic()
	{
        foreach (var ghost in _ghosts)
        {
            switch (ghost)
            {
                case Vector v:
                    v.Add(new Vector(1, 1));
                    break;
                case Segment s:
                    s.Start.Add(new Vector(1, 1));
                    break;
                case Cat c:
                    c.Rename("yeruqrtwefkj");
                    break;
                case Document d:
                    var bytesField = typeof(Document).GetField("content", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var bytes = (byte[])bytesField.GetValue(d);
                    bytes[0] += 1;
                    break;
                case Robot r:
                    Robot.BatteryCapacity += 1;
                    break;
            }
        }
    }


	Vector IFactory<Vector>.Create()
	{
        var v = new Vector(0, 0);
        _ghosts.Add(v);
        return v;
    }

	Segment IFactory<Segment>.Create()
	{
        var s = new Segment(new Vector(0, 0), new Vector(0, 0));
        _ghosts.Add(s);
        return s;
    }
    Document IFactory<Document>.Create()
    {
        var content = new byte[] { 0 };
        var doc = new Document("Ghost", Encoding.UTF8, content);
        _ghosts.Add(doc);
        return doc;
    }

    Cat IFactory<Cat>.Create()
    {
        var c = new Cat("Kitty", "Siamese", DateTime.MaxValue);
        _ghosts.Add(c);
        return c;
    }

    Robot IFactory<Robot>.Create()
    {
        var r = new Robot("R-001");
        _ghosts.Add(r);
        return r;
    }
}