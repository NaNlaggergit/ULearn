using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new();

    public static IEnumerable<Level> CreateLevels()
    {
        var rocketStart = new Vector(200, 500);
        var zeroTarget = new Vector(600, 200);
        yield return new Level("Zero",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (size, v) => Vector.Zero,
            standardPhysics);
        yield return new Level("Heavy",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (size, v) => new Vector(0, 0.9),
            standardPhysics);
        yield return new Level("Up",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(700, 500),
            (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300)),
            standardPhysics);
        yield return new Level("WhiteHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (size, v) =>
            {
                var toWhiteHole = new Vector(v.X - 600, v.Y - 200);
                return toWhiteHole.Normalize() * 140 * toWhiteHole.Length
                / (toWhiteHole.Length * toWhiteHole.Length + 1);
            },
            standardPhysics);
        yield return new Level("BlackHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (size, v) =>
            {
                var blackHolePosition = new Vector((600 + 200) / 2, (500 + 200) / 2);
                var toBlackHole = blackHolePosition - v;
                return new Vector(blackHolePosition.X - v.X, blackHolePosition.Y - v.Y).Normalize() *
                    300 * toBlackHole.Length / (toBlackHole.Length * toBlackHole.Length + 1);
            },
            standardPhysics);
        yield return new Level(
            "BlackAndWhite",
            new Rocket(rocketStart, Vector.Zero, -0.5 * Math.PI),
            zeroTarget,
            (size, v) =>
            {
                var dirWhite = zeroTarget - v;
                double dWhite = dirWhite.Length;
                double gWhite = 140 * dWhite / (dWhite * dWhite + 1);
                Vector fWhite = dirWhite.Normalize() * gWhite;

                var dirBlack = anomaly - v;
                double dBlack = dirBlack.Length;
                double gBlack = 300 * dBlack / (dBlack * dBlack + 1);
                Vector fBlack = dirBlack.Normalize() * gBlack;

                return (fWhite + fBlack) * 0.5;
            },
            standardPhysics
        );
    }
}