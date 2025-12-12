using System;

namespace func_rocket;

public class ForcesTask
{
    /// <summary>
    /// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
    /// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
    /// </summary>
    public static RocketForce GetThrustForce(double forceValue) => r =>
    {
        var directionVector = new Vector(Math.Cos(r.Direction), Math.Sin(r.Direction));
        return directionVector * forceValue;
    };

    /// <summary>
    /// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
    /// </summary>
    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) => r =>
    {
        return gravity(spaceSize, r.Location);
    };



    /// <summary>
    /// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
    /// </summary>
    public static RocketForce Sum(params RocketForce[] forces) => r =>
    {
        Vector total = Vector.Zero;
        foreach (var forcesItem in forces)
        {
            total += forcesItem(r);
        }
        return total;
    };
}